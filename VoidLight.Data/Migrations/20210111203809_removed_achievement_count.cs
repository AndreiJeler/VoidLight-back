using Microsoft.EntityFrameworkCore.Migrations;

namespace VoidLight.Data.Migrations
{
    public partial class removed_achievement_count : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AchievementsAcquired",
                table: "GameUsers");

            migrationBuilder.DropColumn(
                name: "AchievementTotal",
                table: "Games");

            migrationBuilder.AlterColumn<double>(
                name: "TimePlayed",
                table: "GameUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimePlayed",
                table: "GameUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<int>(
                name: "AchievementsAcquired",
                table: "GameUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AchievementTotal",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
