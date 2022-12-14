using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Common;
using HRMS.Infrastructure.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace HRMS.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddHealthChecks();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // Added authorization policies that can be used to authorize users based on their role combinations
            // while accessing any route (policy)
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
                options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
            });

            // Added antiforgery token provider
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
                config.LoginPath = "/auth/login";               // redirect user to this path if they are not authenticaed while accessing any url that requries authentication
                config.AccessDeniedPath = "/auth/accessdenied";  // access denied path
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
                options.ValueCountLimit = int.MaxValue; 
                options.ValueLengthLimit = int.MaxValue;
            });

            services.AddControllersWithViews(options =>
            {
                options.MaxModelBindingCollectionSize = int.MaxValue;
                //  for adding authentication check by default to every route unless specified by AllowAnynomous
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();  // tell json serializer the casing of the property
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Add mvc to service collection
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
