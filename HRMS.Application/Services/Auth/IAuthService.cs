using HRMS.Domain.Entities;

namespace HRMS.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);
        Task<User> Register(string? firstName,
            string? lastName,
            string? email,
            string? phoneNumber,
            string? password,
            IEnumerable<string> assignedRoles
        );
        Task<User> GetUser(int userid);
        Task<bool> IsUserExist(string? email);
        Task<bool> Logout();
    }
}