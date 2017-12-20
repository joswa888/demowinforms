using DemoBackend.Services.Interfaces;
using System.Collections.Generic;
using DemoBackend.Models;
using System.Threading.Tasks;
using DemoBackend.Database.Interface;
using DemoBackend.Contracts.Requests;
using AutoMapper;
using DemoBackend.Contracts.Responses;
using System;

namespace DemoBackend.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeResponse> CreateEmployee(UpsertEmployeeRequest employeeRequest)
        {
            var mapRequest = _mapper.Map<Employee>(employeeRequest);
                mapRequest.DateCreated = DateTime.UtcNow;

            var result = await _employeeRepository.CreateEmployee(mapRequest);

            return _mapper.Map<GetEmployeeResponse>(result);
        }

        public async Task<GetEmployeeResponse> GetEmployee(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeAsNoTracking(employeeId);

            return _mapper.Map<GetEmployeeResponse>(employee);
        }

        public async Task<IEnumerable<GetEmployeeResponse>> GetEmployees()
        {
            var results = await _employeeRepository.GetEmployeesAsNoTracking();

            return _mapper.Map<ICollection<GetEmployeeResponse>>(results);
        }

        public async Task UpdateEmployee(int employeeId, UpsertEmployeeRequest employeeRequest)
        {
            var existingEmployeeData = await _employeeRepository.GetEmployee(employeeId);

            var newEmployeeData = _mapper.Map<Employee>(employeeRequest);

            existingEmployeeData = _mapper.Map(newEmployeeData, existingEmployeeData);

            await _employeeRepository.UpdateEmployee(existingEmployeeData);
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var employeeToDelete = await _employeeRepository.GetEmployee(employeeId);

            await _employeeRepository.DeleteEmployee(employeeToDelete);
        }
    }
}
