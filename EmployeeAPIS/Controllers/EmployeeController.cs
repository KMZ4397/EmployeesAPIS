using EmployeeAPIS.Services;
using EmployeesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            return Ok(await employeeService.GetAllEmployeesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(CreateEmployeeReq request)
        {
            var employee = await employeeService.AddEmployeeAsync(request);
            if (employee == null)
            {
                return StatusCode(500,"No company has this id@!");
            }

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return BadRequest("Ther's no employee with the id entered.");
            }
            try
            {
                var employeeMod = new EmployeeMod
                {
                    Id = employeeDto.Id,
                    Name = employeeDto.Name,
                    Email = employeeDto.Email,
                    Mobile = employeeDto.Mobile,
                    CompanyId = employeeDto.CompanyId
                };
                await employeeService.UpdateEmployeeAsync(id, employeeMod); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await employeeService.GetEmployeeByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
