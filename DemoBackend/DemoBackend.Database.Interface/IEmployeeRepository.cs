using DemoBackend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoBackend.Database.Interface
{
    public interface IEmployeeRepository
    {
        Task<Employee> CreateEmployee(Employee employee);
        Task <IEnumerable<Employee>> GetEmployeesAsNoTracking();
        Task<Employee> GetEmployeeAsNoTracking(int employeeId);
        Task<Employee> GetEmployee(int employeeId);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(Employee employee);
    }
}
