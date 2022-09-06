using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    throw new Exception("Email or Password does not match!");
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    return true;
                }

                throw new Exception("Email or Password does not match!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public async Task<User> Register(string? firstName,
            string? lastName,
            string? email,
            string? phoneNumber,
            string? password,
            IEnumerable<string> assignedRoles
        )
        {
            try
            {
                var response = await IsUserExist(email);
                if (response == true)
                {
                    throw new Exception($"There is already an user registered with this email!.");
                }

                var user = new User
                {
                    UserName = email?.Split('@')[0],
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, assignedRoles);
                }

                if (result.Succeeded)
                {
                    return user;
                }

                throw new Exception($"Unable to create the user!.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"Unable to create the user!.");
            }
        }

        public async Task<bool> IsUserExist(string? email)
        {
            try
            {
                if (email == null)
                    return false;
                return await _userManager.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }
        public async Task<bool> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}
