using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_app_personal_collections.Migrations
{
    public partial class col_16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text3",
                table: "Items",
                newName: "text3");

            migrationBuilder.RenameColumn(
                name: "Text2",
                table: "Items",
                newName: "text2");

            migrationBuilder.RenameColumn(
                name: "Text1",
                table: "Items",
                newName: "text1");

            migrationBuilder.RenameColumn(
                name: "Number3",
                table: "Items",
                newName: "number3");

            migrationBuilder.RenameColumn(
                name: "Number2",
                table: "Items",
                newName: "number2");

            migrationBuilder.RenameColumn(
                name: "Number1",
                table: "Items",
                newName: "number1");

            migrationBuilder.RenameColumn(
                name: "Date3",
                table: "Items",
                newName: "date3");

            migrationBuilder.RenameColumn(
                name: "Date2",
                table: "Items",
                newName: "date2");

            migrationBuilder.RenameColumn(
                name: "Date1",
                table: "Items",
                newName: "date1");

            migrationBuilder.RenameColumn(
                name: "Bool3",
                table: "Items",
                newName: "bool3");

            migrationBuilder.RenameColumn(
                name: "Bool2",
                table: "Items",
                newName: "bool2");

            migrationBuilder.RenameColumn(
                name: "Bool1",
                table: "Items",
                newName: "bool1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "text3",
                table: "Items",
                newName: "Text3");

            migrationBuilder.RenameColumn(
                name: "text2",
                table: "Items",
                newName: "Text2");

            migrationBuilder.RenameColumn(
                name: "text1",
                table: "Items",
                newName: "Text1");

            migrationBuilder.RenameColumn(
                name: "number3",
                table: "Items",
                newName: "Number3");

            migrationBuilder.RenameColumn(
                name: "number2",
                table: "Items",
                newName: "Number2");

            migrationBuilder.RenameColumn(
                name: "number1",
                table: "Items",
                newName: "Number1");

            migrationBuilder.RenameColumn(
                name: "date3",
                table: "Items",
                newName: "Date3");

            migrationBuilder.RenameColumn(
                name: "date2",
                table: "Items",
                newName: "Date2");

            migrationBuilder.RenameColumn(
                name: "date1",
                table: "Items",
                newName: "Date1");

            migrationBuilder.RenameColumn(
                name: "bool3",
                table: "Items",
                newName: "Bool3");

            migrationBuilder.RenameColumn(
                name: "bool2",
                table: "Items",
                newName: "Bool2");

            migrationBuilder.RenameColumn(
                name: "bool1",
                table: "Items",
                newName: "Bool1");
        }
    }
}
