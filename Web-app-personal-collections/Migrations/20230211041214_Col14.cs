using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_app_personal_collections.Migrations
{
    public partial class Col14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Bool1",
                table: "Items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Bool2",
                table: "Items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Bool3",
                table: "Items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date1",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date2",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date3",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Number1",
                table: "Items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Number2",
                table: "Items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Number3",
                table: "Items",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text1",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text2",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text3",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bool1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Bool2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Bool3",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Date1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Date2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Date3",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Number1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Number2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Number3",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Text1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Text2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Text3",
                table: "Items");
        }
    }
}
