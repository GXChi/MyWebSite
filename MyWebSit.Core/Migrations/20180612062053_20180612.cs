using Microsoft.EntityFrameworkCore.Migrations;

namespace MyWebSit.Core.Migrations
{
    public partial class _20180612 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParenId",
                table: "Menus",
                newName: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Menus",
                newName: "ParenId");
        }
    }
}
