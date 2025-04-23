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
        public class StoreCategoryDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string PictureUrl { get; set; }
            public bool IsActive { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreCategory>>> GetStoreCategories()
        {
            var categories = await _context.StoreCategories
                .Where(sc => sc.IsActive)
                .ToListAsync();
            return Ok(categories);
        }
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
        [HttpPost]
        [Authorize(Roles = "Admin")] 
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
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStoreCategory(int id)
        {
            var category = await _context.StoreCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Store category not found.");
            }
            category.IsActive = false; 
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}