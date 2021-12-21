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
        private const string password = "111111";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //Add admin user 
            var adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Panel",
                Position = "Administrator",
                UserName = "admin@sts.net",
                Email = "admin@sts.net",
                DepartmentId = 2,
            };

            var adminRegistrationResult = userManager.CreateAsync(adminUser, password).Result;

            if (adminRegistrationResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName);
            }

            //Add employee user 
            var employeeUser = new ApplicationUser
            {
                FirstName = "Employee1",
                LastName = "User1",
                Position = "Customer Care Agent",
                UserName = "cc1@sts.net",
                Email = "cc1@sts.net",
                DepartmentId = 1,
                PhoneNumber = "+49-88-78-78",
            };

            var employeeRegistrationResult = userManager.CreateAsync(employeeUser, password).Result;

            if (employeeRegistrationResult.Succeeded)
            {
                await userManager.AddToRoleAsync(employeeUser, GlobalConstants.EmployeeRoleName);
            }

            //Add manager user 
            var managerUser = new ApplicationUser
            {
                FirstName = "Manager",
                LastName = "ManagerUser",
                Position = "CC Manager",
                Email = "manager@sts.net",
                UserName = "manager@sts.net",
                DepartmentId = 1,
                PhoneNumber = "+49-88-78-78",
            };

            var managerSignInResult = userManager.CreateAsync(managerUser, password).Result;

            if (managerSignInResult.Succeeded)
            {
                await userManager.AddToRoleAsync(managerUser, GlobalConstants.ManagerRoleName);
            }
        }
    }
}
