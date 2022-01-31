using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Interfaces;

namespace WebAPI.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("UserId"), out int userId);
            UserId = userId;
        }

        public int UserId { get; }
    }
}
