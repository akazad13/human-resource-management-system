using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Common;
using HRMS.Infrastructure.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace HRMS.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHealthChecks();

            //make HttpContext available across the app
            //services.AddHttpContextAccessor();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
                options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
            });

            //services.AddSession(options =>
            //{
            //    options.Cookie.Name = ".HRMS_Session_";
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    options.Cookie.SameSite = SameSiteMode.None;
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;

            //    options.IdleTimeout = TimeSpan.FromMinutes(configuration.GetValue<double>("SessionTimeout"));
            //});


            services.AddAntiforgery(o =>
            {
                o.SuppressXFrameOptionsHeader = true;
                o.Cookie.SameSite = SameSiteMode.None;
                o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                o.HeaderName = "RequestVerificationToken";
            });

            services.ConfigureApplicationCookie(config =>
            {
                config.SlidingExpiration = true;
                config.LoginPath = "/auth/login";
                config.AccessDeniedPath = "/auth/accessdenied";
                config.Cookie.Name = ".HRMS_Cookie_";
                config.Cookie.SameSite = SameSiteMode.Lax;
                config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                config.Cookie.IsEssential = true;
                config.Cookie.HttpOnly = true;
                config.ExpireTimeSpan = TimeSpan.FromMinutes(configuration.GetValue<double>("SessionTimeout"));
            });

            //Form limit
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue; // 50000 items max
                options.ValueLengthLimit = int.MaxValue; // 1000MB max len form data
            });

            services.AddControllersWithViews(options =>
            {
                options.MaxModelBindingCollectionSize = int.MaxValue;
                //  for adding authentication check
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddMvc(options =>
            {
                options.MaxModelBindingCollectionSize = int.MaxValue;

            });

            services.AddSingleton(context => AddAutoMapper(new AppDomainTypeFinder()));
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
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
