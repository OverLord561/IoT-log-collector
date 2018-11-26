using Microsoft.EntityFrameworkCore.Migrations;

namespace MySQLDataProviderPlugin.Migrations
{
    public partial class NewFieldasstring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testField",
                table: "GeneralDevices");

            migrationBuilder.AddColumn<string>(
                name: "testField2",
                table: "GeneralDevices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testField2",
                table: "GeneralDevices");

            migrationBuilder.AddColumn<int>(
                name: "testField",
                table: "GeneralDevices",
                nullable: false,
                defaultValue: 0);
        }
    }
}
