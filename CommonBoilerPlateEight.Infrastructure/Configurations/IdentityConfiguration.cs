using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Infrastructure.Context;
namespace CommonBoilerPlateEight.Infrastructure.Configurations
{ 
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
     .AddEntityFrameworkStores<CommonBoilerPlateEightDbContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.SignIn.RequireConfirmedEmail = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1500);
            });
        }
    }
}