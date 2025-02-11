using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShoppingCartSW.Models.Data;
using ShoppingCartSW.Models.Repositories;

namespace ShoppingCartSW
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Get the connections string details from the appsettings file.
            var connectionString = builder.Configuration.GetConnectionString("Default");
            // Add the context class to the dependency injection and set it up for SQL server
            // and pass it the connection string.
            builder.Services.AddDbContext<ShoppingCartDBContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            //Add our classes to the dependency injection so we can request them in our other classes.
            //By adding them using the interface name as the key, we can change the class associated with it 
            //when needed without updating our other classes.
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                           .AddCookie(options =>
                           {
                               //Sets how logn the cookie lasts before deleted.
                               options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                               //Allows the timespan to reset when still in use. Only happens if the timer has lapsed over half its time.
                               options.SlidingExpiration = true;
                               //Sets the default redirection locations for failed access attempts
                               options.LoginPath = "/Authentication/Login";
                               options.AccessDeniedPath = "/Authentication/AccessDenied";
                           });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
