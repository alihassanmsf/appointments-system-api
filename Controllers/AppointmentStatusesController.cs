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
    public class AppointmentStatusesController : ControllerBase
    {
        private readonly AppointmentSystemContext _context;
        public AppointmentStatusesController(AppointmentSystemContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentStatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentStatus>>> GetAppointmentStatuses()
        {
            return await _context.AppointmentStatuses.ToListAsync();
        }

        // GET: api/AppointmentStatuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentStatus>> GetAppointmentStatus(int id)
        {
            var appointmentStatus = await _context.AppointmentStatuses.FindAsync(id);

            if (appointmentStatus == null)
            {
                return NotFound();
            }

            return appointmentStatus;
        }

        // POST: api/AppointmentStatuses
        [HttpPost]
        public async Task<ActionResult<AppointmentStatus>> PostAppointmentStatus(AppointmentStatus appointmentStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AppointmentStatuses.Add(appointmentStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointmentStatus", new { id = appointmentStatus.Id }, appointmentStatus);
        }

        // PUT: api/AppointmentStatuses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentStatus(int id, AppointmentStatus appointmentStatusUpdate)
        {
            if (id != appointmentStatusUpdate.Id)
            {
                return BadRequest("Appointment Status ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(appointmentStatusUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/AppointmentStatuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentStatus(int id)
        {
            var appointmentStatus = await _context.AppointmentStatuses.FindAsync(id);
            if (appointmentStatus == null)
            {
                return NotFound();
            }

            _context.AppointmentStatuses.Remove(appointmentStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentStatusExists(int id)
        {
            return _context.AppointmentStatuses.Any(e => e.Id == id);
        }
    }
}
