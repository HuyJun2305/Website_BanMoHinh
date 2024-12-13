using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Data.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAddressesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderAddress>>> GetOrderAddresses()
        {
          if (_context.OrderAddresses == null)
          {
              return NotFound();
          }
            return await _context.OrderAddresses.ToListAsync();
        }

        // GET: api/OrderAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderAddress>> GetOrderAddress(Guid id)
        {
          if (_context.OrderAddresses == null)
          {
              return NotFound();
          }
            var orderAddress = await _context.OrderAddresses.FindAsync(id);

            if (orderAddress == null)
            {
                return NotFound();
            }

            return orderAddress;
        }

        // PUT: api/OrderAddresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderAddress(Guid id, OrderAddress orderAddress)
        {
            if (id != orderAddress.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderAddressExists(id))
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

        // POST: api/OrderAddresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderAddress>> PostOrderAddress(OrderAddress orderAddress)
        {
          if (_context.OrderAddresses == null)
          {
              return Problem("Entity set 'ApplicationDbContext.OrderAddresses'  is null.");
          }
            _context.OrderAddresses.Add(orderAddress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderAddress", new { id = orderAddress.Id }, orderAddress);
        }

        // DELETE: api/OrderAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAddress(Guid id)
        {
            if (_context.OrderAddresses == null)
            {
                return NotFound();
            }
            var orderAddress = await _context.OrderAddresses.FindAsync(id);
            if (orderAddress == null)
            {
                return NotFound();
            }

            _context.OrderAddresses.Remove(orderAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderAddressExists(Guid id)
        {
            return (_context.OrderAddresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
