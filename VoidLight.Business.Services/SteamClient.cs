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
        private readonly ISteamGameCollection _gameCollection;


        public SteamClient(IOptions<AppSettings> appSettings, ISteamGameCollection gameCollection)
        {
            _appSettings = appSettings.Value;
            _client = new HttpClient();
            _gameCollection = gameCollection;
        }

        public async Task<JEnumerable<JToken>> GetGameAchievements(string appId)
        {
            var url = $"{Constants.STEAM_GAME_SCHEME_URL}/?key={_appSettings.SteamKey}&appid={appId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            var achievements = jsonData.SelectToken("game.availableGameStats.achievements").Children();
            return achievements;
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
                return gameName != null ? gameName : Constants.STEAM_NO_GAME_PLAYING;
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

            var achievementsList = new List<GameAchievement>();

            var url = $"{Constants.STEAM_PLAYER_ACHIEVEMENTS_URL}/?appid={appId}&key={_appSettings.SteamKey}&steamid={steamId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            var achievements = jsonData.SelectToken("playerstats.achievements").Children().Where(a=>a.SelectToken("achieved").Value<int>()==1);
            foreach(var achievement in achievements)
            {
                var gameAchievement = gameAchievements.FirstOrDefault(ga => ga.SelectToken("name").Value<string>() == achievement.SelectToken("apiname").Value<string>());
                var achievementName = gameAchievement.SelectToken("displayName").Value<String>();
                var achievementIcon = gameAchievement.SelectToken("icon").Value<String>();
                var unlock = achievement.SelectToken("unlocktime").Value<int>();
                var dateUnlock = new DateTime(unlock, DateTimeKind.Utc);
                var newAchievement = new GameAchievement() {
                    Description = achievementName,
                    User = user,
                    Game=game,
                    UserId=user.Id,
                    GameId=game.Id,
                    TimeAchieved=dateUnlock,
                    Icon=achievementIcon
                };

                achievementsList.Add(newAchievement);


                //creearea achievement-ului dintre joc si user
            }

            return achievementsList;
        }

        public async Task<IEnumerable<Game>> GetUserGames(string steamId, User user, Platform platform)
        {
            var url = $"{Constants.STEAM_OWNED_GAMES_URL}/?key={_appSettings.SteamKey}&steamid={steamId}&format=json";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            
            var games = jsonData.SelectToken(Constants.STEAM_OWNED_GAMES_LIST_TOKEN).Children();

            var newGames = new List<Game>();

            foreach (var game in games)
            {
                var appId = game[Constants.STEAM_APP_ID].Value<string>();

                var gameNameToken = await _gameCollection.GetGameName(appId);

                if (gameNameToken is null)
                {
                    continue;
                }

                var gameName = gameNameToken[Constants.STEAM_NAME_TOKEN].Value<string>();

                var newGame = new Game()
                {
                    Name = gameName,
                    Description = gameName
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
                        User = user
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
