using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;

namespace VoidLight.Business.Services
{
    public class SteamClient : ISteamClient
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _client;


        public SteamClient(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _client = new HttpClient();
        }

        public async Task<JEnumerable<JToken>> GetGameAchievements(string appId)
        {
            var url = $"{Constants.STEAM_GAME_SCHEME_URL}/?key={_appSettings.SteamKey}&appid={appId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            try
            {
                var achievements = jsonData.SelectToken(Constants.STEAM_GAME_SCHEMA_ACHIEVEMENTS).Children();
                return achievements;
            }
            catch
            {
                return new JEnumerable<JToken>();
            }
        }

        public async Task<Game> GetGameDetails(string appId)
        {
            var url = $"{Constants.STEAM_GAME_SCHEME_URL}/?key={_appSettings.SteamKey}&appid={appId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            try
            {
                var gameName = jsonData.SelectToken(Constants.STEAM_GAMENAME_TOKEN).Value<string>();
                var game = new Game()
                {
                    Name = gameName,
                    Description = gameName
                };
                return game;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> GetUnlockedAchievementsNumber(string steamId, string appId)
        {

            var url = $"{Constants.STEAM_PLAYER_ACHIEVEMENTS_URL}/?appid={appId}&key={_appSettings.SteamKey}&steamid={steamId}";
            var response = "";
            try
            {
                response = await _client.GetStringAsync(url);
            }
            catch
            {
                return 0;
            }
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            try
            {
                var achievements = jsonData.SelectToken(Constants.STEAM_USER_ACHIEVEMENTS).Children().Where(a => a.SelectToken(Constants.STEAM_ACHIEVEMENT_ACHIEVED).Value<int>() == 1);

                return achievements.Count();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<string> GetUserCurrentPlayingGame(string steamId)
        {
            var url = $"{Constants.STEAM_USER_INFO_URL}/?key={_appSettings.SteamKey}&steamids={steamId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            var player = jsonData.SelectToken(Constants.STEAM_USER_LIST_TOKEN).First;


            try
            {
                var gameId = player[Constants.STEAM_GAMEID].Value<string>();
                var gameName = player[Constants.STEAM_GAME_EXTRA_NAME].Value<string>();
                return gameName ?? Constants.STEAM_NO_GAME_PLAYING;
                // momentan se trimite numai numele jocului spre front-end
            }
            catch
            {
                return Constants.STEAM_NO_GAME_PLAYING;
            }
        }

        public async Task<IList<GameAchievement>> GetUserGameAchievements(string steamId, string appId, User user, Game game)
        {
            var gameAchievements = await GetGameAchievements(appId);

            if (gameAchievements.Count() == 0)
            {
                throw new Exception("The game has no achievements");
            }

            var achievementsList = new List<GameAchievement>();
            

            var url = $"{Constants.STEAM_PLAYER_ACHIEVEMENTS_URL}/?appid={appId}&key={_appSettings.SteamKey}&steamid={steamId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            var achievements = jsonData.SelectToken(Constants.STEAM_USER_ACHIEVEMENTS).Children().Where(a => a.SelectToken(Constants.STEAM_ACHIEVEMENT_ACHIEVED).Value<int>() == 1);
            foreach (var achievement in achievements)
            {
                var gameAchievement = gameAchievements.FirstOrDefault(ga => ga.SelectToken(Constants.STEAM_ACHIEVEMENT_NAME).Value<string>() == achievement.SelectToken(Constants.STEAM_ACHIEVEMENT_APINAME).Value<string>());
                var achievementName = gameAchievement.SelectToken(Constants.STEAM_ACHIEVEMENT_DISPLAYNAME).Value<String>();
                var achievementIcon = gameAchievement.SelectToken(Constants.STEAM_ACHIEVEMENT_ICON).Value<String>();
                var unlock = achievement.SelectToken(Constants.STEAM_ACHIEVEMENT_UNLOCK_TIME).Value<int>();
                var dateUnlock = new DateTime(unlock, DateTimeKind.Utc);
                var newAchievement = new GameAchievement()
                {
                    Description = achievementName,
                    User = user,
                    Game = game,
                    UserId = user.Id,
                    GameId = game.Id,
                    TimeAchieved = dateUnlock,
                    Icon = achievementIcon
                };

                achievementsList.Add(newAchievement);


                //creearea achievement-ului dintre joc si user
            }

            return achievementsList;
        }

        public async Task<IEnumerable<Game>> GetUserGames(string steamId, User user, Platform platform)
        {
            var url = $"{Constants.STEAM_OWNED_GAMES_URL}/?key={_appSettings.SteamKey}&steamid={steamId}&format=json&include_appinfo=true";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);

            var games = jsonData.SelectToken(Constants.STEAM_OWNED_GAMES_LIST_TOKEN).Children();

            var newGames = new List<Game>();

            foreach (var game in games)
            {
                var appId = game[Constants.STEAM_APP_ID].Value<string>();

                //var gameNameToken = await _gameCollection.GetGameName(appId);
                var gameName = game.SelectToken("name").Value<string>();
                var gameIcon = game.SelectToken("img_logo_url").Value<string>();
                var iconUrl = $"{Constants.STEAM_GAME_ICON_URL}/{appId}/{gameIcon}.jpg";
                var timePlayed = game.SelectToken("playtime_forever").Value<int>();
                var hoursPlayed = (double)timePlayed / 60;

                //  var nrAchievements = (await GetGameAchievements(appId)).Count();

                /*                if (nrAchievements == 0)
                                {
                                    continue;
                                }*/
                //var nrAchievementsAcq = await GetUnlockedAchievementsNumber(steamId, appId);



                /* if (gameNameToken is null)
                 {
                     continue;
                 }*/

                //var gameName = gameNameToken[Constants.STEAM_NAME_TOKEN].Value<string>();

                var newGame = new Game()
                {
                    Name = gameName,
                    Description = gameName,
                    Icon = iconUrl
                    //  AchievementTotal = nrAchievements
                };



                if (newGame is null)
                {
                    continue;
                }

                newGame.GameUsers = new List<GameUser>
                {
                    new GameUser()
                    {
                        Game = newGame,
                        User = user,
                        UserId=user.Id,
                        TimePlayed =(int) hoursPlayed
                        //AchievementsAcquired = nrAchievementsAcq
                    }
                };

                newGame.GamePlatforms = new List<GamePlatform>
                {
                    new GamePlatform()
                    {
                        AppId = appId,
                        Game = newGame,
                        Platform = platform,
                        PlatformId = platform.Id
                    }
                };

                newGames.Add(newGame);
            }

            return newGames;
        }

        public async Task GetUserInfo(string steamId)
        {
            var url = $"{Constants.STEAM_USER_INFO_URL}/?key={_appSettings.SteamKey}&steamids={steamId}";
            var response = await _client.GetStringAsync(url);
        }


    }
}
