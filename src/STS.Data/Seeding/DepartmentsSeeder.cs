using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using STS.Data.Models;

namespace STS.Data.Seeding
{
    internal class DepartmentsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Departments.Any())
            {
                return;
            }

            var departments = new List<Department>
            {
                new Department { Name = "Customer Care" },
                new Department { Name = "IT Support" },
            };

            await dbContext.Departments.AddRangeAsync(departments);
        }
    }
}
