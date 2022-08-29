using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Utilities;
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
            services.Configure<ConfigModel>(configuration);

            services.AddHealthChecks();

            //make HttpContext available across the app
            services.AddHttpContextAccessor();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(o => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
                options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
            });


            //To generate the cache table follow: https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0#distributed-sql-server-cache
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = configuration.GetValue<string>("DistCache:ConnectionString");
            //    options.SchemaName = configuration.GetValue<string>("DistCache:SchemaName");
            //    options.TableName = configuration.GetValue<string>("DistCache:TableName");
            //    options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(configuration.GetValue<double>("DistCache:ExpiredItemsDeletionInterval"));
            //});

            services.AddSession(options =>
            {
                options.Cookie.Name = ".HRMS_Session_";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

                options.IdleTimeout = TimeSpan.FromMinutes(configuration.GetValue<double>("SessionTimeout"));
            });


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


            services.AddSingleton<ICurrentUserService, CurrentUserService>();
        }
    }
}
