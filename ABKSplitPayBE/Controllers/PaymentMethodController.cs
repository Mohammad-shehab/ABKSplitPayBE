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
    public class PaymentMethodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PaymentMethodController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class PaymentMethodDto
        {
            public string Token { get; set; }
            public string LastFourDigits { get; set; }
            public string CardType { get; set; }
            public int ExpiryMonth { get; set; }
            public int ExpiryYear { get; set; }
            public bool IsDefault { get; set; }
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PaymentMethod>> GetPaymentMethods()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var paymentMethods = await _context.PaymentMethods
                .Where(pm => pm.UserId == userId)
                .ToListAsync();

            return Ok(paymentMethods);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PaymentMethod>> GetPaymentMethod(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(pm => pm.PaymentMethodId == id && pm.UserId == userId);

            if (paymentMethod == null)
            {
                return NotFound("Payment method not found.");
            }

            return Ok(paymentMethod);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PaymentMethod>> CreatePaymentMethod(PaymentMethodDto paymentMethodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (paymentMethodDto.IsDefault)
            {
                var existingDefault = await _context.PaymentMethods
                    .Where(pm => pm.UserId == userId && pm.IsDefault)
                    .ToListAsync();
                foreach (var pm in existingDefault)
                {
                    pm.IsDefault = false;
                }
            }
            var paymentMethod = new PaymentMethod
            {
                UserId = userId,
                Token = paymentMethodDto.Token,
                LastFourDigits = paymentMethodDto.LastFourDigits,
                CardType = paymentMethodDto.CardType,
                ExpiryMonth = paymentMethodDto.ExpiryMonth,
                ExpiryYear = paymentMethodDto.ExpiryYear,
                IsDefault = paymentMethodDto.IsDefault,
                AddedAt = DateTime.UtcNow
            };
            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentMethod), new { id = paymentMethod.PaymentMethodId }, paymentMethod);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentMethod(int id, PaymentMethodDto paymentMethodDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(pm => pm.PaymentMethodId == id && pm.UserId == userId);

            if (paymentMethod == null)
            {
                return NotFound("Payment method not found.");
            }

            if (paymentMethodDto.IsDefault)
            {
                var existingDefault = await _context.PaymentMethods
                    .Where(pm => pm.UserId == userId && pm.IsDefault && pm.PaymentMethodId != id)
                    .ToListAsync();
                foreach (var pm in existingDefault)
                {
                    pm.IsDefault = false;
                }
            }
            paymentMethod.Token = paymentMethodDto.Token ?? paymentMethod.Token;
            paymentMethod.LastFourDigits = paymentMethodDto.LastFourDigits ?? paymentMethod.LastFourDigits;
            paymentMethod.CardType = paymentMethodDto.CardType ?? paymentMethod.CardType;
            paymentMethod.ExpiryMonth = paymentMethodDto.ExpiryMonth != 0 ? paymentMethodDto.ExpiryMonth : paymentMethod.ExpiryMonth;
            paymentMethod.ExpiryYear = paymentMethodDto.ExpiryYear != 0 ? paymentMethodDto.ExpiryYear : paymentMethod.ExpiryYear;
            paymentMethod.IsDefault = paymentMethodDto.IsDefault;

            _context.Entry(paymentMethod).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(pm => pm.PaymentMethodId == id && pm.UserId == userId);

            if (paymentMethod == null)
            {
                return NotFound("Payment method not found.");
            }
            var installmentsUsingMethod = await _context.Installments
                .Where(i => i.PaymentMethodId == id)
                .ToListAsync();

            if (installmentsUsingMethod.Any())
            {
                return BadRequest("Cannot delete payment method because it is associated with existing installments.");
            }
            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}