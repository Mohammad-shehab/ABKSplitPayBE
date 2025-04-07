using ABKSplitPayBE.Data;
using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ABKSplitPayBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class AddressDto
        {
            public string FullName { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public bool IsDefault { get; set; }
        }

        // GET: api/Address
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Address>> GetAddresses()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return Ok(addresses);
        }

        // GET: api/Address/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound("Address not found.");
            }

            return Ok(address);
        }

        // POST: api/Address
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Address>> CreateAddress(AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // If this is set as default, unset any existing default
            if (addressDto.IsDefault)
            {
                var existingDefault = await _context.Addresses
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .ToListAsync();
                foreach (var addr in existingDefault)
                {
                    addr.IsDefault = false;
                }
            }

            var address = new Address
            {
                UserId = userId,
                FullName = addressDto.FullName,
                AddressLine1 = addressDto.AddressLine1,
                AddressLine2 = addressDto.AddressLine2,
                City = addressDto.City,
                State = addressDto.State,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country,
                IsDefault = addressDto.IsDefault
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAddress), new { id = address.AddressId }, address);
        }

        // PUT: api/Address/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAddress(int id, AddressDto addressDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound("Address not found.");
            }

            if (addressDto.IsDefault)
            {
                var existingDefault = await _context.Addresses
                    .Where(a => a.UserId == userId && a.IsDefault && a.AddressId != id)
                    .ToListAsync();
                foreach (var addr in existingDefault)
                {
                    addr.IsDefault = false;
                }
            }

            address.FullName = addressDto.FullName ?? address.FullName;
            address.AddressLine1 = addressDto.AddressLine1 ?? address.AddressLine1;
            address.AddressLine2 = addressDto.AddressLine2 ?? address.AddressLine2;
            address.City = addressDto.City ?? address.City;
            address.State = addressDto.State ?? address.State;
            address.PostalCode = addressDto.PostalCode ?? address.PostalCode;
            address.Country = addressDto.Country ?? address.Country;
            address.IsDefault = addressDto.IsDefault;

            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Address/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound("Address not found.");
            }

            // Check if the address is used by any orders
            var ordersUsingAddress = await _context.Orders
                .Where(o => o.ShippingAddressId == id)
                .ToListAsync();

            if (ordersUsingAddress.Any())
            {
                return BadRequest("Cannot delete address because it is associated with existing orders.");
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}