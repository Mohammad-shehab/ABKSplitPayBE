using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABKSplitPayBE.Migrations
{
    /// <inheritdoc />
    public partial class picturnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "Description", "PictureUrl" },
                values: new object[] { "MRI scan package.", "https://www.capitalradiology.com.au/media/he1jvtno/mri-2000-x-1333-v2.png" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "Description", "PictureUrl" },
                values: new object[] { "Comprehensive health checkup package.", "https://images.unsplash.com/photo-1576091160397-5d14be92a6ad?q=80&w=2070&auto=format&fit=crop" });
        }
    }
}
