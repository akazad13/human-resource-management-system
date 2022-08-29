using HRMS.Domain.Entities;

namespace HRMS.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);
        Task<User> GetUser(int userid);
        Task<bool> Logout();
    }
}