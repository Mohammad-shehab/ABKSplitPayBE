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
    public class InstallmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstallmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Installment/order/{orderId}
        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetInstallments(int orderId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var installments = await _context.Installments
                .Where(i => i.OrderId == orderId)
                .Select(i => new
                {
                    i.InstallmentId,
                    i.InstallmentNumber,
                    i.DueDate,
                    i.Amount,
                    i.Currency,
                    i.IsPaid,
                    i.PaidDate,
                    i.PaymentMethodId,
                    i.TransactionId,
                    i.PaymentStatus,
                    PaymentMethod = i.PaymentMethod != null ? new
                    {
                        i.PaymentMethod.PaymentMethodId,
                        i.PaymentMethod.UserId,
                        i.PaymentMethod.LastFourDigits,
                        i.PaymentMethod.CardType,
                        i.PaymentMethod.ExpiryMonth,
                        i.PaymentMethod.ExpiryYear,
                        i.PaymentMethod.IsDefault,
                        i.PaymentMethod.AddedAt
                    } : null
                })
                .ToListAsync();

            var response = new
            {
                Order = new
                {
                    order.OrderId,
                    order.UserId,
                    order.TotalAmount,
                    order.Currency,
                    order.Status,
                    order.OrderDate,
                    order.Notes,
                    order.ShippingMethod
                },
                Installments = installments
            };

            return Ok(response);
        }

        // GET: api/Installment/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetInstallment(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var installment = await _context.Installments
                .Where(i => i.InstallmentId == id && i.Order.UserId == userId)
                .Select(i => new
                {
                    i.InstallmentId,
                    i.InstallmentNumber,
                    i.DueDate,
                    i.Amount,
                    i.Currency,
                    i.IsPaid,
                    i.PaidDate,
                    i.PaymentMethodId,
                    i.TransactionId,
                    i.PaymentStatus,
                    Order = new
                    {
                        i.Order.OrderId,
                        i.Order.UserId,
                        i.Order.TotalAmount,
                        i.Order.Currency,
                        i.Order.Status,
                        i.Order.OrderDate,
                        i.Order.Notes,
                        i.Order.ShippingMethod
                    },
                    PaymentMethod = i.PaymentMethod != null ? new
                    {
                        i.PaymentMethod.PaymentMethodId,
                        i.PaymentMethod.UserId,
                        i.PaymentMethod.LastFourDigits,
                        i.PaymentMethod.CardType,
                        i.PaymentMethod.ExpiryMonth,
                        i.PaymentMethod.ExpiryYear,
                        i.PaymentMethod.IsDefault,
                        i.PaymentMethod.AddedAt
                    } : null
                })
                .FirstOrDefaultAsync();

            if (installment == null)
            {
                return NotFound("Installment not found.");
            }

            return Ok(installment);
        }

        // POST: api/Installment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateInstallment([FromBody] dynamic installmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate required fields
            int orderId = installmentDto.OrderId;
            int installmentNumber = installmentDto.InstallmentNumber;
            decimal amount = installmentDto.Amount;
            int paymentMethodId = installmentDto.PaymentMethodId;

            if (orderId == 0 || installmentNumber == 0 || amount == 0 || paymentMethodId == 0)
            {
                return BadRequest("OrderId, InstallmentNumber, Amount, and PaymentMethodId are required.");
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return BadRequest("Order not found.");
            }

            var paymentMethod = await _context.PaymentMethods.FindAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                return BadRequest("Payment method not found.");
            }

            var installment = new Installment
            {
                OrderId = orderId,
                InstallmentNumber = installmentNumber,
                DueDate = installmentDto.DueDate,
                Amount = amount,
                Currency = installmentDto.Currency,
                IsPaid = installmentDto.IsPaid ?? false,
                PaidDate = installmentDto.PaidDate,
                PaymentMethodId = paymentMethodId,
                TransactionId = installmentDto.TransactionId,
                PaymentStatus = installmentDto.PaymentStatus
            };

            _context.Installments.Add(installment);
            await _context.SaveChangesAsync();

            // Fetch the created installment for the response
            var createdInstallment = await _context.Installments
                .Where(i => i.InstallmentId == installment.InstallmentId)
                .Select(i => new
                {
                    i.InstallmentId,
                    i.InstallmentNumber,
                    i.DueDate,
                    i.Amount,
                    i.Currency,
                    i.IsPaid,
                    i.PaidDate,
                    i.PaymentMethodId,
                    i.TransactionId,
                    i.PaymentStatus,
                    Order = new
                    {
                        i.Order.OrderId,
                        i.Order.UserId,
                        i.Order.TotalAmount,
                        i.Order.Currency,
                        i.Order.Status,
                        i.Order.OrderDate,
                        i.Order.Notes,
                        i.Order.ShippingMethod
                    },
                    PaymentMethod = i.PaymentMethod != null ? new
                    {
                        i.PaymentMethod.PaymentMethodId,
                        i.PaymentMethod.UserId,
                        i.PaymentMethod.LastFourDigits,
                        i.PaymentMethod.CardType,
                        i.PaymentMethod.ExpiryMonth,
                        i.PaymentMethod.ExpiryYear,
                        i.PaymentMethod.IsDefault,
                        i.PaymentMethod.AddedAt
                    } : null
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetInstallment), new { id = installment.InstallmentId }, createdInstallment);
        }

        // PUT: api/Installment/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateInstallment(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var installment = await _context.Installments
                .Include(i => i.Order)
                .FirstOrDefaultAsync(i => i.InstallmentId == id);

            if (installment == null)
            {
                return NotFound("Installment not found.");
            }

            // Ensure the user can only update installments for their own orders
            if (installment.Order.UserId != userId)
            {
                return Forbid("You are not authorized to update this installment.");
            }

            if (installment.IsPaid)
            {
                return BadRequest("Installment is already paid.");
            }

            // Mark the installment as paid
            installment.IsPaid = true;
            installment.PaidDate = DateTime.UtcNow;
            installment.PaymentStatus = "Paid";

            _context.Entry(installment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Check if all installments for the order are paid
            var allInstallments = await _context.Installments
                .Where(i => i.OrderId == installment.OrderId)
                .ToListAsync();

            if (allInstallments.All(i => i.IsPaid))
            {
                // Update the order status to "Paid"
                installment.Order.Status = "Paid";
                _context.Entry(installment.Order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            // Fetch the updated installment for the response
            var updatedInstallment = await _context.Installments
                .Where(i => i.InstallmentId == id)
                .Select(i => new
                {
                    i.InstallmentId,
                    i.InstallmentNumber,
                    i.DueDate,
                    i.Amount,
                    i.Currency,
                    i.IsPaid,
                    i.PaidDate,
                    i.PaymentMethodId,
                    i.TransactionId,
                    i.PaymentStatus,
                    Order = new
                    {
                        i.Order.OrderId,
                        i.Order.UserId,
                        i.Order.TotalAmount,
                        i.Order.Currency,
                        i.Order.Status,
                        i.Order.OrderDate,
                        i.Order.Notes,
                        i.Order.ShippingMethod
                    },
                    PaymentMethod = i.PaymentMethod != null ? new
                    {
                        i.PaymentMethod.PaymentMethodId,
                        i.PaymentMethod.UserId,
                        i.PaymentMethod.LastFourDigits,
                        i.PaymentMethod.CardType,
                        i.PaymentMethod.ExpiryMonth,
                        i.PaymentMethod.ExpiryYear,
                        i.PaymentMethod.IsDefault,
                        i.PaymentMethod.AddedAt
                    } : null
                })
                .FirstOrDefaultAsync();

            return Ok(updatedInstallment);
        }

        // PUT: api/Installment/order/{orderId}/pay
        [HttpPut("order/{orderId}/pay")]
        [Authorize]
        public async Task<IActionResult> PayInstallments(int orderId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var installments = await _context.Installments
                .Where(i => i.OrderId == orderId)
                .ToListAsync();

            if (!installments.Any())
            {
                return NotFound("No installments found for this order.");
            }

            foreach (var installment in installments)
            {
                if (!installment.IsPaid) // Only update unpaid installments
                {
                    installment.IsPaid = true;
                    installment.PaidDate = DateTime.UtcNow;
                    installment.PaymentStatus = "Paid";
                    _context.Entry(installment).State = EntityState.Modified;
                }
            }

            // Update the order status to "Paid" if all installments are paid
            if (installments.All(i => i.IsPaid))
            {
                order.Status = "Paid";
                _context.Entry(order).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            // Fetch the updated installments for the response
            var updatedInstallments = await _context.Installments
                .Where(i => i.OrderId == orderId)
                .Select(i => new
                {
                    i.InstallmentId,
                    i.InstallmentNumber,
                    i.DueDate,
                    i.Amount,
                    i.Currency,
                    i.IsPaid,
                    i.PaidDate,
                    i.PaymentMethodId,
                    i.TransactionId,
                    i.PaymentStatus,
                    PaymentMethod = i.PaymentMethod != null ? new
                    {
                        i.PaymentMethod.PaymentMethodId,
                        i.PaymentMethod.UserId,
                        i.PaymentMethod.LastFourDigits,
                        i.PaymentMethod.CardType,
                        i.PaymentMethod.ExpiryMonth,
                        i.PaymentMethod.ExpiryYear,
                        i.PaymentMethod.IsDefault,
                        i.PaymentMethod.AddedAt
                    } : null
                })
                .ToListAsync();

            var response = new
            {
                Order = new
                {
                    order.OrderId,
                    order.UserId,
                    order.TotalAmount,
                    order.Currency,
                    order.Status,
                    order.OrderDate,
                    order.Notes,
                    order.ShippingMethod
                },
                Installments = updatedInstallments
            };

            return Ok(response);
        }

        // DELETE: api/Installment/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInstallment(int id)
        {
            var installment = await _context.Installments.FindAsync(id);
            if (installment == null)
            {
                return NotFound("Installment not found.");
            }

            _context.Installments.Remove(installment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}