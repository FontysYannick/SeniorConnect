using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SeniorConnect.API.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            // Specificeer het pad expliciet
            var path = Path.Combine(Directory.GetCurrentDirectory(), "src", "SeniorConnect.API");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.SeniorConnect.API.json")
                .AddUserSecrets<Program>()
                .Build();

            var connectionString = configuration.GetConnectionString("SeniorConnectSqlDb");

            optionsBuilder.UseSqlServer(connectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
