using CustomerAPI.Data;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomer()
        {
            return await _context.Customer.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customers customer)
        {
            if (id != customer.CustomerId)
                return BadRequest();

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomer(Customers customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
                return NotFound();

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}