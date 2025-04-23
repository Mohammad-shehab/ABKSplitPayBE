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
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class TransactionDto
        {
            public int InstallmentId { get; set; }
            public string TransactionReference { get; set; }
            public DateTime PayDate { get; set; }
            public string Status { get; set; }
        }
        [HttpGet("installment/{installmentId}")]
        [Authorize]
        public async Task<ActionResult<Transaction>> GetTransactions(int installmentId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var installment = await _context.Installments
                .Include(i => i.Order)
                .FirstOrDefaultAsync(i => i.InstallmentId == installmentId && i.Order.UserId == userId);

            if (installment == null)
            {
                return NotFound("Installment not found.");
            }

            var transactions = await _context.Transactions
                .Where(t => t.InstallmentId == installmentId)
                .ToListAsync();

            return Ok(transactions);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var transaction = await _context.Transactions
                .Include(t => t.Installment)
                .ThenInclude(i => i.Order)
                .FirstOrDefaultAsync(t => t.TransactionId == id && t.Installment.Order.UserId == userId);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            return Ok(transaction);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Transaction>> CreateTransaction(TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var installment = await _context.Installments.FindAsync(transactionDto.InstallmentId);
            if (installment == null)
            {
                return BadRequest("Installment not found.");
            }
            var transaction = new Transaction
            {
                InstallmentId = transactionDto.InstallmentId,
                TransactionReference = transactionDto.TransactionReference,
                PayDate = transactionDto.PayDate,
                Status = transactionDto.Status
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTransaction(int id, TransactionDto transactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }
            var installment = await _context.Installments.FindAsync(transactionDto.InstallmentId);
            if (installment == null)
            {
                return BadRequest("Installment not found.");
            }
            transaction.InstallmentId = transactionDto.InstallmentId != 0 ? transactionDto.InstallmentId : transaction.InstallmentId;
            transaction.TransactionReference = transactionDto.TransactionReference ?? transaction.TransactionReference;
            transaction.PayDate = transactionDto.PayDate != default ? transactionDto.PayDate : transaction.PayDate;
            transaction.Status = transactionDto.Status ?? transaction.Status;
            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}