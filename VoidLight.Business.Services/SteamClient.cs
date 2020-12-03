using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;

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

        public async Task GetUserInfo(string steamId)
        {
            var url = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_appSettings.SteamKey}&steamids={steamId}";
            var response = await _client.GetStringAsync(url);
        }


    }
}
