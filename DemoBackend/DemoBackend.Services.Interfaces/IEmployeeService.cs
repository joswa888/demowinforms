using DemoBackend.Contracts.Requests;
using DemoBackend.Contracts.Responses;
using DemoBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoBackend.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<GetEmployeeResponse> CreateEmployee(UpsertEmployeeRequest employeeRequest);
        Task<IEnumerable<GetEmployeeResponse>> GetEmployees();
        Task<GetEmployeeResponse> GetEmployee(int employeeId);
        Task UpdateEmployee(int employeeId, UpsertEmployeeRequest employeeRequest);
        Task DeleteEmployee(int employeeId);
    }
}
