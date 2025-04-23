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
        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _context.Stores
                .Include(s => s.StoreCategory)
                .Where(s => s.IsActive)
                .Select(s => new
                {
                    s.StoreId,
                    s.Name,
                    s.Description,
                    s.WebsiteUrl,
                    s.StoreCategoryId,
                    s.LogoUrl,
                    s.IsActive,
                    StoreCategory = new
                    {
                        s.StoreCategory.StoreCategoryId,
                        s.StoreCategory.Name,                      
                    }
                })
                .ToListAsync();
            return Ok(stores);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(int id)
        {
            var store = await _context.Stores
                .Include(s => s.StoreCategory)
                .Where(s => s.IsActive && s.StoreId == id)
                .Select(s => new
                {
                    s.StoreId,
                    s.Name,
                    s.Description,
                    s.WebsiteUrl,
                    s.StoreCategoryId,
                    s.LogoUrl,
                    s.IsActive,
                    StoreCategory = new
                    {
                        s.StoreCategory.StoreCategoryId,
                        s.StoreCategory.Name,
                        s.StoreCategory.Description,
                        s.StoreCategory.PictureUrl,
                        s.StoreCategory.IsActive
                    }
                })
                .FirstOrDefaultAsync();
            if (store == null)
            {
                return NotFound("Store not found.");
            }
            return Ok(store);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> CreateStore([FromBody] dynamic storeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string name = storeDto.Name;
            int storeCategoryId = storeDto.StoreCategoryId;
            bool isActive = storeDto.IsActive;

            if (string.IsNullOrEmpty(name) || storeCategoryId == 0)
            {
                return BadRequest("Name and StoreCategoryId are required.");
            }
            var storeCategory = await _context.StoreCategories.FindAsync(storeCategoryId);
            if (storeCategory == null)
            {
                return BadRequest("Invalid StoreCategoryId.");
            }
            var store = new Store
            {
                Name = name,
                Description = storeDto.Description,
                WebsiteUrl = storeDto.WebsiteUrl,
                StoreCategoryId = storeCategoryId,
                LogoUrl = storeDto.LogoUrl,
                IsActive = isActive
            };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
            var createdStore = await _context.Stores
                .Include(s => s.StoreCategory)
                .Where(s => s.StoreId == store.StoreId)
                .Select(s => new
                {
                    s.StoreId,
                    s.Name,
                    s.Description,
                    s.WebsiteUrl,
                    s.StoreCategoryId,
                    s.LogoUrl,
                    s.IsActive,
                    StoreCategory = new
                    {
                        s.StoreCategory.StoreCategoryId,
                        s.StoreCategory.Name,
                        s.StoreCategory.Description,
                        s.StoreCategory.PictureUrl,
                        s.StoreCategory.IsActive
                    }
                })
                .FirstOrDefaultAsync();
            return CreatedAtAction(nameof(GetStore), new { id = store.StoreId }, createdStore);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateStore(int id, [FromBody] dynamic storeDto)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound("Store not found.");
            }
            int storeCategoryId = storeDto.StoreCategoryId;
            if (storeCategoryId != 0)
            {
                var storeCategory = await _context.StoreCategories.FindAsync(storeCategoryId);
                if (storeCategory == null)
                {
                    return BadRequest("Invalid StoreCategoryId.");
                }
                store.StoreCategoryId = storeCategoryId;
            }
            store.Name = storeDto.Name ?? store.Name;
            store.Description = storeDto.Description ?? store.Description;
            store.WebsiteUrl = storeDto.WebsiteUrl ?? store.WebsiteUrl;
            store.LogoUrl = storeDto.LogoUrl ?? store.LogoUrl;
            store.IsActive = storeDto.IsActive != null ? (bool)storeDto.IsActive : store.IsActive;

            _context.Entry(store).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound("Store not found.");
            }
            store.IsActive = false; 
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}