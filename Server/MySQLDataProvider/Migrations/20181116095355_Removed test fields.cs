using Microsoft.EntityFrameworkCore.Migrations;

namespace MySQLDataProviderPlugin.Migrations
{
    public partial class Removedtestfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceIdentifier",
                table: "GeneralDevices");

            migrationBuilder.DropColumn(
                name: "OneMoreField",
                table: "GeneralDevices");

            migrationBuilder.DropColumn(
                name: "testField2",
                table: "GeneralDevices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceIdentifier",
                table: "GeneralDevices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OneMoreField",
                table: "GeneralDevices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "testField2",
                table: "GeneralDevices",
                nullable: true);
        }
    }
}
