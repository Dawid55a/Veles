using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VelesAPI.Migrations
{
    public partial class addRolesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "UserGroups",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserGroups");
        }
    }
}
