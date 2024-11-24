using Microsoft.AspNetCore.Identity;
using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using CommonBoilerPlateEight.Infrastructure.Context;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.DefaultDataSeeder
{
    public class DataSeeder
    {
        public static async Task SeedDefaultData(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CommonBoilerPlateEightDbContext>();
                if (context == null)
                {
                    throw new Exception("Please run migration");
                }

                var RoleAdmin = "Admin";
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                using (var tx = await context.Database.BeginTransactionAsync())
                {
                    if (!await roleManager.RoleExistsAsync(RoleAdmin))
                        await roleManager.CreateAsync(new IdentityRole(RoleAdmin));
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    string adminUserEmail = "admin@gmail.com";

                    var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                    if (adminUser == null)
                    {
                        var newAdminUser = new ApplicationUser
                        {
                            UserName ="admin",
                            FullName ="Admin Admin",
                            Email= adminUserEmail,
                            PhoneNumber ="9750214567",
                        };
                        await userManager.CreateAsync(newAdminUser, "Admin@123").ConfigureAwait(false);
                        await userManager.AddToRoleAsync(newAdminUser, RoleAdmin);
                        
                    }
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    await tx.CommitAsync();
                }
            }
        }
    }
}
