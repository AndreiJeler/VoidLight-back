using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Business;

namespace VoidLight.Business.Services.Contracts
{
    public interface IGameService
    {
        public IAsyncEnumerable<GameDto> GetUserGames(int userId);
        public IAsyncEnumerable<GameDto> GetUserFavouriteGames(int userId);
    }
}
