using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABKSplitPayBE.Migrations
{
    /// <inheritdoc />
    public partial class updateAbyat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 4,
                column: "LogoUrl",
                value: "https://d1yjjnpx0p53s8.cloudfront.net/styles/large/s3/abyat_wing_logo_0.jpg?itok=Trhx2GSu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "StoreId",
                keyValue: 4,
                column: "LogoUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQBNTvYAyzRiucO7d_DLudPP-8B2qwdQOOH4Q&s");
        }
    }
}
