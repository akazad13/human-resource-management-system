using HRMS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;

namespace HRMS.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public IdentityService(
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            var result = await _userManager.CheckPasswordAsync(user, password);

            if (!result)
            {
                return null;
            }

            return user;
        }
        public async Task<bool> IsUserExist(string email)
        {
            if (email == null)
                return false;
            return await _userManager.Users.AnyAsync(u => u.Email == email);
        }

        public Task<IList<string>> GetUserRoles(User user)
        {
            return _userManager.GetRolesAsync(user);
        }
        public async Task<string> GetUserNameAsync(long userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user.UserName;
        }
        public async Task<Result> CreateUserAsync(string firstname, string Lastname, string email, string password, long? ManagerId)
        {
            var user = new User
            {
                FirstName = firstname,
                LastName = Lastname,
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            return result.ToApplicationResult();
        }

        public async Task<bool> IsInRoleAsync(long userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(long userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<Result> DeleteUserAsync(long userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }

        public async Task<Result> DeleteUserAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}