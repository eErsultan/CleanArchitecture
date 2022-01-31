using Application.DTOs.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<TokenDto> LoginAsync(LoginRequest model);
        Task<string> RegisterAsync(RegisterRequest model);
        Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request);
        Task<ICollection<string>> GetRolesAsync();
    }
}