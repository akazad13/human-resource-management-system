using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure.SeedDatabase;

public static class Seed
{
    public static async Task SeedDefaultUserAsync(this IServiceProvider services, IServiceScope scope)
    {

        var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        if (!await userManager.Users.AnyAsync())
        {
            // create some roles

            var roles = new List<Role>
            {
                new Role{Name = "Admin"},
                new Role{Name = "User"},
                new Role{Name= "Manager"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new User
            {
                UserName = "akazad.cse13",
                Email = "akazad.cse13@gmail.com",
                FirstName = "Abul",
                LastName = "Kalam"
            };

            var result = await userManager.CreateAsync(admin, "Password123");
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(admin, new[] { "Admin" });
            }
        }
    }
}
