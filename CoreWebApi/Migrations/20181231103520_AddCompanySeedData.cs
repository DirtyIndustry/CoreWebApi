using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddCompanySeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                column: "Name",
                value: "Alibaba");

            migrationBuilder.InsertData(
                table: "Companies",
                column: "Name",
                value: "百度");

            migrationBuilder.InsertData(
                table: "Companies",
                column: "Name",
                value: "腾讯");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Name",
                keyValue: "Alibaba");

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Name",
                keyValue: "百度");

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Name",
                keyValue: "腾讯");
        }
    }
}
