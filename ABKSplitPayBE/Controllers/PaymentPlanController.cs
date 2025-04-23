using ABKSplitPayBE.Data;
using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABKSplitPayBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentPlanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PaymentPlanController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class PaymentPlanDto
        {
            public string Name { get; set; }
            public int NumberOfInstallments { get; set; }
            public int IntervalDays { get; set; }
            public decimal InterestRate { get; set; }
            public bool IsActive { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentPlan>>> GetPaymentPlans()
        {
            var paymentPlans = await _context.PaymentPlans
                .Where(pp => pp.IsActive)
                .ToListAsync();
            return Ok(paymentPlans);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentPlan>> GetPaymentPlan(int id)
        {
            var paymentPlan = await _context.PaymentPlans
                .FirstOrDefaultAsync(pp => pp.PaymentPlanId == id && pp.IsActive);

            if (paymentPlan == null)
            {
                return NotFound("Payment plan not found.");
            }

            return Ok(paymentPlan);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaymentPlan>> CreatePaymentPlan(PaymentPlanDto paymentPlanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentPlan = new PaymentPlan
            {
                Name = paymentPlanDto.Name,
                NumberOfInstallments = paymentPlanDto.NumberOfInstallments,
                IntervalDays = paymentPlanDto.IntervalDays,
                InterestRate = paymentPlanDto.InterestRate,
                IsActive = paymentPlanDto.IsActive
            };

            _context.PaymentPlans.Add(paymentPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentPlan), new { id = paymentPlan.PaymentPlanId }, paymentPlan);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePaymentPlan(int id, PaymentPlanDto paymentPlanDto)
        {
            var paymentPlan = await _context.PaymentPlans.FindAsync(id);
            if (paymentPlan == null)
            {
                return NotFound("Payment plan not found.");
            }

            paymentPlan.Name = paymentPlanDto.Name ?? paymentPlan.Name;
            paymentPlan.NumberOfInstallments = paymentPlanDto.NumberOfInstallments != 0 ? paymentPlanDto.NumberOfInstallments : paymentPlan.NumberOfInstallments;
            paymentPlan.IntervalDays = paymentPlanDto.IntervalDays != 0 ? paymentPlanDto.IntervalDays : paymentPlan.IntervalDays;
            paymentPlan.InterestRate = paymentPlanDto.InterestRate != 0 ? paymentPlanDto.InterestRate : paymentPlan.InterestRate;
            paymentPlan.IsActive = paymentPlanDto.IsActive;

            _context.Entry(paymentPlan).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePaymentPlan(int id)
        {
            var paymentPlan = await _context.PaymentPlans.FindAsync(id);
            if (paymentPlan == null)
            {
                return NotFound("Payment plan not found.");
            }
            paymentPlan.IsActive = false; 
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}