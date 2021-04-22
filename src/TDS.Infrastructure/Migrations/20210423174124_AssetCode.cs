using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Infrastructure.Migrations
{
    public partial class AssetCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetCode",
                table: "Payments",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetCode",
                table: "Payments");
        }
    }
}
