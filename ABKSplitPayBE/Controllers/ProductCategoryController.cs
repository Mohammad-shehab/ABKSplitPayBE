using ABKSplitPayBE.Data;
using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public class ProductCategoryDto
        {
            [Required(ErrorMessage = "Name is required.")]
            [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
            public string Name { get; set; }

            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
            public string Description { get; set; }

            [Url(ErrorMessage = "PictureUrl must be a valid URL.")]
            public string PictureUrl { get; set; }

            public bool IsActive { get; set; }
        }
        public class ProductCategoryResponseDto
        {
            public int ProductCategoryId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string PictureUrl { get; set; }
            public bool IsActive { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategoryResponseDto>>> GetProductCategories()
        {
            var categories = await _context.ProductCategories
                .Where(pc => pc.IsActive)
                .Select(pc => new ProductCategoryResponseDto
                {
                    ProductCategoryId = pc.ProductCategoryId,
                    Name = pc.Name,
                    Description = pc.Description,
                    PictureUrl = pc.PictureUrl,
                    IsActive = pc.IsActive
                })
                .ToListAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryResponseDto>> GetProductCategory(int id)
        {
            var category = await _context.ProductCategories
                .Where(pc => pc.ProductCategoryId == id && pc.IsActive)
                .Select(pc => new ProductCategoryResponseDto
                {
                    ProductCategoryId = pc.ProductCategoryId,
                    Name = pc.Name,
                    Description = pc.Description,
                    PictureUrl = pc.PictureUrl,
                    IsActive = pc.IsActive
                })
                .FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound($"Product category with ID {id} not found or is inactive.");
            }

            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductCategoryResponseDto>> CreateProductCategory(ProductCategoryDto categoryDto)
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

            var createdCategoryDto = new ProductCategoryResponseDto
            {
                ProductCategoryId = category.ProductCategoryId,
                Name = category.Name,
                Description = category.Description,
                PictureUrl = category.PictureUrl,
                IsActive = category.IsActive
            };
            return CreatedAtAction(nameof(GetProductCategory), new { id = category.ProductCategoryId }, createdCategoryDto);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductCategory(int id, ProductCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Product category with ID {id} not found.");
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
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Product category with ID {id} not found.");
            }

            if (!category.IsActive)
            {
                return BadRequest("Product category is already inactive.");
            }
            category.IsActive = false; 
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}