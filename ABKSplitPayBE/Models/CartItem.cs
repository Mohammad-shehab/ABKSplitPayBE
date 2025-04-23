using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}

