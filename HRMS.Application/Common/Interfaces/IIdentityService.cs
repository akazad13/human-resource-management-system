using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;

namespace HRMS.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<User> AuthenticateUser(string email, string password);
        Task<bool> IsUserExist(string email);
        Task<string> GetUserNameAsync(long userId);
        Task<IList<string>> GetUserRoles(User user);
        Task<bool> IsInRoleAsync(long userId, string role);
        Task<bool> AuthorizeAsync(long userId, string policyName);
        Task<Result> CreateUserAsync(string firstname, string Lastname, string email, string password, long? managerId);
        Task<Result> DeleteUserAsync(long userId);
    }
}
