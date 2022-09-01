using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRMS.Application.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> Login(string email, string password)
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

        public async Task<User> Register(string? firstName,
            string? lastName,
            string? email,
            string? phoneNumber,
            string? password,
            IEnumerable<string> assignedRoles
        )
        {
            var response = await IsUserExist(email);
            if (response == true)
            {
                throw new Exception($"There is already an user registered with this email!." );
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

        public async Task<User> GetUser(int userid)
        {
            return await _userManager.FindByIdAsync(userid.ToString());
        }

        public async Task<bool> IsUserExist(string? email)
        {
            if (email == null)
                return false;
            return await _userManager.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
