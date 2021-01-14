using Microsoft.EntityFrameworkCore.Migrations;

namespace VoidLight.Data.Migrations
{
    public partial class added_totaltime_achievementTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AchievementsAcquired",
                table: "GameUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimePlayed",
                table: "GameUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AchievementTotal",
                table: "Games",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AchievementsAcquired",
                table: "GameUsers");

            migrationBuilder.DropColumn(
                name: "TimePlayed",
                table: "GameUsers");

            migrationBuilder.DropColumn(
                name: "AchievementTotal",
                table: "Games");
        }
    }
}
