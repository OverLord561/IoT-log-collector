using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSSQLDataProviderPlugin.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateStamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralDevices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralDevices");
        }
    }
}
