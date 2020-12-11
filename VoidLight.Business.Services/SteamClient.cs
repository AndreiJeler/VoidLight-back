﻿using Microsoft.Extensions.Options;
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