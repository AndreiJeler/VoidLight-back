using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;

namespace VoidLight.Business.Services
{
    public class GameService : IGameService
    {
        private readonly VoidLightDbContext _context;

        public GameService(VoidLightDbContext context)
        {
            _context = context;
        }

        public IAsyncEnumerable<GameDto> GetUserFavouriteGames(int userId)
        {
            return _context.GameUsers
                 //.Include(gu => gu.Game).ThenInclude(g => g.Publisher)
                 .Where(gu => gu.UserId == userId && gu.IsFavourite == true)
                 .Select(gu => new GameDto()
                 {
                     Description = gu.Game.Description,
                     Id = gu.GameId,
                     Name = gu.Game.Name,
                  //   Publisher = gu.Game.Publisher == null ? "No publisher" : gu.Game.Publisher.Name,
                     IsFavourite=gu.IsFavourite,
                     Icon = gu.Game.Icon,
                   //  AchievementsAcquired=gu.AchievementsAcquired,
                   //  AchievementsTotal=gu.Game.AchievementTotal,
                     TimePlayed=gu.TimePlayed
                 }).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<GameDto> GetUserGames(int userId)
        {
            return _context.GameUsers
                  //.Include(gu => gu.Game).ThenInclude(g => g.Publisher)
                  .Where(gu => gu.UserId == userId)
                  .Select(gu => new GameDto()
                  {
                      Description = gu.Game.Description,
                      Id = gu.GameId,
                      Name = gu.Game.Name,
                      //Publisher = gu.Game.Publisher == null ? "No publisher" : gu.Game.Publisher.Name,
                      IsFavourite = gu.IsFavourite,
                      Icon = gu.Game.Icon,
                     // AchievementsAcquired = gu.AchievementsAcquired,
                     // AchievementsTotal = gu.Game.AchievementTotal,
                      TimePlayed = gu.TimePlayed
                  }).AsAsyncEnumerable();
        }

        public async Task UpdateFavoriteGame(int userId, int gameId)
        {
            var game = await _context.Games.Include(g => g.GameUsers).FirstOrDefaultAsync(g => g.Id == gameId);
            if (game == null)
            {
                throw new Exception("Invalid game");
            }
            var userGame = game.GameUsers.FirstOrDefault(u => u.UserId == userId);
            if (userGame == null)
            {
                throw new Exception("Invalid user");
            }
            userGame.IsFavourite = !userGame.IsFavourite;
            _context.Update(userGame);
            await _context.SaveChangesAsync();
        }
    }
}
