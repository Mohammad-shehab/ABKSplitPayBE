using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ABKSplitPayBE.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentPlan> PaymentPlans { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<WishList> Wishlists { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraints
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<ProductCategory>()
                .HasIndex(pc => pc.Name)
                .IsUnique();
            modelBuilder.Entity<StoreCategory>()
                .HasIndex(sc => sc.Name)
                .IsUnique();
            modelBuilder.Entity<Store>()
                .HasIndex(s => s.Name)
                .IsUnique();
            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.UserId)
                .IsUnique();

            // Configure the Installment -> PaymentMethod relationship to avoid cascading delete cycles
            modelBuilder.Entity<Installment>()
                .HasOne(i => i.PaymentMethod)
                .WithMany(pm => pm.Installments)
                .HasForeignKey(i => i.PaymentMethodId)
                .OnDelete(DeleteBehavior.NoAction); // Change to NO ACTION to avoid cascade path conflict

            // Seed ProductCategories
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory
                {
                    ProductCategoryId = 1,
                    Name = "Electronics",
                    Description = "Devices and gadgets such as smartphones, laptops, and TVs.",
                    PictureUrl = "https://images.unsplash.com/photo-1516321497487-e288fb19713f?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new ProductCategory
                {
                    ProductCategoryId = 2,
                    Name = "School Bills",
                    Description = "Payments for school fees, uniforms, and educational materials.",
                    PictureUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new ProductCategory
                {
                    ProductCategoryId = 3,
                    Name = "Medical Expenses",
                    Description = "Payments for medical bills, consultations, and treatments.",
                    PictureUrl = "https://images.unsplash.com/photo-1576091160397-5d14be92a6ad?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new ProductCategory
                {
                    ProductCategoryId = 4,
                    Name = "Home Furniture",
                    Description = "Furniture items like sofas, beds, and dining tables.",
                    PictureUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                }
            );

            // Seed StoreCategories
            modelBuilder.Entity<StoreCategory>().HasData(
                new StoreCategory
                {
                    StoreCategoryId = 1,
                    Name = "Electronics Stores",
                    Description = "Stores specializing in electronic devices and gadgets.",
                    PictureUrl = "https://images.unsplash.com/photo-1550005799-34c8c3d9c1b6?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new StoreCategory
                {
                    StoreCategoryId = 2,
                    Name = "Furniture Stores",
                    Description = "Stores offering a variety of home furniture.",
                    PictureUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new StoreCategory
                {
                    StoreCategoryId = 3,
                    Name = "Educational Services",
                    Description = "Services related to education, including schools and bookstores.",
                    PictureUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new StoreCategory
                {
                    StoreCategoryId = 4,
                    Name = "Medical Services",
                    Description = "Hospitals, clinics, and pharmacies offering medical services.",
                    PictureUrl = "https://images.unsplash.com/photo-1576091160397-5d14be92a6ad?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                }
            );

            // Seed Stores
            modelBuilder.Entity<Store>().HasData(
                // Electronics Stores
                new Store
                {
                    StoreId = 1,
                    Name = "Jarir Bookstore",
                    Description = "A leading retailer for electronics and books in the Middle East.",
                    WebsiteUrl = "https://www.jarir.com",
                    StoreCategoryId = 1,
                    LogoUrl = "https://www.jarir.com/static/jarir-logo.png",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 2,
                    Name = "Xcite by Alghanim Electronics",
                    Description = "Kuwait's largest electronics retailer.",
                    WebsiteUrl = "https://www.xcite.com",
                    StoreCategoryId = 1,
                    LogoUrl = "https://www.xcite.com/static/xcite-logo.png",
                    IsActive = true
                },
                // Furniture Stores
                new Store
                {
                    StoreId = 3,
                    Name = "IKEA Kuwait",
                    Description = "Affordable furniture and home decor solutions.",
                    WebsiteUrl = "https://www.ikea.com/kw",
                    StoreCategoryId = 2,
                    LogoUrl = "https://www.ikea.com/global/en/images/ikea-logo.svg",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 4,
                    Name = "The One",
                    Description = "Stylish furniture and home accessories.",
                    WebsiteUrl = "https://www.theone.com",
                    StoreCategoryId = 2,
                    LogoUrl = "https://www.theone.com/static/theone-logo.png",
                    IsActive = true
                },
                // Educational Services
                new Store
                {
                    StoreId = 5,
                    Name = "American International School",
                    Description = "A premier international school in Kuwait.",
                    WebsiteUrl = "https://www.ais-kuwait.org",
                    StoreCategoryId = 3,
                    LogoUrl = "https://www.ais-kuwait.org/static/ais-logo.png",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 6,
                    Name = "Al-Ru’ya Bookstore",
                    Description = "A bookstore offering educational materials and school supplies.",
                    WebsiteUrl = "https://www.alruya.com",
                    StoreCategoryId = 3,
                    LogoUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop", // Placeholder
                    IsActive = true
                },
                // Medical Services
                new Store
                {
                    StoreId = 7,
                    Name = "Dar Al Shifa Hospital",
                    Description = "A leading hospital in Kuwait offering comprehensive medical services.",
                    WebsiteUrl = "https://www.daralshifa.com",
                    StoreCategoryId = 4,
                    LogoUrl = "https://www.daralshifa.com/static/daralshifa-logo.png",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 8,
                    Name = "Royale Hayat Hospital",
                    Description = "A premium hospital specializing in various medical treatments.",
                    WebsiteUrl = "https://www.royalehayat.com",
                    StoreCategoryId = 4,
                    LogoUrl = "https://www.royalehayat.com/static/royalehayat-logo.png",
                    IsActive = true
                }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                // Electronics
                new Product
                {
                    ProductId = 1,
                    Name = "Samsung Galaxy S23",
                    Description = "Latest Samsung smartphone with advanced features.",
                    Price = 250.00m,
                    StockQuantity = 50,
                    ProductCategoryId = 1,
                    StoreId = 1, // Jarir Bookstore
                    PictureUrl = "https://images.samsung.com/is/image/samsung/p6pim/ae/2302/gallery/ae-galaxy-s23-s918-sm-s918bzkhmeb-534862463?$650_519_PNG$",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 2,
                    Name = "MacBook Pro",
                    Description = "High-performance laptop for professionals.",
                    Price = 1200.00m,
                    StockQuantity = 30,
                    ProductCategoryId = 1,
                    StoreId = 2, // Xcite
                    PictureUrl = "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mbp-spacegray-select-202206?wid=904&hei=840&fmt=jpeg&qlt=90&.v=1664497359481",
                    IsActive = true
                },
                // School Bills
                new Product
                {
                    ProductId = 3,
                    Name = "AIS Tuition Fee",
                    Description = "Annual tuition fee for American International School.",
                    Price = 5000.00m,
                    StockQuantity = 0, // Unlimited for bills
                    ProductCategoryId = 2,
                    StoreId = 5, // American International School
                    PictureUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 4,
                    Name = "School Uniform",
                    Description = "Standard school uniform set.",
                    Price = 50.00m,
                    StockQuantity = 100,
                    ProductCategoryId = 2,
                    StoreId = 6, // Al-Ru’ya Bookstore
                    PictureUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                // Medical Expenses
                new Product
                {
                    ProductId = 5,
                    Name = "General Checkup",
                    Description = "Comprehensive health checkup package.",
                    Price = 150.00m,
                    StockQuantity = 0, // Unlimited for services
                    ProductCategoryId = 3,
                    StoreId = 7, // Dar Al Shifa Hospital
                    PictureUrl = "https://images.unsplash.com/photo-1576091160397-5d14be92a6ad?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Dental Cleaning",
                    Description = "Professional dental cleaning service.",
                    Price = 80.00m,
                    StockQuantity = 0, // Unlimited for services
                    ProductCategoryId = 3,
                    StoreId = 8, // Royale Hayat Hospital
                    PictureUrl = "https://images.unsplash.com/photo-1576091160397-5d14be92a6ad?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                },
                // Home Furniture
                new Product
                {
                    ProductId = 7,
                    Name = "Sofa Set",
                    Description = "Modern 3-seater sofa set.",
                    Price = 400.00m,
                    StockQuantity = 20,
                    ProductCategoryId = 4,
                    StoreId = 3, // IKEA Kuwait
                    PictureUrl = "https://www.ikea.com/kw/en/images/products/friheten-three-seat-sofa-bed-skiftebo-dark-grey__0245285_pe384403_s5.jpg",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 8,
                    Name = "Dining Table",
                    Description = "Elegant 6-seater dining table.",
                    Price = 300.00m,
                    StockQuantity = 15,
                    ProductCategoryId = 4,
                    StoreId = 4, // The One
                    PictureUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop",
                    IsActive = true
                }
            );
        }
    }
}