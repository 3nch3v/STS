using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace STS.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StsDbContext>
    {
        public StsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            var builder = new DbContextOptionsBuilder<StsDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new StsDbContext(builder.Options);
        }
    }
}
