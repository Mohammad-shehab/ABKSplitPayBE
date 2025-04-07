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
    public class StoreCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StoreCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTO for creating/updating store categories
        public class StoreCategoryDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string PictureUrl { get; set; }
            public bool IsActive { get; set; }
        }

        // GET: api/StoreCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreCategory>>> GetStoreCategories()
        {
            var categories = await _context.StoreCategories
                .Where(sc => sc.IsActive)
                .ToListAsync();
            return Ok(categories);
        }

        // GET: api/StoreCategory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreCategory>> GetStoreCategory(int id)
        {
            var category = await _context.StoreCategories
                .FirstOrDefaultAsync(sc => sc.StoreCategoryId == id && sc.IsActive);

            if (category == null)
            {
                return NotFound("Store category not found.");
            }

            return Ok(category);
        }

        // POST: api/StoreCategory
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only admins can create categories
        public async Task<ActionResult<StoreCategory>> CreateStoreCategory(StoreCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new StoreCategory
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                PictureUrl = categoryDto.PictureUrl,
                IsActive = categoryDto.IsActive
            };

            _context.StoreCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStoreCategory), new { id = category.StoreCategoryId }, category);
        }

        // PUT: api/StoreCategory/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can update categories
        public async Task<IActionResult> UpdateStoreCategory(int id, StoreCategoryDto categoryDto)
        {
            var category = await _context.StoreCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Store category not found.");
            }

            category.Name = categoryDto.Name ?? category.Name;
            category.Description = categoryDto.Description ?? category.Description;
            category.PictureUrl = categoryDto.PictureUrl ?? category.PictureUrl;
            category.IsActive = categoryDto.IsActive;

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/StoreCategory/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete categories
        public async Task<IActionResult> DeleteStoreCategory(int id)
        {
            var category = await _context.StoreCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Store category not found.");
            }

            category.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}