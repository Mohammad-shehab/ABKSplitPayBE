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
    public class ProductCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTO for creating/updating product categories
        public class ProductCategoryDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string PictureUrl { get; set; }
            public bool IsActive { get; set; }
        }

        // GET: api/ProductCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
            var categories = await _context.ProductCategories
                .Where(pc => pc.IsActive)
                .ToListAsync();
            return Ok(categories);
        }

        // GET: api/ProductCategory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
            var category = await _context.ProductCategories
                .FirstOrDefaultAsync(pc => pc.ProductCategoryId == id && pc.IsActive);

            if (category == null)
            {
                return NotFound("Product category not found.");
            }

            return Ok(category);
        }

        // POST: api/ProductCategory
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only admins can create categories
        public async Task<ActionResult<ProductCategory>> CreateProductCategory(ProductCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new ProductCategory
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                PictureUrl = categoryDto.PictureUrl,
                IsActive = categoryDto.IsActive
            };

            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductCategory), new { id = category.ProductCategoryId }, category);
        }

        // PUT: api/ProductCategory/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can update categories
        public async Task<IActionResult> UpdateProductCategory(int id, ProductCategoryDto categoryDto)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Product category not found.");
            }

            category.Name = categoryDto.Name ?? category.Name;
            category.Description = categoryDto.Description ?? category.Description;
            category.PictureUrl = categoryDto.PictureUrl ?? category.PictureUrl;
            category.IsActive = categoryDto.IsActive;

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ProductCategory/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete categories
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Product category not found.");
            }

            category.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}