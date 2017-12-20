using DemoBackend.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DemoBackend.DatabaseConfig
{
    public class DemoBackend_DbContextFactory : IDesignTimeDbContextFactory<DemoBackendContext>
    {
        public DemoBackendContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DemoBackendContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.UseSqlServer(configuration.GetConnectionString("Defa‌​ultConnection"));

            return new DemoBackendContext(builder.Options);
        }
    }
}
