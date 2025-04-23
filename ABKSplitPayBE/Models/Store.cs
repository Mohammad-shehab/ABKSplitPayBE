using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required, MaxLength(255)]
        public string WebsiteUrl { get; set; }
        [Required]
        public int StoreCategoryId { get; set; }
        [MaxLength(255)]
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; } = true;
        [ForeignKey("StoreCategoryId")]
        public StoreCategory StoreCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
