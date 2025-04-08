using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABKSplitPayBE.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserpicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "PictureUrl",
                value: "https://images.shopkees.com/uploads/cdn/images/1000/9995354760_1675408536.webp");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "PictureUrl",
                value: "https://www.ais-kuwait.org/admissions/tuition-fees/");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Description", "Name", "PictureUrl" },
                values: new object[] { "Annual tuition fee for American University of the Middle East.", "AUM Tuition Fee", "https://www.aum.edu.kw/english/admission/undergraduate-admission/tuition-fees" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Name",
                value: "MRI");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 1,
                column: "PictureUrl",
                value: "https://i0.wp.com/zilani-int.com/wp-content/uploads/2023/02/8ebb2cb57bdb71ac4f0aaadfd61911d5.jpeg?w=680&ssl=1");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 2,
                column: "PictureUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/IKEA_Shopping_centre_Sweden.jpg/1200px-IKEA_Shopping_centre_Sweden.jpg");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 3,
                column: "PictureUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQiCK3dZsrWBzP6I0HBpEjA-1-AqZoEeF1drQ&s");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 4,
                column: "PictureUrl",
                value: "https://previews.123rf.com/images/shutterboythailand/shutterboythailand1604/shutterboythailand160400336/58429893-medical-service-word-on-tablet-screen-with-medical-equipment-on-background.jpg");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 1,
                column: "LogoUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT_EYfEMiV59HZO1JVMunmUd-XHOpbVrWI8DQ&s");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 2,
                column: "LogoUrl",
                value: "https://www.xcite.com/assets/icons/logo.jpg");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 3,
                column: "LogoUrl",
                value: "https://static.dezeen.com/uploads/2019/04/ikea-logo-new-hero-1.jpg");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 4,
                columns: new[] { "LogoUrl", "Name", "WebsiteUrl" },
                values: new object[] { "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQBNTvYAyzRiucO7d_DLudPP-8B2qwdQOOH4Q&s", "Abyat", "https://www.abyat.com/kw/ar" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 5,
                column: "LogoUrl",
                value: "https://media.licdn.com/dms/image/v2/C561BAQE_m2Ujim4lyQ/company-background_10000/company-background_10000/0/1584559760344/american_international_school_kuwait_cover?e=2147483647&v=beta&t=zt2lRU5uJA-o2CHmJuVDqoWV8SIqa_W_F69eqQ3yRhs");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 6,
                columns: new[] { "Description", "LogoUrl", "Name", "WebsiteUrl" },
                values: new object[] { "AUM Top university in Kuwait in QS & Times Higher Education Rankings", "https://www.aum.edu.kw/images/ShareLogo.jpg", "AUM", "https://www.aum.edu.kw/" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 7,
                column: "LogoUrl",
                value: "https://purchase.daralshifa.com/images/logo.png");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 8,
                column: "LogoUrl",
                value: "https://www.sharpersoftware.com/img/Customers/RHHLogo.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "PictureUrl",
                value: "https://images.samsung.com/is/image/samsung/p6pim/ae/2302/gallery/ae-galaxy-s23-s918-sm-s918bzkhmeb-534862463?$650_519_PNG$");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "PictureUrl",
                value: "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Description", "Name", "PictureUrl" },
                values: new object[] { "Standard school uniform set.", "School Uniform", "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Name",
                value: "General Checkup");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 1,
                column: "PictureUrl",
                value: "https://images.unsplash.com/photo-1550005799-34c8c3d9c1b6?q=80&w=2070&auto=format&fit=crop");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 2,
                column: "PictureUrl",
                value: "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 3,
                column: "PictureUrl",
                value: "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop");

            migrationBuilder.UpdateData(
                table: "StoreCategories",
                keyColumn: "StoreCategoryId",
                keyValue: 4,
                column: "PictureUrl",
                value: "https://images.unsplash.com/photo-1576091160397-5d14be92a6ad?q=80&w=2070&auto=format&fit=crop");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 1,
                column: "LogoUrl",
                value: "https://www.jarir.com/static/jarir-logo.png");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 2,
                column: "LogoUrl",
                value: "https://www.xcite.com/static/xcite-logo.png");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 3,
                column: "LogoUrl",
                value: "https://www.ikea.com/global/en/images/ikea-logo.svg");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 4,
                columns: new[] { "LogoUrl", "Name", "WebsiteUrl" },
                values: new object[] { "https://www.theone.com/static/theone-logo.png", "The One", "https://www.theone.com" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 5,
                column: "LogoUrl",
                value: "https://www.ais-kuwait.org/static/ais-logo.png");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 6,
                columns: new[] { "Description", "LogoUrl", "Name", "WebsiteUrl" },
                values: new object[] { "A bookstore offering educational materials and school supplies.", "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?q=80&w=2070&auto=format&fit=crop", "Al-Ru’ya Bookstore", "https://www.alruya.com" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 7,
                column: "LogoUrl",
                value: "https://www.daralshifa.com/static/daralshifa-logo.png");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 8,
                column: "LogoUrl",
                value: "https://www.royalehayat.com/static/royalehayat-logo.png");
        }
    }
}
