using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Context;
using Application.Interfaces;
using Application.DTOs.Account;
using Application.Exceptions;
using Domain.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IdentityContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            IdentityContext context,
            IOptions<JWTSettings> jwtSettings,
            IAuthenticatedUserService authenticatedUserService)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<TokenDto> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
                if (passwordCheck)
                {
                    var token = await GenerateJwtTokenAsync(user);

                    return new TokenDto()
                    {
                        Token = token.Token,
                        RefreshToken = token.RefreshToken
                    };
                }
            }

            throw new HttpException(HttpStatusCode.Forbidden, "Не верный логин или пароль");
        }

        public async Task<string> RegisterAsync(RegisterRequest model)
        {
            if (model.Password.Length < 8)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Пароль должен состоять не менее чем из 8 символов");
            }

            var userExist = await _userManager.FindByNameAsync(model.UserName);

            if (userExist != null)
            {
                throw new HttpException(HttpStatusCode.Conflict, "Пользователь с таким логином уже существует");
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now
            };

            await _userManager.CreateAsync(user, model.Password);

            return "Пользователь успешно создан";
        }

        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var validatedToken = GetPrincipalFromToken(request.Token);
            if (validatedToken == null)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Неверный токен");
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Срок действия этого токена еще не истек");
            }

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == request.RefreshToken);
           
            if (storedRefreshToken == null)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Этот токен обновления не существует");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Срок действия этого токена обновления истек");
            }

            if (storedRefreshToken.Used)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Этот токен обновления был использован");
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            
            if (storedRefreshToken.JwtId != jti)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Этот токен обновления не соответствует этому JWT");
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "UserId").Value);

            return await GenerateJwtTokenAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,
                    ValidAudience = _jwtSettings.ValidAudience,
                    ValidIssuer = _jwtSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
                };
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    throw new SecurityTokenException("Передан неверный токен");
                }

                return principal;
            }
            catch
            {
                throw new Exception();
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<TokenDto> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_jwtSettings.ExpiresMinutes)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new TokenDto()
            {
                Token = tokenString,
                RefreshToken = refreshToken.Token,
            };
        }

        public async Task<ICollection<string>> GetRolesAsync()
        {
            var user = await _userManager.FindByIdAsync(_authenticatedUserService.UserId.ToString());
            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }
    }
}
