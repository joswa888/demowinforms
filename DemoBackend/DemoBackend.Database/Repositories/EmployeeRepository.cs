using DemoBackend.Database.Interface;
using System.Collections.Generic;
using DemoBackend.Models;
using System.Threading.Tasks;
using DemoBackend.Database.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DemoBackendContext _context;

        public EmployeeRepository(DemoBackendContext context)
        {
            _context = context;
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            _context.Add(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            return await _context.Employees
                                 .SingleOrDefaultAsync(e => e.Id.Equals(employeeId));
        }

        public async Task<Employee> GetEmployeeAsNoTracking(int employeeId)
        {
            return await _context.Employees
                                 .AsNoTracking()
                                 .SingleOrDefaultAsync(e => e.Id.Equals(employeeId));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsNoTracking()
        {
            return await _context.Employees
                                 .AsNoTracking()
                                 .OrderBy(s => s.Id)
                                 .ToArrayAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeExists(employee.Id))
                {
                    //log to file or table storage
                }
            }
        }

        public async Task DeleteEmployee(Employee employee)
        {
            try
            {
                _context.Remove(employee);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeExists(employee.Id))
                {
                    //log to file or table storage
                }
            }
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id.Equals(id));
        }
    }
}
