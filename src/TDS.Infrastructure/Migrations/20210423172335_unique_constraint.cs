using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Infrastructure.Migrations
{
    public partial class unique_constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentId",
                table: "Payments",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Address",
                table: "Accounts",
                column: "Address",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Address",
                table: "Accounts");
        }
    }
}
