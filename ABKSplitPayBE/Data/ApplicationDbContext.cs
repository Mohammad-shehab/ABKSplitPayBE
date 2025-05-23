﻿using ABKSplitPayBE.Models;
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
            modelBuilder.Entity<Installment>()
                .HasOne(i => i.PaymentMethod)
                .WithMany(pm => pm.Installments)
                .HasForeignKey(i => i.PaymentMethodId)
                .OnDelete(DeleteBehavior.NoAction);






            var paymentPlans = new List<PaymentPlan>();

            // Add the first payment plan as "Pay Full"
            paymentPlans.Add(new PaymentPlan
            {
                PaymentPlanId = 1,
                Name = "Pay Full",
                NumberOfInstallments = 1,
                IntervalDays = 0, // No interval for a single payment
                InterestRate = 0,
                IsActive = true
            });

            // Add the remaining payment plans
            for (int i = 2; i <= 12; i++)
            {
                paymentPlans.Add(new PaymentPlan
                {
                    PaymentPlanId = i,
                    Name = $"{i} Month",
                    NumberOfInstallments = i,
                    IntervalDays = 30,
                    InterestRate = 0,
                    IsActive = true
                });
            }

            modelBuilder.Entity<PaymentPlan>().HasData(paymentPlans);

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory
                {
                    ProductCategoryId = 1,
                    Name = "Electronics",
                    Description = "Devices and gadgets such as smartphones, laptops, and TVs.",
                    PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQGwOifpWIFylO-2Lkn8Zi5ECXt6PFr-ihOuw&s",
                    IsActive = true
                },
                new ProductCategory
                {
                    ProductCategoryId = 2,
                    Name = "School Bills",
                    Description = "Payments for school fees, uniforms, and educational materials.",
                    PictureUrl = "https://assets.visme.co/templates/banners/thumbnails/i_School-Tuition-Receipt_full.jpg",
                    IsActive = true
                },
                new ProductCategory
                {
                    ProductCategoryId = 3,
                    Name = "Medical Expenses",
                    Description = "Payments for medical bills, consultations, and treatments.",
                    PictureUrl = "https://www.aklerbrowning.com/wp-content/uploads/medical-expenses.jpg",
                    IsActive = true
                },
                new ProductCategory
                {
                    ProductCategoryId = 4,
                    Name = "Home Furniture",
                    Description = "Furniture items like sofas, beds, and dining tables.",
                    PictureUrl = "https://www.athome-furniture.com/web/image/159801-47a6cc44/OUTDOOghR-BANNER-ATHOME--copy-copy.jpg",
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
                    PictureUrl = "https://i0.wp.com/zilani-int.com/wp-content/uploads/2023/02/8ebb2cb57bdb71ac4f0aaadfd61911d5.jpeg?w=680&ssl=1",
                    IsActive = true
                },
                new StoreCategory
                {
                    StoreCategoryId = 2,
                    Name = "Furniture Stores",
                    Description = "Stores offering a variety of home furniture.",
                    PictureUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/IKEA_Shopping_centre_Sweden.jpg/1200px-IKEA_Shopping_centre_Sweden.jpg",
                    IsActive = true
                },
                new StoreCategory
                {
                    StoreCategoryId = 3,
                    Name = "Educational Services",
                    Description = "Services related to education, including schools and bookstores.",
                    PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQiCK3dZsrWBzP6I0HBpEjA-1-AqZoEeF1drQ&s",
                    IsActive = true
                },
                new StoreCategory
                {
                    StoreCategoryId = 4,
                    Name = "Medical Services",
                    Description = "Hospitals, clinics, and pharmacies offering medical services.",
                    PictureUrl = "https://previews.123rf.com/images/shutterboythailand/shutterboythailand1604/shutterboythailand160400336/58429893-medical-service-word-on-tablet-screen-with-medical-equipment-on-background.jpg",
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
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT_EYfEMiV59HZO1JVMunmUd-XHOpbVrWI8DQ&s",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 2,
                    Name = "Xcite by Alghanim Electronics",
                    Description = "Kuwait's largest electronics retailer.",
                    WebsiteUrl = "https://www.xcite.com",
                    StoreCategoryId = 1,
                    LogoUrl = "https://www.xcite.com/assets/icons/logo.jpg",
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
                    LogoUrl = "https://static.dezeen.com/uploads/2019/04/ikea-logo-new-hero-1.jpg",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 4,
                    Name = "Abyat",
                    Description = "Stylish furniture and home accessories.",
                    WebsiteUrl = "https://www.abyat.com/kw/ar",
                    StoreCategoryId = 2,
                    LogoUrl = "https://d1yjjnpx0p53s8.cloudfront.net/styles/large/s3/abyat_wing_logo_0.jpg?itok=Trhx2GSu",
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
                    LogoUrl = "https://media.licdn.com/dms/image/v2/C561BAQE_m2Ujim4lyQ/company-background_10000/company-background_10000/0/1584559760344/american_international_school_kuwait_cover?e=2147483647&v=beta&t=zt2lRU5uJA-o2CHmJuVDqoWV8SIqa_W_F69eqQ3yRhs",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 6,
                    Name = "AUM",
                    Description = "AUM Top university in Kuwait in QS & Times Higher Education Rankings",
                    WebsiteUrl = "https://www.aum.edu.kw/",
                    StoreCategoryId = 3,
                    LogoUrl = "https://www.aum.edu.kw/images/ShareLogo.jpg", 
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
                    LogoUrl = "https://purchase.daralshifa.com/images/logo.png",
                    IsActive = true
                },
                new Store
                {
                    StoreId = 8,
                    Name = "Royale Hayat Hospital",
                    Description = "A premium hospital specializing in various medical treatments.",
                    WebsiteUrl = "https://www.royalehayat.com",
                    StoreCategoryId = 4,
                    LogoUrl = "https://www.sharpersoftware.com/img/Customers/RHHLogo.jpg",
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
                    StoreId = 1, 
                    PictureUrl = "https://images.shopkees.com/uploads/cdn/images/1000/9995354760_1675408536.webp",
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
                    StoreId = 2, 
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
                    StockQuantity = 0, 
                    ProductCategoryId = 2,
                    StoreId = 5, 
                    PictureUrl = "https://media.licdn.com/dms/image/v2/C561BAQE_m2Ujim4lyQ/company-background_10000/company-background_10000/0/1584559760344/american_international_school_kuwait_cover?e=2147483647&v=beta&t=zt2lRU5uJA-o2CHmJuVDqoWV8SIqa_W_F69eqQ3yRhs",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 4,
                    Name = "AUM Tuition Fee",
                    Description = "Annual tuition fee for American University of the Middle East.",
                    Price = 1000.00m,
                    StockQuantity = 100,
                    ProductCategoryId = 2,
                    StoreId = 6, 
                    PictureUrl = "https://www.aum.edu.kw/images/ShareLogo.jpg",
                    IsActive = true
                },
                // Medical Expenses
                new Product
                {
                    ProductId = 5,
                    Name = "MRI",
                    Description = "MRI scan package.",
                    Price = 150.00m,
                    StockQuantity = 0, 
                    ProductCategoryId = 3,
                    StoreId = 7, 
                    PictureUrl = "https://www.capitalradiology.com.au/media/he1jvtno/mri-2000-x-1333-v2.png",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Dental Cleaning",
                    Description = "Professional dental cleaning service.",
                    Price = 80.00m,
                    StockQuantity = 0, 
                    ProductCategoryId = 3,
                    StoreId = 8, 
                    PictureUrl = "https://west85thdental.com/wp-content/uploads/2022/01/woman-getting-a-dental-cleaning.jpg",
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
                    StoreId = 3, 
                    PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9bK65Pig7XvG_6MJ9RcEXWfVijHKjTDmPzg&s",
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
                    StoreId = 4, 
                    PictureUrl = "https://m.media-amazon.com/images/I/51Yt8mJTKzL.jpg",
                    IsActive = true
                }
            );
        }
    }
}