using Application.Interfaces;

namespace UnitTests.Common.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public int UserId { get; }

        public AuthenticatedUserService()
        {
            UserId = 124;
        }
    }
}