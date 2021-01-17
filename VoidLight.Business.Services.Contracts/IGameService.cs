using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business;

namespace VoidLight.Business.Services.Contracts
{
    public interface IGameService
    {
        public IAsyncEnumerable<GameDto> GetUserGames(int userId);
        public IAsyncEnumerable<GameDto> GetUserFavouriteGames(int userId);
        public Task UpdateFavoriteGame(int userId, int gameId);
    }
}
