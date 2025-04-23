using ABKSplitPayBE.Data;
using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ABKSplitPayBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class WishListDto
        {
            public int ProductId { get; set; }
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<WishList>> GetWishList()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var wishList = await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return Ok(wishList);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WishList>> GetWishListItem(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var wishListItem = await _context.Wishlists
                .Include(w => w.Product)
                .FirstOrDefaultAsync(w => w.WishListId == id && w.UserId == userId);

            if (wishListItem == null)
            {
                return NotFound("Wishlist item not found.");
            }

            return Ok(wishListItem);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WishList>> CreateWishListItem(WishListDto wishListDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var product = await _context.Products.FindAsync(wishListDto.ProductId);
            if (product == null || !product.IsActive)
            {
                return BadRequest("Product not found or inactive.");
            }
            var existingItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == wishListDto.ProductId);
            if (existingItem != null)
            {
                return BadRequest("Product is already in the wishlist.");
            }

            var wishListItem = new WishList
            {
                UserId = userId,
                ProductId = wishListDto.ProductId,
                AddedDate = DateTime.UtcNow
            };
            _context.Wishlists.Add(wishListItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWishListItem), new { id = wishListItem.WishListId }, wishListItem);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWishListItem(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var wishListItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.WishListId == id && w.UserId == userId);
            if (wishListItem == null)
            {
                return NotFound("Wishlist item not found.");
            }
            _context.Wishlists.Remove(wishListItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}