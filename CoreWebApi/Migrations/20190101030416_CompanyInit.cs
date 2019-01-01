using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class CompanyInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    Department = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_UserName", x => x.UserName);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Department", "Position", "UserName" },
                values: new object[] { 1, "headquarter", "CEO", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Department", "Position", "UserName" },
                values: new object[] { 2, "headquarter", "Boss", "张三" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Department", "Position", "UserName" },
                values: new object[] { 3, "headquarter", "Administrator", "李四" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
