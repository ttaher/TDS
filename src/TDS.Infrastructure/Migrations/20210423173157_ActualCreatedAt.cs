using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace TDS.Infrastructure.Migrations
{
    public partial class ActualCreatedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualCreatedAt",
                table: "Payments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCreatedAt",
                table: "Payments");
        }
    }
}
