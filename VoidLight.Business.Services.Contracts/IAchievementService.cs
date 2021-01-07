using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;

namespace VoidLight.Business.Services.Contracts
{
    public interface IAchievementService
    {
        public Task GetGameAchievements(int gameId);
        public Task<IList<GameAchievement>> GetUserGameAchievements(int userId, int gameId);
        public Task<IList<GameAchievement>> GetCurrentUserGameAchievements(int userId, int gameId);
        public IAsyncEnumerable<GameAchievementDto> GetCurrentUserGameAchievementsDTO(int userId, int gameId);
        public Task<IEnumerable<GameAchievementDto>> AddNewUserGameAchievements(int userId, int gameId);




    }
}
