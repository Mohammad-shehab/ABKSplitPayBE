using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class PaymentPlan
    {


        [Key]
        public int PaymentPlanId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int NumberOfInstallments { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int IntervalDays { get; set; }

        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; } = 0.00m;

        public bool IsActive { get; set; } = true;

        // Relationships
        public ICollection<Order> Orders { get; set; }
    }

}
