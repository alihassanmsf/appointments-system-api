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
    public class CustomersController : ControllerBase
    {
        private readonly AppointmentSystemContext _context;

        public CustomersController(AppointmentSystemContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
 
        public async Task<IActionResult> PutCustomer(int id, Customer customerUpdate)
        {
            if (id != customerUpdate.Id)
            {
                return BadRequest("Customer ID mismatch.");
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            customer.Name = customerUpdate.Name;
            customer.ContactNumber = customerUpdate.ContactNumber; 

            try
            {


                // Save changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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


        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
