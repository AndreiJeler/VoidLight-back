using Microsoft.EntityFrameworkCore.Migrations;

namespace VoidLight.Data.Migrations
{
    public partial class changedthePKforthegameAchievement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameAchievements",
                table: "GameAchievements");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "GameAchievements",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameAchievements",
                table: "GameAchievements",
                columns: new[] { "GameId", "UserId", "Description" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameAchievements",
                table: "GameAchievements");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "GameAchievements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameAchievements",
                table: "GameAchievements",
                columns: new[] { "GameId", "UserId" });
        }
    }
}
