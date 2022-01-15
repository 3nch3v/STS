using System;
using System.Threading.Tasks;

namespace STS.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(StsDbContext dbContext, IServiceProvider serviceProvider);
    }
}
