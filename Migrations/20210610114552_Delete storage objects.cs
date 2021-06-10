using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace FVRcal.Migrations
{
    public partial class Deletestorageobjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropTable(
                name: "Storage_Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    Storage_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Flags = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.Storage_id);
                });

            migrationBuilder.CreateTable(
                name: "Storage_Type",
                columns: table => new
                {
                    St_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage_Type", x => x.St_Type_Id);
                });
        }
    }
}
