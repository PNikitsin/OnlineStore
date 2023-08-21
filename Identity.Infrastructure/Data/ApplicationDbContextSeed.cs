using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString())); 

                #region developer

                var developer = new ApplicationUser
                {
                    UserName = "pnikitsin",
                    FirstName = "Pavel",
                    LastName = "Nikitsin",
                    Email = "pnikitsin@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                if (!context!.Users.Any(u => u.UserName == developer.UserName))
                {
                    await userManager.CreateAsync(developer, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(developer, Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(developer, Roles.User.ToString());
                }

                #endregion
            }
        }
    }
}