using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business.Discord;
using VoidLight.Infrastructure.Common;

namespace VoidLight.Business.Services
{
    public class DiscordService : IDiscordService
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _client;


        public DiscordService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _client = new HttpClient();
        }

        public async Task<string> DecodeAuthenticationCode(string code)
        {
            var oAuthRequest = JsonConvert.SerializeObject(new DiscordTokenRequest(_appSettings.DiscordClientId, _appSettings.DiscordClientSecret, code, Constants.DISCORD_REDIRECT_URI, Constants.DISCORD_SCOPES));

            var dict = new Dictionary<string, string>();
            dict.Add("client_id", _appSettings.DiscordClientId);
            dict.Add("client_secret", _appSettings.DiscordClientSecret);
            dict.Add("grant_type", "authorization_code");
            dict.Add("code", code);
            dict.Add("redirect_uri",Constants.DISCORD_REDIRECT_URI);
            dict.Add("scope", Constants.DISCORD_SCOPES);

            var formOAuth = new FormUrlEncodedContent(dict);

            var response = await _client.PostAsync(Constants.DISCORD_OAUTH_TOKEN_URL, formOAuth);
            var jsonData = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return jsonData.SelectToken("access_token").Value<string>();
        }

        public async Task<DiscordRetrieveUserResponse> DecodeToken(string token)
        {
            var newClient = new HttpClient();
            newClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await newClient.GetAsync(Constants.DISCORD_USER_TOKEN_URL);
            var jsonData = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            return new DiscordRetrieveUserResponse(jsonData.SelectToken("id").Value<string>(), jsonData.SelectToken("username").Value<string>(), jsonData.SelectToken("discriminator").Value<string>(), jsonData.SelectToken("email").Value<string>());

        }
    }
}
