namespace CharityAction.Web.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope
                    .ServiceProvider
                    .GetService<CharityDbContext>()
                    .Database
                    .Migrate();

                var userManager = serviceScope
                    .ServiceProvider
                    .GetService<UserManager<ApplicationUser>>();

                var roleManager = serviceScope
                    .ServiceProvider
                    .GetService<RoleManager<IdentityRole>>();

                Task.Run(async () =>
                {
                    // add custom roles
                    var roles = new[]
                    {
                        WebConstants.AdministratorRole
                    };

                    foreach (var role in roles)
                    {
                        var existingRole = await roleManager.RoleExistsAsync(role);

                        if (!existingRole)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role
                            });
                        }
                    }

                    // add dummy user
                    var testUsername = "test";
                    var testEmail = "test@mail.com";
                    var testUser = await userManager.FindByEmailAsync(testEmail);

                    if (testUser == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = testUsername,
                            Email = testEmail
                        };

                        await userManager.CreateAsync(user, "test12");
                    }

                    // add administrator
                    var adminUsername = "admin";
                    var adminEmail = "admin@mymail.com";
                    var adminFromDb = await userManager.FindByEmailAsync(adminEmail);

                    if (adminFromDb == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = adminUsername,
                            Email = adminEmail
                        };

                        await userManager.CreateAsync(user, "admin12");
                        await userManager.AddToRoleAsync(user, WebConstants.AdministratorRole);
                    }
                })
                .Wait();
            }

            return app;
        }
    }
}