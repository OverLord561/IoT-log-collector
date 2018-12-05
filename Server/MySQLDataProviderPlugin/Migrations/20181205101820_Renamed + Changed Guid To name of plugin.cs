using Microsoft.EntityFrameworkCore.Migrations;

namespace MySQLDataProviderPlugin.Migrations
{
    public partial class RenamedChangedGuidTonameofplugin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeviceGuid",
                table: "DeviceLogs",
                newName: "PluginName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PluginName",
                table: "DeviceLogs",
                newName: "DeviceGuid");
        }
    }
}
