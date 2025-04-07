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
    public class CartItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class CartItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        // GET: api/CartItem
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<CartItem>> GetCartItems()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            return Ok(cart.CartItems);
        }

        // GET: api/CartItem/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CartItem>> GetCartItem(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartItemId == id && ci.Cart.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            return Ok(cartItem);
        }

        // POST: api/CartItem
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CartItem>> CreateCartItem(CartItemDto cartItemDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return BadRequest("Cart not found. Please create a cart first.");
            }

            var product = await _context.Products.FindAsync(cartItemDto.ProductId);
            if (product == null || !product.IsActive)
            {
                return BadRequest("Product not found or inactive.");
            }

            if (cartItemDto.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity,
                AddedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.CartItemId }, cartItem);
        }

        // PUT: api/CartItem/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem(int id, CartItemDto cartItemDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == id && ci.Cart.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            var product = await _context.Products.FindAsync(cartItemDto.ProductId);
            if (product == null || !product.IsActive)
            {
                return BadRequest("Product not found or inactive.");
            }

            if (cartItemDto.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

            cartItem.ProductId = cartItemDto.ProductId != 0 ? cartItemDto.ProductId : cartItem.ProductId;
            cartItem.Quantity = cartItemDto.Quantity != 0 ? cartItemDto.Quantity : cartItem.Quantity;
            cartItem.Cart.UpdatedAt = DateTime.UtcNow;

            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/CartItem/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == id && ci.Cart.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            _context.CartItems.Remove(cartItem);
            cartItem.Cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}