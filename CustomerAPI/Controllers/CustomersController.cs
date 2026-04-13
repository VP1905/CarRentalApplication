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
        public IActionResult GetCustomer()
        {
            var customers = new[]
            {
        new
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@example.com",
            PhoneNumber = "123-456-7890"
        },
        new
        {
            Id = 2,
            FirstName = "Emma",
            LastName = "Brown",
            Email = "emma.brown@example.com",
            PhoneNumber = "987-654-3210"
        },
        new
        {
            Id = 3,
            FirstName = "Liam",
            LastName = "Wilson",
            Email = "liam.wilson@example.com",
            PhoneNumber = "555-222-1111"
        }
    };

            return Ok(customers);
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