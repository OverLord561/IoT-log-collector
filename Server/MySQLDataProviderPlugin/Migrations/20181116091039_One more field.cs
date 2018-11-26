using Microsoft.EntityFrameworkCore.Migrations;

namespace MySQLDataProviderPlugin.Migrations
{
    public partial class Onemorefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OneMoreField",
                table: "GeneralDevices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OneMoreField",
                table: "GeneralDevices");
        }
    }
}
