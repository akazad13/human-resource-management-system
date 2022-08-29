using AutoMapper;
using FluentValidation;
using HRMS.Application.Common.Mapper;
using HRMS.Application.Common.Utilities;
using HRMS.Application.Services.Auth;
using HRMS.Domain.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Application.Infrastructure
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton(context => AddAutoMapper(new AppDomainTypeFinder()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Add helper and other services
            services.AddScoped<IHelper, Helper>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        private static IMapper AddAutoMapper(ITypeFinder typeFinder)
        {
            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                            .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                            .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //register
            AutoMapperConfiguration.Init(config);

            return AutoMapperConfiguration.Mapper;
        }
    }
}
