using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class WishList
    {
        [Key]
        public int WishListId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
