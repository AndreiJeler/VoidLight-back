﻿using Newtonsoft.Json.Linq;
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
        public Task<JEnumerable<JToken>> GetGameAchievements(string appId);
        public Task<IList<GameAchievement>> GetUserGameAchievements(string steamId, string appId, User user, Game game);
        public Task<int> GetUnlockedAchievementsNumber(string steamId, string appId);
    }
}
