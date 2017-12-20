using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoBackend.Services.Interfaces;
using DemoBackend.Contracts.Requests;

namespace DemoBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET api/Employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        // GET api/Employees/5
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int employeeId)
        {
            var employee = await _employeeService.GetEmployee(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody]UpsertEmployeeRequest employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEmployeeRecord = await _employeeService.CreateEmployee(employeeRequest);

            return CreatedAtAction(nameof(GetEmployee), new { employeeId = newEmployeeRecord.Id }, newEmployeeRecord);
        }

        // PUT api/Employees/5
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int employeeId, [FromBody]UpsertEmployeeRequest employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _employeeService.UpdateEmployee(employeeId, employeeRequest);

            return NoContent();
        }

        // DELETE api/Employees/5
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int employeeId)
        {
            await _employeeService.DeleteEmployee(employeeId);

            return NoContent();
        }
    }
}
