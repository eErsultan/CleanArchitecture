using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Constants;
using WebAPI.Controllers.Base;
using Application.DTOs.Common;
using Application.DTOs.Account;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// Авторизоваться
        /// </summary>
        /// <response code="200">Успешная операция</response>
        /// <response code="403">Не верный логин или пароль</response>
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status403Forbidden)]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Account.Login)]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            return Ok(await _service.LoginAsync(model));
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <response code="200">Успешная операция</response>
        /// <response code="403">Пароль должен состоять не менее чем из 8 символов</response>
        /// <response code="409">Пользователь с таким логином уже существует</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Account.Register)]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            return Ok(await _service.RegisterAsync(model));
        }

        /// <summary>
        /// Обновить токен
        /// </summary>
        /// <param name="request">token and refreshToken</param>
        /// <response code="200">Успешная операция</response>
        /// <response code="403">Неверный токен</response>
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status403Forbidden)]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Account.RefreshToken)]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            return Ok(await _service.RefreshTokenAsync(request));
        }

        /// <summary>
        /// Получить роли
        /// </summary>
        /// <response code="200">Успешная операция</response>
        /// <response code="404">Пользователь не найден</response>
        [ProducesResponseType(typeof(ICollection<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [HttpGet(ApiRoutes.Account.Roles)]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _service.GetRolesAsync());
        }
    }
}