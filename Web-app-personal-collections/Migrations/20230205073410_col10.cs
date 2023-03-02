using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_app_personal_collections.Migrations
{
    public partial class col10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Likes_CollectionId",
                table: "Likes",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Collections_CollectionId",
                table: "Likes",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Collections_CollectionId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_CollectionId",
                table: "Likes");
        }
    }
}
