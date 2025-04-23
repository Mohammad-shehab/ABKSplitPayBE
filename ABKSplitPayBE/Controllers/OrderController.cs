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
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class OrderDto
        {
            public int PaymentPlanId { get; set; }
            public int? ShippingAddressId { get; set; }
            public string Currency { get; set; }
            public string Notes { get; set; }
            public string ShippingMethod { get; set; }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new
                {
                    orderId = o.OrderId,
                    userId = o.UserId,
                    paymentPlanId = o.PaymentPlanId,
                    shippingAddressId = o.ShippingAddressId,
                    totalAmount = o.TotalAmount,
                    currency = o.Currency,
                    status = o.Status,
                    orderDate = o.OrderDate,
                    notes = o.Notes,
                    shippingMethod = o.ShippingMethod,
                    productNames = o.OrderItems.Select(oi => oi.Product.Name).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var order = await _context.Orders
                .Where(o => o.OrderId == id && o.UserId == userId)
                .Select(o => new
                {
                    orderId = o.OrderId,
                    userId = o.UserId,
                    paymentPlanId = o.PaymentPlanId,
                    shippingAddressId = o.ShippingAddressId,
                    totalAmount = o.TotalAmount,
                    currency = o.Currency,
                    status = o.Status,
                    orderDate = o.OrderDate,
                    notes = o.Notes,
                    shippingMethod = o.ShippingMethod,
                    productNames = o.OrderItems.Select(oi => oi.Product.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }
        [HttpGet("points")]
        [Authorize]
        public async Task<IActionResult> GetPoints()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var paidOrdersTotal = await _context.Orders
                .Where(o => o.UserId == userId && o.Status == "Paid")
                .SumAsync(o => o.TotalAmount);

            return Ok(new { points = paidOrdersTotal });
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("Cart is empty or not found.");
            }

            var paymentPlan = await _context.PaymentPlans.FindAsync(orderDto.PaymentPlanId);
            if (paymentPlan == null || !paymentPlan.IsActive)
            {
                return BadRequest("Payment plan not found or inactive.");
            }

            Address shippingAddress = null;
            if (orderDto.ShippingAddressId.HasValue)
            {
                shippingAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.AddressId == orderDto.ShippingAddressId && a.UserId == userId);
                if (shippingAddress == null)
                {
                    return BadRequest("Shipping address not found.");
                }
            }
            decimal totalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            var order = new Order
            {
                UserId = userId,
                PaymentPlanId = orderDto.PaymentPlanId,
                ShippingAddressId = orderDto.ShippingAddressId,
                TotalAmount = totalAmount,
                Currency = orderDto.Currency ?? "KWD",
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                Notes = orderDto.Notes,
                ShippingMethod = orderDto.ShippingMethod
            };
            order.OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList();
            var paymentMethod = await _context.PaymentMethods.FirstOrDefaultAsync(pm => pm.UserId == userId);
            if (paymentMethod == null)
            {
                return BadRequest("No valid payment method found for the user.");
            }
            decimal installmentAmount = totalAmount / paymentPlan.NumberOfInstallments;
            order.Installments = Enumerable.Range(1, paymentPlan.NumberOfInstallments)
                .Select(i => new Installment
                {
                    InstallmentNumber = i,
                    DueDate = DateTime.UtcNow.AddDays(i * paymentPlan.IntervalDays),
                    Amount = installmentAmount,
                    Currency = order.Currency,
                    IsPaid = false,
                    PaymentStatus = "Pending",
                    PaymentMethodId = paymentMethod.PaymentMethodId, 
                    TransactionId = Guid.NewGuid().ToString() 
                }).ToList();

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDto orderDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var paymentPlan = await _context.PaymentPlans.FindAsync(orderDto.PaymentPlanId);
            if (paymentPlan == null || !paymentPlan.IsActive)
            {
                return BadRequest("Payment plan not found or inactive.");
            }

            Address shippingAddress = null;
            if (orderDto.ShippingAddressId.HasValue)
            {
                shippingAddress = await _context.Addresses.FindAsync(orderDto.ShippingAddressId);
                if (shippingAddress == null)
                {
                    return BadRequest("Shipping address not found.");
                }
            }

            order.PaymentPlanId = orderDto.PaymentPlanId != 0 ? orderDto.PaymentPlanId : order.PaymentPlanId;
            order.ShippingAddressId = orderDto.ShippingAddressId ?? order.ShippingAddressId;
            order.Currency = orderDto.Currency ?? order.Currency;
            order.Notes = orderDto.Notes ?? order.Notes;
            order.ShippingMethod = orderDto.ShippingMethod ?? order.ShippingMethod;
            order.UpdatedAt = DateTime.UtcNow;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}