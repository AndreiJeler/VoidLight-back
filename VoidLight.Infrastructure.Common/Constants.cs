using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Infrastructure.Common
{
    public static class Constants
    {
        #region URL
        public const string APP_URL = "https://localhost:44324/";
        public const string CLIENT_URL = "http://localhost:4200/";

        #endregion

        #region HttpStatus
        public const string HTTP_CREATED = "created";
        public const string HTTP_UPDATED = "updated";
        #endregion HttpStatus

        #region Images
        public const string DEFAULT_IMAGE_USER = "Images\\Default\\defaultUserPicture.gif";

        #endregion


        #region Exceptions
        public const string AUTHENTICATION_EXCEPTION = "Authentication failed";
        public const string SEND_EMAIL_EXCEPTION = "Can't send the email";
        public const string INVALID_PARAMETER_EXCEPTION = "Invalid Parameter";
        public const string UNAUTHORIZED_EXCEPTION = "Unauthorized access";
        public const string ACCOUNT_ALREADY_CONFIRMED_EXCEPTION = "Account already confirmed";

        #endregion


        #region Email
        public const string ACCOUNT_ACTIVATION_SUBJECT = "Account activation";
        public const string ACCOUNT_ACTIVATION_BODY = "Click on the bellow link for activate your account \n";
        public const string ACCOUNT_ACTIVATION_LINK = CLIENT_URL + "account-activation?token=";
        public const string SMTP_CLIENT = "smtp.gmail.com";
        public const int SMTP_PORT = 587;
        public const string TEMPORARY_PASSWORD_MESSAGE = "There is your temporary pessword. You MUST change it in your Profile section after login. ";
        public const string RESET_PASSWORD_MESSAGE = "Your password has been changed";
        public const string RESET_PASSWORD_SUBJECT = "Reset password";
        #endregion Email

        #region User
        public const string PASSWORD_TEMPLATE = "000000";
        public const string AUTHORIZATION_HEADER = "Authorization";
        public const string AUTHORIZATION_ITEM = "User";
        public const string AUTHORIZATION_ID = "id";
        public const string UNAUTHORIZED_MESSAGE = "Unauthorized";

        #endregion

        #region Steam
        public const string STEAM_ALL_GAMES_URL = "http://api.steampowered.com/ISteamApps/GetAppList/v0002/";
        public const string STEAM_ALL_GAMES_APP_LIST = "applist.apps";
        public const string STEAM_APP_ID = "appid";
        public const string STEAM_GAME_SCHEME_URL = "http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2";
        public const string STEAM_GAMENAME_TOKEN = "game.gameName";
        public const string STEAM_USER_INFO_URL = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002";
        public const string STEAM_OWNED_GAMES_URL = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001";
        public const string STEAM_OWNED_GAMES_LIST_TOKEN= "response.games";
        public const string STEAM_NAME_TOKEN = "name";
        public const string STEAM_USER_LIST_TOKEN = "response.players";
        public const string STEAM_GAMEID = "gameid";
        public const string STEAM_GAME_EXTRA_NAME = "gameextrainfo";
        public const string STEAM_NO_GAME_PLAYING = "None";
        public const string STEAM_PLAYER_ACHIEVEMENTS_URL = "http://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v0001";
        public const string STEAM_GAME_SCHEMA_ACHIEVEMENTS = "game.availableGameStats.achievements";
        public const string STEAM_USER_ACHIEVEMENTS = "playerstats.achievements";
        public const string STEAM_ACHIEVEMENT_ACHIEVED = "achieved";
        public const string STEAM_ACHIEVEMENT_NAME = "name";
        public const string STEAM_ACHIEVEMENT_APINAME = "apiname";
        public const string STEAM_ACHIEVEMENT_DISPLAYNAME = "displayname";
        public const string STEAM_ACHIEVEMENT_ICON = "icon";
        public const string STEAM_ACHIEVEMENT_UNLOCK_TIME = "unlocktime";





        #endregion

        #region Discord
        public const string DISCORD_OAUTH_TOKEN_URL = "https://discord.com/api/v6/oauth2/token";
        public const string DISCORD_SCOPES = "identify email guilds guilds.join";
        public const string DISCORD_REDIRECT_URI = "http://localhost:4200/discord-return";
        public const string DISCORD_USER_TOKEN_URL = "https://discord.com/api/v6/users/@me";



        #endregion
    }
}
