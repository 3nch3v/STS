using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using STS.Data.Models;

namespace STS.Data.Seeding
{
    internal class StatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Statuses.Any())
            {
                return;
            }

            var priorities = new List<Status>
            {
                new Status { Name = "Open" },
                new Status { Name = "Pending" },
                new Status { Name = "Solved" },
                new Status { Name = "Closed" },
                new Status { Name = "In progress" },
                new Status { Name = "On hold" },
                new Status { Name = "New reply" },
            };

            await dbContext.Statuses.AddRangeAsync(priorities);
        }
    }
}
