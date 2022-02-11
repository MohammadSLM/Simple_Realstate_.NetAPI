using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Model;
using MyApi1.Model;
using MyApi1.Model.DTO;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customer.OrderByDescending(a => a.Id).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<Customer>>> GetCustomersBy9()
        {
            var list = await _context.Customer.OrderByDescending(a => a.Id)
                .Take(9)
                .ToListAsync();
            return list;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] CustomerDto dto , int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Customer.FindAsync(id);
            model.Title = dto.Title;
            model.Desc = dto.Desc;
            model.VidUrl = dto.VidUrl;

            _context.Customer.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> Add([FromForm] CustomerDto dto,CancellationToken cancellationToken)
        {
            Customer customer = new Customer()
            {
                Title = dto.Title,
                Desc = dto.Desc,
                VidUrl = dto.VidUrl
            };
            await _context.Customer.AddAsync(customer,cancellationToken);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
