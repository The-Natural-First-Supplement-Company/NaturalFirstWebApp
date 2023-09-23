using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using NaturalFirstWebApp.Models;

namespace NaturalFirstWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromDays(30);
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use appropriate setting based on your HTTPS configuration
                options.LoginPath = "/Home/Index"; // Redirect to login page if authentication fails
                options.LogoutPath = "/Home/Logout"; // Redirect to logout page
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Set the desired expiration time
                options.SlidingExpiration = true; // Extend the expiration on each request
            });

            string apiUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

            builder.Services.AddHttpClient("MyApiClient", client =>
            {
                // Configure the base address and other options
                client.BaseAddress = new Uri(apiUrl);
                // Other configuration options
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}