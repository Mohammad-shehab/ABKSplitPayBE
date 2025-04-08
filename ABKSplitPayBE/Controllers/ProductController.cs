using ABKSplitPayBE.Data;
using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABKSplitPayBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTO for creating/updating products
        public class ProductCreateUpdateDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public int ProductCategoryId { get; set; }
            public int? StoreId { get; set; }
            public string PictureUrl { get; set; }
            public bool IsActive { get; set; }
        }

        // DTO for API responses (GET)
        public class ProductResponseDto
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public int ProductCategoryId { get; set; }
            public int? StoreId { get; set; }
            public string PictureUrl { get; set; }
            public bool IsActive { get; set; }
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ProductCategoryId = p.ProductCategoryId,
                    StoreId = p.StoreId,
                    PictureUrl = p.PictureUrl,
                    IsActive = p.IsActive
                })
                .ToListAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == id && p.IsActive)
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ProductCategoryId = p.ProductCategoryId,
                    StoreId = p.StoreId,
                    PictureUrl = p.PictureUrl,
                    IsActive = p.IsActive
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only admins can create products
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductCreateUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                ProductCategoryId = productDto.ProductCategoryId,
                StoreId = productDto.StoreId,
                PictureUrl = productDto.PictureUrl,
                IsActive = productDto.IsActive
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var createdProductDto = new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ProductCategoryId = product.ProductCategoryId,
                StoreId = product.StoreId,
                PictureUrl = product.PictureUrl,
                IsActive = product.IsActive
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, createdProductDto);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can update products
        public async Task<IActionResult> UpdateProduct(int id, ProductCreateUpdateDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            product.Name = productDto.Name ?? product.Name;
            product.Description = productDto.Description ?? product.Description;
            product.Price = productDto.Price != 0 ? productDto.Price : product.Price;
            product.StockQuantity = productDto.StockQuantity != 0 ? productDto.StockQuantity : product.StockQuantity;
            product.ProductCategoryId = productDto.ProductCategoryId != 0 ? productDto.ProductCategoryId : product.ProductCategoryId;
            product.StoreId = productDto.StoreId ?? product.StoreId;
            product.PictureUrl = productDto.PictureUrl ?? product.PictureUrl;
            product.IsActive = productDto.IsActive;

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete products
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            product.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}