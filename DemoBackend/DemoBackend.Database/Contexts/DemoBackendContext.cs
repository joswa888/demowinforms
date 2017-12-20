using DemoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Contexts
{
    public class DemoBackendContext : DbContext
    {
        public DemoBackendContext(DbContextOptions<DemoBackendContext> options)
            :base(options)
        {
                
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
