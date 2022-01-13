using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using STS.Common;
using STS.Data.Models;

namespace STS.Data.Seeding
{
    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                Position = "Administrator",
                UserName = "Admin",
                Email = "admin@sts.net",
                DepartmentId = 1,
            };

            var adminRegistrationResult = userManager.CreateAsync(adminUser, "administrator").Result;

            if (adminRegistrationResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
