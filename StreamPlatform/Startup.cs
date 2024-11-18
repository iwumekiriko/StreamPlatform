using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StreamPlatform.Data;
using StreamPlatform.Hubs;
using StreamPlatform.Models;

namespace StreamPlatform;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddControllersWithViews();
        services.AddSignalR();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Streams}/{action=Index}");
            endpoints.MapControllerRoute(
                name: "cabinet",
                pattern: "{controller=Account}/{action=Cabinet}",
                defaults: new { controller = "Account", action = "Cabinet" });
            endpoints.MapControllerRoute(
                name: "live",
                pattern: "live/{username}",
                defaults: new { controller = "Streams", action = "Details" });
            endpoints.MapHub<StreamHub>("/stream");
        });
    }
}