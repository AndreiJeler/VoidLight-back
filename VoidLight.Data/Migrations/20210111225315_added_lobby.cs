using Microsoft.EntityFrameworkCore.Migrations;

namespace VoidLight.Data.Migrations
{
    public partial class added_lobby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lobby",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: false),
                    HasStarted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lobby_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLobby",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LobbyId = table.Column<int>(nullable: false),
                    IsInitializer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLobby", x => new { x.UserId, x.LobbyId });
                    table.ForeignKey(
                        name: "FK_UserLobby_Lobby_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobby",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLobby_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lobby_GameId",
                table: "Lobby",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLobby_LobbyId",
                table: "UserLobby",
                column: "LobbyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLobby");

            migrationBuilder.DropTable(
                name: "Lobby");
        }
    }
}
