using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABKSplitPayBE.Models
{
    public class Installment
    {
        [Key]
        public int InstallmentId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int InstallmentNumber { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(3)]
        public string Currency { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime? PaidDate { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [MaxLength(100)]
        public string? TransactionId { get; set; }

        [Required, MaxLength(20)]
        public string PaymentStatus { get; set; }

        // Relationships
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("PaymentMethodId")]
        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}