using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Infrastructure.Common;

namespace VoidLight.Business.Services
{
    public class SteamGameCollection : ISteamGameCollection
    {
        private readonly JEnumerable<JToken> _games;
        private readonly HttpClient _client;

        public SteamGameCollection()
        {
            _client = new HttpClient();
            _games = GetGames().Result;
        }

        private async Task<JEnumerable<JToken>> GetGames()
        {
            var url = await _client.GetStringAsync(Constants.STEAM_ALL_GAMES_URL);
            var response = (JObject)JsonConvert.DeserializeObject(url);
            var objects = response.SelectToken(Constants.STEAM_ALL_GAMES_APP_LIST).Children(); //Constant
            return objects;
        }

        public Task<JToken> GetGameName(string appId)
        {
            return Task.Run(() => _games.FirstOrDefault(ga => ga[Constants.STEAM_APP_ID].Value<string>() == appId));
        }
    }
}
