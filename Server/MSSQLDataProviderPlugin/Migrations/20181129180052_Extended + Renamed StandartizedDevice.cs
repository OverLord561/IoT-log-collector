using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSSQLDataProviderPlugin.Migrations
{
    public partial class ExtendedRenamedStandartizedDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralDevices");

            migrationBuilder.CreateTable(
                name: "DeviceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeviceGuid = table.Column<string>(nullable: true),
                    DateStamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceLogs");

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
    }
}
