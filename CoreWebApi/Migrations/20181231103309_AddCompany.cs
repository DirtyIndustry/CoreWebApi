using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Name);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CompanyName",
                value: "Alibaba");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CompanyName",
                value: "百度");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CompanyName",
                value: "腾讯");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyName",
                table: "Users",
                column: "CompanyName");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyName",
                table: "Users",
                column: "CompanyName",
                principalTable: "Companies",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyName",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Users");
        }
    }
}
