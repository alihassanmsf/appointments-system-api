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
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentSystemContext _context;

        public AppointmentsController(AppointmentSystemContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Include(a => a.AppointmentStatus)
                .ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Include(a => a.AppointmentStatus)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST: api/Appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentCreationDto appointmentDto)
        {
            var appointment = new Appointment
            {
                AppointmentDate = appointmentDto.AppointmentDate,
                EmployeeId = appointmentDto.EmployeeId,
                CustomerId = appointmentDto.CustomerId,
                ServiceId = appointmentDto.ServiceId,
                AppointmentStatusId = appointmentDto.AppointmentStatusId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointmentUpdate)
        {
            if (id != appointmentUpdate.Id)
            {
                return BadRequest("Appointment ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(appointmentUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
