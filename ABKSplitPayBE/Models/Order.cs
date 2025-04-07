using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class Order
    {


  
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int PaymentPlanId { get; set; }

        public int? ShippingAddressId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(3)]
        public string Currency { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [MaxLength(50)]
        public string ShippingMethod { get; set; }

        // Relationships
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("PaymentPlanId")]
        public PaymentPlan PaymentPlan { get; set; }

        [ForeignKey("ShippingAddressId")]
        public Address ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Installment> Installments { get; set; }
    }
}

