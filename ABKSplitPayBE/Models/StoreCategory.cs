using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class StoreCategory
    {
        [Key]
        public int StoreCategoryId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(255)]
        public string PictureUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Store> Stores { get; set; }    
    }
}
