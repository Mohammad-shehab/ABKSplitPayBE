using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABKSplitPayBE.Models
{
    public class Product
    {




        [Key]
        public int ProductId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; } = 0;

        [Required]
        public int ProductCategoryId { get; set; }

        public int? StoreId { get; set; }

        [MaxLength(255)]
        public string PictureUrl { get; set; }

        public bool IsActive { get; set; } = true;

        // Relationships
        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<WishList> WishlistItems { get; set; }
    }

}
