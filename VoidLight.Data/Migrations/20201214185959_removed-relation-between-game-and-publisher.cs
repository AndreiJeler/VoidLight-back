using Microsoft.EntityFrameworkCore.Migrations;

namespace VoidLight.Data.Migrations
{
    public partial class removedrelationbetweengameandpublisher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GamePublishers_GamePublisherId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GamePublisherId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GamePublisherId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GamePublisherId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_GamePublisherId",
                table: "Games",
                column: "GamePublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GamePublishers_GamePublisherId",
                table: "Games",
                column: "GamePublisherId",
                principalTable: "GamePublishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
