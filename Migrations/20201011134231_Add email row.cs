using Microsoft.EntityFrameworkCore.Migrations;

namespace FVRcal.Migrations
{
    public partial class Addemailrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Accounts",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Accounts");
        }
    }
}
