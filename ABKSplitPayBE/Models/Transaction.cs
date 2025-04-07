using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class Transaction
    {

        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int InstallmentId { get; set; }

        [Required, MaxLength(100)]
        public string TransactionReference { get; set; } // Transaction ID from the payment processor

        [Required]
        public DateTime PayDate { get; set; } // Date of the payment attempt

        [Required, MaxLength(20)]
        public string Status { get; set; } // e.g., "Success", "Failed"

        // Relationships
        [ForeignKey("InstallmentId")]
        public Installment Installment { get; set; }
    }
}

