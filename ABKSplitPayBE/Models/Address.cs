using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [Required, MaxLength(255)]
        public string AddressLine1 { get; set; }
        [MaxLength(255)]
        public string AddressLine2 { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [MaxLength(100)]
        public string State { get; set; }
        [Required, MaxLength(20)]
        public string PostalCode { get; set; }
        [Required, MaxLength(100)]
        public string Country { get; set; }
        public bool IsDefault { get; set; } = false;
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}

