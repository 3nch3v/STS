using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using STS.Data.Models;

namespace STS.Data.Seeding
{
    internal class PrioritiesSeeder : ISeeder
    {
        public async Task SeedAsync(StsDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Priorities.Any())
            {
                return;
            }

            var priorities = new List<Priority>
            {
                new Priority { Name = "Urgent" },
                new Priority { Name = "High" },
                new Priority { Name = "Medium" },
                new Priority { Name = "Low" },
            };

            await dbContext.Priorities.AddRangeAsync(priorities);
        }
    }
}
