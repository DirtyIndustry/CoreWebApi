using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations.Entrance
{
    public partial class ChangeRoleToType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Logins",
                newName: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Logins",
                newName: "Role");
        }
    }
}
