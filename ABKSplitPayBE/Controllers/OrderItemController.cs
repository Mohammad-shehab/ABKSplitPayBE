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
    public class OrderItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderItem/order/{orderId}
        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<CartItem>> GetOrderItems(int orderId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();

            return Ok(orderItems);
        }

        // GET: api/OrderItem/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var orderItem = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == id && oi.Order.UserId == userId);

            if (orderItem == null)
            {
                return NotFound("Order item not found.");
            }

            return Ok(orderItem);
        }

        // Note: POST, PUT, DELETE for OrderItem are not typically needed as they are managed via Order creation
    }
}