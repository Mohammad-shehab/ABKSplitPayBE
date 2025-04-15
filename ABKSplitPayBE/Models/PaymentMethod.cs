using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class PaymentMethod
    {

 
        [Key]
        public int PaymentMethodId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, MaxLength(255)]
        public string Token { get; set; }

        [Required]
        public string LastFourDigits { get; set; }

        [Required, MaxLength(20)]
        public string CardType { get; set; }

        [Required, Range(1, 12)]
        public int ExpiryMonth { get; set; }

        [Required, Range(2000, 9999)]
        public int ExpiryYear { get; set; }

        public bool IsDefault { get; set; } = false;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Installment> Installments { get; set; }
    }
}

