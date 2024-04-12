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
    public class ServicesController : ControllerBase
    {
        private readonly AppointmentSystemContext _context;

        public ServicesController(AppointmentSystemContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // POST: api/Services
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.Id }, service);
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]

        public async Task<IActionResult> PutService(int id, Service serviceUpdate)
        {
            if (id != serviceUpdate.Id)
            {
                return BadRequest("Service ID mismatch.");
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound($"Service with ID {id} not found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            service.Name = serviceUpdate.Name;
            service.Price = serviceUpdate.Price;

            try
            {


                // Save changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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
                return StatusCode(500, "An error occurred while updating the customer. Please try again later.");
            }

            return NoContent();
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
