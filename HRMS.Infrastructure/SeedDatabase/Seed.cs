using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.SeedDatabase;

public class Seed : ISeed
{
    private readonly ILogger<Seed> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IEmployeeRepository _employeeRepository;
    public Seed(
        ILogger<Seed> logger,
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IEmployeeRepository employeeRepository
    )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _employeeRepository = employeeRepository;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            var roles = new List<Role>
                {
                    new Role{Name = "Admin"},
                    new Role{Name = "User"},
                    new Role{Name= "Manager"}
                };

            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

            var admin = new User
            {
                UserName = "admin",
                Email = "admin@hrms.com",
                FirstName = "Abul",
                LastName = "Kalam"
            };

            var result = await _userManager.CreateAsync(admin, "Password123");
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(admin, new[] { "Admin" });

                var emp = new Employee()
                {
                    UserId = admin.Id,
                    User = admin
                };

                await _employeeRepository.Create(emp);

                var ret = await _employeeRepository.Commit();
                if (!ret)
                {
                    _logger.LogCritical($"Failed to create employee for {admin.Email}", emp);
                }

            }
        }
    }
}
