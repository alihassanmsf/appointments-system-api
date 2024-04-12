using AppointmentSystemApi.Data;
using AppointmentSystemApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowLocalhost4200")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppointmentSystemContext _context;

        public EmployeesController(AppointmentSystemContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employeeUpdate)
        {
            if (id != employeeUpdate.Id)
            {
                return BadRequest("Employee ID mismatch.");
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Update fields individually, excluding read-only properties
            employee.Name = employeeUpdate.Name;
            employee.Specialization = employeeUpdate.Specialization;
            // Continue for other fields...

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    // Consider a more user-friendly message or a strategy to resolve concurrency conflicts
                    return StatusCode(409, "A concurrency error occurred. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, "An error occurred while updating the employee. Please try again later.");
            }

            return NoContent();
        }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
