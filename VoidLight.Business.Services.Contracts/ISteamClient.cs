using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Entities;

namespace VoidLight.Business.Services.Contracts
{
    public interface ISteamClient
    {
        public Task GetUserInfo(string steamId);
        public Task<string> GetUserCurrentPlayingGame(string steamId);
        public Task<IEnumerable<Game>> GetUserGames(string steamId, User user, Platform platform);
        public Task<Game> GetGameDetails(string appId);

    }
}
