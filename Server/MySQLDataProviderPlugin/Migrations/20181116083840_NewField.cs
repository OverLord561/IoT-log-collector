using Microsoft.EntityFrameworkCore.Migrations;

namespace MySQLDataProviderPlugin.Migrations
{
    public partial class NewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "testField",
                table: "GeneralDevices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testField",
                table: "GeneralDevices");
        }
    }
}
