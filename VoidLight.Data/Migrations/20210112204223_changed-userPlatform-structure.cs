using Microsoft.EntityFrameworkCore.Migrations;

namespace VoidLight.Data.Migrations
{
    public partial class changeduserPlatformstructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobby_Games_GameId",
                table: "Lobby");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLobby_Lobby_LobbyId",
                table: "UserLobby");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLobby_Users_UserId",
                table: "UserLobby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLobby",
                table: "UserLobby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lobby",
                table: "Lobby");

            migrationBuilder.RenameTable(
                name: "UserLobby",
                newName: "UserLobbies");

            migrationBuilder.RenameTable(
                name: "Lobby",
                newName: "Lobbies");

            migrationBuilder.RenameIndex(
                name: "IX_UserLobby_LobbyId",
                table: "UserLobbies",
                newName: "IX_UserLobbies_LobbyId");

            migrationBuilder.RenameIndex(
                name: "IX_Lobby_GameId",
                table: "Lobbies",
                newName: "IX_Lobbies_GameId");

            migrationBuilder.AddColumn<string>(
                name: "KnownAs",
                table: "UserPlatforms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginId",
                table: "UserPlatforms",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLobbies",
                table: "UserLobbies",
                columns: new[] { "UserId", "LobbyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lobbies",
                table: "Lobbies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Games_GameId",
                table: "Lobbies",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLobbies_Lobbies_LobbyId",
                table: "UserLobbies",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLobbies_Users_UserId",
                table: "UserLobbies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Games_GameId",
                table: "Lobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLobbies_Lobbies_LobbyId",
                table: "UserLobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLobbies_Users_UserId",
                table: "UserLobbies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLobbies",
                table: "UserLobbies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lobbies",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "KnownAs",
                table: "UserPlatforms");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "UserPlatforms");

            migrationBuilder.RenameTable(
                name: "UserLobbies",
                newName: "UserLobby");

            migrationBuilder.RenameTable(
                name: "Lobbies",
                newName: "Lobby");

            migrationBuilder.RenameIndex(
                name: "IX_UserLobbies_LobbyId",
                table: "UserLobby",
                newName: "IX_UserLobby_LobbyId");

            migrationBuilder.RenameIndex(
                name: "IX_Lobbies_GameId",
                table: "Lobby",
                newName: "IX_Lobby_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLobby",
                table: "UserLobby",
                columns: new[] { "UserId", "LobbyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lobby",
                table: "Lobby",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobby_Games_GameId",
                table: "Lobby",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLobby_Lobby_LobbyId",
                table: "UserLobby",
                column: "LobbyId",
                principalTable: "Lobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLobby_Users_UserId",
                table: "UserLobby",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
