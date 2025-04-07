using ABKSplitPayBE.Data;
using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABKSplitPayBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTO for creating/updating stores
        public class StoreDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string WebsiteUrl { get; set; }
            public int StoreCategoryId { get; set; }
            public string LogoUrl { get; set; }
            public bool IsActive { get; set; }
        }

        // GET: api/Store
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            var stores = await _context.Stores
                .Include(s => s.StoreCategory)
                .Where(s => s.IsActive)
                .ToListAsync();
            return Ok(stores);
        }

        // GET: api/Store/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _context.Stores
                .Include(s => s.StoreCategory)
                .FirstOrDefaultAsync(s => s.StoreId == id && s.IsActive);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            return Ok(store);
        }

        // POST: api/Store
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only admins can create stores
        public async Task<ActionResult<Store>> CreateStore(StoreDto storeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var store = new Store
            {
                Name = storeDto.Name,
                Description = storeDto.Description,
                WebsiteUrl = storeDto.WebsiteUrl,
                StoreCategoryId = storeDto.StoreCategoryId,
                LogoUrl = storeDto.LogoUrl,
                IsActive = storeDto.IsActive
            };

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStore), new { id = store.StoreId }, store);
        }

        // PUT: api/Store/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can update stores
        public async Task<IActionResult> UpdateStore(int id, StoreDto storeDto)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound("Store not found.");
            }

            store.Name = storeDto.Name ?? store.Name;
            store.Description = storeDto.Description ?? store.Description;
            store.WebsiteUrl = storeDto.WebsiteUrl ?? store.WebsiteUrl;
            store.StoreCategoryId = storeDto.StoreCategoryId != 0 ? storeDto.StoreCategoryId : store.StoreCategoryId;
            store.LogoUrl = storeDto.LogoUrl ?? store.LogoUrl;
            store.IsActive = storeDto.IsActive;

            _context.Entry(store).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Store/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete stores
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound("Store not found.");
            }

            store.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}