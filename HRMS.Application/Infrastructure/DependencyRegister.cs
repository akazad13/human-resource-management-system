using FluentValidation;
using HRMS.Application.Services.Auth;
using HRMS.Application.Services.Employee;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Application.Infrastructure
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}
