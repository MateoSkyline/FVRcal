using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace FVRcal.Migrations
{
    public partial class Launch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    firstname = table.Column<string>(maxLength: 60, nullable: true),
                    lastname = table.Column<string>(maxLength: 60, nullable: true),
                    username = table.Column<string>(maxLength: 30, nullable: true),
                    usercode = table.Column<string>(maxLength: 6, nullable: true),
                    password = table.Column<string>(maxLength: 128, nullable: true),
                    salt = table.Column<string>(maxLength: 16, nullable: true),
                    permissions = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    storage_id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    time = table.Column<DateTime>(nullable: false),
                    flags = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.storage_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Storage");
        }
    }
}
