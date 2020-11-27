using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                 .Include(gu => gu.Game).ThenInclude(g => g.Publisher)
                 .Where(gu => gu.UserId == userId && gu.IsFavourite == true)
                 .Select(gu => new GameDto()
                 {
                     Description = gu.Game.Description,
                     Id = gu.GameId,
                     Name = gu.Game.Name,
                     Publisher = gu.Game.Publisher == null ? "No publisher" : gu.Game.Publisher.Name,
                     IsFavourite=gu.IsFavourite
                 }).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<GameDto> GetUserGames(int userId)
        {
            return _context.GameUsers
                  .Include(gu => gu.Game).ThenInclude(g => g.Publisher)
                  .Where(gu => gu.UserId == userId)
                  .Select(gu => new GameDto()
                  {
                      Description = gu.Game.Description,
                      Id = gu.GameId,
                      Name = gu.Game.Name,
                      Publisher = gu.Game.Publisher == null ? "No publisher" : gu.Game.Publisher.Name,
                      IsFavourite = gu.IsFavourite

                  }).AsAsyncEnumerable();
        }
    }
}
