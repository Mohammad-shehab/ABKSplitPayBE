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

        public class InstallmentDto
        {
            public int OrderId { get; set; }
            public int InstallmentNumber { get; set; }
            public DateTime DueDate { get; set; }
            public decimal Amount { get; set; }
            public string Currency { get; set; }
            public bool IsPaid { get; set; }
            public DateTime? PaidDate { get; set; }
            public int PaymentMethodId { get; set; }
            public string TransactionId { get; set; }
            public string PaymentStatus { get; set; }
        }

        // GET: api/Installment/order/{orderId}
        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<Installment>> GetInstallments(int orderId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var installments = await _context.Installments
                .Include(i => i.Order)
                .Include(i => i.PaymentMethod)
                .Include(i => i.Transactions)
                .Where(i => i.OrderId == orderId)
                .ToListAsync();

            return Ok(installments);
        }

        // GET: api/Installment/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Installment>> GetInstallment(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var installment = await _context.Installments
                .Include(i => i.Order)
                .Include(i => i.PaymentMethod)
                .Include(i => i.Transactions)
                .FirstOrDefaultAsync(i => i.InstallmentId == id && i.Order.UserId == userId);

            if (installment == null)
            {
                return NotFound("Installment not found.");
            }

            return Ok(installment);
        }

        // POST: api/Installment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Installment>> CreateInstallment(InstallmentDto installmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Orders.FindAsync(installmentDto.OrderId);
            if (order == null)
            {
                return BadRequest("Order not found.");
            }

            var paymentMethod = await _context.PaymentMethods.FindAsync(installmentDto.PaymentMethodId);
            if (paymentMethod == null)
            {
                return BadRequest("Payment method not found.");
            }

            var installment = new Installment
            {
                OrderId = installmentDto.OrderId,
                InstallmentNumber = installmentDto.InstallmentNumber,
                DueDate = installmentDto.DueDate,
                Amount = installmentDto.Amount,
                Currency = installmentDto.Currency,
                IsPaid = installmentDto.IsPaid,
                PaidDate = installmentDto.PaidDate,
                PaymentMethodId = installmentDto.PaymentMethodId,
                TransactionId = installmentDto.TransactionId,
                PaymentStatus = installmentDto.PaymentStatus
            };

            _context.Installments.Add(installment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInstallment), new { id = installment.InstallmentId }, installment);
        }

        // PUT: api/Installment/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInstallment(int id, InstallmentDto installmentDto)
        {
            var installment = await _context.Installments.FindAsync(id);
            if (installment == null)
            {
                return NotFound("Installment not found.");
            }

            var paymentMethod = await _context.PaymentMethods.FindAsync(installmentDto.PaymentMethodId);
            if (paymentMethod == null)
            {
                return BadRequest("Payment method not found.");
            }

            installment.OrderId = installmentDto.OrderId != 0 ? installmentDto.OrderId : installment.OrderId;
            installment.InstallmentNumber = installmentDto.InstallmentNumber != 0 ? installmentDto.InstallmentNumber : installment.InstallmentNumber;
            installment.DueDate = installmentDto.DueDate != default ? installmentDto.DueDate : installment.DueDate;
            installment.Amount = installmentDto.Amount != 0 ? installmentDto.Amount : installment.Amount;
            installment.Currency = installmentDto.Currency ?? installment.Currency;
            installment.IsPaid = installmentDto.IsPaid;
            installment.PaidDate = installmentDto.PaidDate ?? installment.PaidDate;
            installment.PaymentMethodId = installmentDto.PaymentMethodId != 0 ? installmentDto.PaymentMethodId : installment.PaymentMethodId;
            installment.TransactionId = installmentDto.TransactionId ?? installment.TransactionId;
            installment.PaymentStatus = installmentDto.PaymentStatus ?? installment.PaymentStatus;

            _context.Entry(installment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
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