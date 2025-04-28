using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABKSplitPayBE.Migrations
{
    /// <inheritdoc />
    public partial class GCHSGSCGS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 1,
                columns: new[] { "IntervalDays", "Name" },
                values: new object[] { 0, "Pay Full" });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 2,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "1 Month", 1 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 3,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "2 Month", 2 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 4,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "3 Month", 3 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 5,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "4 Month", 4 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 6,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "5 Month", 5 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 7,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "6 Month", 6 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 8,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "7 Month", 7 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 9,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "8 Month", 8 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 10,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "9 Month", 9 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 11,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "10 Month", 10 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 12,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "11 Month", 11 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 1,
                columns: new[] { "IntervalDays", "Name" },
                values: new object[] { 30, "1 Month Plan" });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 2,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "2 Month Plan", 2 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 3,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "3 Month Plan", 3 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 4,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "4 Month Plan", 4 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 5,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "5 Month Plan", 5 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 6,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "6 Month Plan", 6 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 7,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "7 Month Plan", 7 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 8,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "8 Month Plan", 8 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 9,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "9 Month Plan", 9 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 10,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "10 Month Plan", 10 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 11,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "11 Month Plan", 11 });

            migrationBuilder.UpdateData(
                table: "PaymentPlans",
                keyColumn: "PaymentPlanId",
                keyValue: 12,
                columns: new[] { "Name", "NumberOfInstallments" },
                values: new object[] { "12 Month Plan", 12 });
        }
    }
}
