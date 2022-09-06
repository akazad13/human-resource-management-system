using HRMS.Application.Infrastructure;
using HRMS.Infrastructure.Extensions;
using HRMS.Infrastructure.SeedDatabase;
using HRMS.Persistence.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuration for services
builder.Services.AddApplication();
//Configuration for Persitence and Identity provider
builder.Services.AddInfrastructure(builder.Configuration);
// Configuration for Application
builder.Services.AddAppInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseForwardedHeaders();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(Errapp =>
    {
        Errapp.Run(async context =>
        {
            ILogger? _logger = null;
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    _logger = scope.ServiceProvider.GetService<ILogger>();
                }
            }
            catch (Exception ex) { }

            if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    var ex = error.Error;
                    if (_logger != null)
                    {
                        _logger.LogError($"[Global Ajax Erorr]: ", ex);
                    }

                    var ret = new
                    {
                        message = "Internal error occurred!",
                        Code = context.Response.StatusCode
                    };

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(ret), Encoding.UTF8);
                }
            }
            else
            {
                var error = context.Features.Get<IExceptionHandlerFeature>();
                var exType = string.Empty;
                if (error != null)
                {
                    var ex = error?.Error;
                    exType = ex?.GetType().FullName;
                    if (_logger != null)
                    {
                        _logger.LogError($"[Global Error]: ", ex);
                    }
                }

                context.Response.Redirect("Home/Error");

                await Task.CompletedTask;
            }

        });

    });
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapHealthChecks("/healthz");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialise and seed database
using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<ISeed>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
}

app.Run();
