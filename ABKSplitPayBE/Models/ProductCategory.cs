using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(255)]
        public string PictureUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Product> Products { get; set; } 
    }
}
