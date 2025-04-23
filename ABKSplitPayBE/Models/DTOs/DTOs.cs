namespace ABKSplitPayBE.Models.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ProductCategoryId { get; set; }
        public int? StoreId { get; set; }
        public string PictureUrl { get; set; }
        public bool IsActive { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
        public StoreDto Store { get; set; }
    }
    public class ProductCategoryDto
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public bool IsActive { get; set; }
    }
    public class StoreDto
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public int StoreCategoryId { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public StoreCategoryDto StoreCategory { get; set; }
    }
    public class StoreCategoryDto
    {
        public int StoreCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public bool IsActive { get; set; }
    }
}