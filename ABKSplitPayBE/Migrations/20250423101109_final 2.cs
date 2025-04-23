using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ABKSplitPayBE.Migrations
{
    /// <inheritdoc />
    public partial class final2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Description", "IsActive", "Name", "PictureUrl", "Price", "ProductCategoryId", "StockQuantity", "StoreId" },
                values: new object[,]
                {
                    { 1, "Latest Samsung smartphone with advanced features.", true, "Samsung Galaxy S23", "https://images.shopkees.com/uploads/cdn/images/1000/9995354760_1675408536.webp", 250.00m, 1, 50, 1 },
                    { 2, "High-performance laptop for professionals.", true, "MacBook Pro", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mbp-spacegray-select-202206?wid=904&hei=840&fmt=jpeg&qlt=90&.v=1664497359481", 1200.00m, 1, 30, 2 },
                    { 7, "Modern 3-seater sofa set.", true, "Sofa Set", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9bK65Pig7XvG_6MJ9RcEXWfVijHKjTDmPzg&s", 400.00m, 4, 20, 3 },
                    { 8, "Elegant 6-seater dining table.", true, "Dining Table", "https://m.media-amazon.com/images/I/51Yt8mJTKzL.jpg", 300.00m, 4, 15, 4 }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "StoreId", "Description", "IsActive", "LogoUrl", "Name", "StoreCategoryId", "WebsiteUrl" },
                values: new object[,]
                {
                    { 5, "A premier international school in Kuwait.", true, "https://media.licdn.com/dms/image/v2/C561BAQE_m2Ujim4lyQ/company-background_10000/company-background_10000/0/1584559760344/american_international_school_kuwait_cover?e=2147483647&v=beta&t=zt2lRU5uJA-o2CHmJuVDqoWV8SIqa_W_F69eqQ3yRhs", "American International School", 3, "https://www.ais-kuwait.org" },
                    { 6, "AUM Top university in Kuwait in QS & Times Higher Education Rankings", true, "https://www.aum.edu.kw/images/ShareLogo.jpg", "AUM", 3, "https://www.aum.edu.kw/" },
                    { 7, "A leading hospital in Kuwait offering comprehensive medical services.", true, "https://purchase.daralshifa.com/images/logo.png", "Dar Al Shifa Hospital", 4, "https://www.daralshifa.com" },
                    { 8, "A premium hospital specializing in various medical treatments.", true, "https://www.sharpersoftware.com/img/Customers/RHHLogo.jpg", "Royale Hayat Hospital", 4, "https://www.royalehayat.com" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Description", "IsActive", "Name", "PictureUrl", "Price", "ProductCategoryId", "StockQuantity", "StoreId" },
                values: new object[,]
                {
                    { 3, "Annual tuition fee for American International School.", true, "AIS Tuition Fee", "https://media.licdn.com/dms/image/v2/C561BAQE_m2Ujim4lyQ/company-background_10000/company-background_10000/0/1584559760344/american_international_school_kuwait_cover?e=2147483647&v=beta&t=zt2lRU5uJA-o2CHmJuVDqoWV8SIqa_W_F69eqQ3yRhs", 5000.00m, 2, 0, 5 },
                    { 4, "Annual tuition fee for American University of the Middle East.", true, "AUM Tuition Fee", "https://www.aum.edu.kw/images/ShareLogo.jpg", 50.00m, 2, 100, 6 },
                    { 5, "MRI scan package.", true, "MRI", "https://www.capitalradiology.com.au/media/he1jvtno/mri-2000-x-1333-v2.png", 150.00m, 3, 0, 7 },
                    { 6, "Professional dental cleaning service.", true, "Dental Cleaning", "https://west85thdental.com/wp-content/uploads/2022/01/woman-getting-a-dental-cleaning.jpg", 80.00m, 3, 0, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 8);
        }
    }
}
