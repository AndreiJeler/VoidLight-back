using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;
using VoidLight.Data.Mappers;

namespace VoidLight.Business.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly VoidLightDbContext _context;
        private readonly ISteamClient _steamClient;


        public AchievementService(VoidLightDbContext context, ISteamClient steamClient)
        {
            _context = context;
            _steamClient = steamClient;
        }


        public async Task<IList<GameAchievement>> GetCurrentUserGameAchievements(int userId, int gameId)
        {
            return await _context.GameAchievements.Include(ga => ga.Game).Include(ga => ga.User).ToListAsync();
        }

        public IAsyncEnumerable<GameAchievementDto> GetCurrentUserGameAchievementsDTO(int userId, int gameId)
        {
            return _context.GameAchievements.Include(ga => ga.Game).Include(ga => ga.User).Select(g => GameAchievementMapper.ConvertEntityToDto(g)).AsAsyncEnumerable();

        }

        public async Task GetGameAchievements(int gameId)
        {
            var steamPlatform = await _context.Platforms.FirstOrDefaultAsync(platf => platf.Name == "Steam");
            var game = await _context.Games.FirstOrDefaultAsync(game => game.Id == gameId);
            var gamePlatform = await _context.GamePlatforms.FirstOrDefaultAsync(gp => gp.GameId == gameId && gp.PlatformId == steamPlatform.Id);
            var appId = gamePlatform.AppId;
            await _steamClient.GetGameAchievements(appId);
        }

        public async Task<IList<GameAchievement>> GetUserGameAchievements(int userId, int gameId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var steamPlatform = await _context.Platforms.FirstOrDefaultAsync(platf => platf.Name == "Steam");
            var userPlatform = await _context.UserPlatforms.FirstOrDefaultAsync(up => up.UserId == userId && up.PlatformId == steamPlatform.Id);
            var game = await _context.Games.FirstOrDefaultAsync(game => game.Id == gameId);
            var gamePlatform = await _context.GamePlatforms.FirstOrDefaultAsync(gp => gp.GameId == gameId && gp.PlatformId == steamPlatform.Id);
            var appId = gamePlatform.AppId;

            return await _steamClient.GetUserGameAchievements(userPlatform.LoginToken, appId, user, game);
        }

        public async Task<IEnumerable<GameAchievementDto>> AddNewUserGameAchievements(int userId, int gameId)
        {
            var steamAchievements = await GetUserGameAchievements(userId, gameId);
            var newAchievements = new List<GameAchievement>();
            var allAchievements = _context.GameAchievements.ToList();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var steamPlatform = await _context.Platforms.FirstOrDefaultAsync(platf => platf.Name == "Steam");
            var userPlatform = await _context.UserPlatforms.FirstOrDefaultAsync(up => up.UserId == userId && up.PlatformId == steamPlatform.Id);
            var game = await _context.Games.Include(g => g.GameAchievements).ThenInclude(ga => ga.User).FirstOrDefaultAsync(game => game.Id == gameId);
            var gamePlatform = await _context.GamePlatforms.FirstOrDefaultAsync(gp => gp.GameId == gameId && gp.PlatformId == steamPlatform.Id);

            foreach (var achievement in steamAchievements)
            {
                if (allAchievements.Any(a => a.GameId == gameId && a.Description == achievement.Description && a.UserId == userId))
                {
                    continue;
                }
                if (newAchievements.Any(a => a.GameId == gameId && a.Description == achievement.Description && a.UserId == userId))
                {
                    continue;
                }
                newAchievements.Add(achievement);
            }
            await _context.AddRangeAsync(newAchievements);
            await _context.SaveChangesAsync();
            return newAchievements.Select(na => GameAchievementMapper.ConvertEntityToDto(na)).AsEnumerable();
        }


    }
}
