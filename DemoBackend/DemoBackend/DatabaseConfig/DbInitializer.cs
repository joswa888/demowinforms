using DemoBackend.Database.Contexts;
using DemoBackend.Models;
using System;
using System.Linq;

namespace DemoBackend.DatabaseConfig
{
    public static class DbInitializer
    {
        public static void Initialize(DemoBackendContext context)
        {
            context.Database.EnsureCreated(); //if DB doesn't exist, it will auto create the DB.

            if (context.Employees.Any())
                return;

            var employee = new Employee
            {
                FirstName = "Joshua",
                LastName = "Colanggo",
                DateCreated = DateTime.UtcNow
            };

            context.Add(employee);
            context.SaveChanges();
        }
    }
}
