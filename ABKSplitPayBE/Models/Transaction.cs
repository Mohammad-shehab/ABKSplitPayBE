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
        public string TransactionReference { get; set; } 
        [Required]
        public DateTime PayDate { get; set; } 
        [Required, MaxLength(20)]
        public string Status { get; set; } 
        [ForeignKey("InstallmentId")]
        public Installment Installment { get; set; }
    }
}

