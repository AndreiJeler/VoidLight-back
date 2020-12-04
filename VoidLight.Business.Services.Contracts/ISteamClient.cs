using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoidLight.Business.Services.Contracts
{
    public interface ISteamClient
    {
        public Task GetUserInfo(string steamId);
        public Task<string> GetUserCurrentPlayingGame(string steamId);

    }
}
