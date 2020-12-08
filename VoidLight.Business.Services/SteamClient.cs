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

        public async Task<Game> GetGameDetails(string appId)
        {
            var url = $"http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?key={_appSettings.SteamKey}&appid={appId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            try
            {
                var gameName = jsonData.SelectToken("game.gameName").Value<string>();
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
            var url = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_appSettings.SteamKey}&steamids={steamId}";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            var player = jsonData.SelectToken("response.players").First;


            try
            {
                var gameId = player["gameid"].Value<string>();
                var gameName = player["gameextrainfo"].Value<string>();
                return gameName != null ? gameName : "None";
            }
            catch
            {
                return "None";
            }
        }

        public async Task<IEnumerable<Game>> GetUserGames(string steamId, User user, Platform platform)
        {
            var url = $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_appSettings.SteamKey}&steamid={steamId}&format=json";
            var response = await _client.GetStringAsync(url);
            var jsonData = (JObject)JsonConvert.DeserializeObject(response);
            
            var games = jsonData.SelectToken("response.games").Children();

            var newGames = new List<Game>();

            foreach (var game in games)
            {
                var appId = game["appid"].Value<string>();

                var gameNameToken = await _gameCollection.GetGameName(appId);

                if (gameNameToken is null)
                {
                    continue;
                }

                var gameName = gameNameToken["name"].Value<string>();

                var newGame = new Game()
                {
                    Name = gameName,
                    Description = gameName
                };



                if (newGame is null)
                {
                    continue;
                }

                newGame.GameUsers = new List<GameUser>();
                newGame.GameUsers.Add(new GameUser()
                {
                    Game = newGame,
                    User = user
                });

                newGame.GamePlatforms = new List<GamePlatform>();
                newGame.GamePlatforms.Add(new GamePlatform()
                {
                    AppId = appId,
                    Game = newGame,
                    Platform = platform,
                    PlatformId = platform.Id
                });

                newGames.Add(newGame);
            }

            return newGames;
        }

        public async Task GetUserInfo(string steamId)
        {
            var url = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_appSettings.SteamKey}&steamids={steamId}";
            var response = await _client.GetStringAsync(url);
        }


    }
}
