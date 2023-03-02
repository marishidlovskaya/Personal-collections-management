using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_app_personal_collections.Migrations
{
    public partial class col16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_collectionConfiqs_CollectionId",
                table: "collectionConfiqs",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_collectionConfiqs_Collections_CollectionId",
                table: "collectionConfiqs",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_collectionConfiqs_Collections_CollectionId",
                table: "collectionConfiqs");

            migrationBuilder.DropIndex(
                name: "IX_collectionConfiqs_CollectionId",
                table: "collectionConfiqs");
        }
    }
}
