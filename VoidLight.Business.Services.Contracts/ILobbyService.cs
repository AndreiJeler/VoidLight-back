using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business;

namespace VoidLight.Business.Services.Contracts
{
    public interface ILobbyService
    {
        public IAsyncEnumerable<GameLobbiesInfoDto> GetAllGameInfoForUser(int userId);
        public IAsyncEnumerable<GameLobbiesInfoDto> GetFavouriteGameInfoForUser(int userId);
        public IAsyncEnumerable<LobbyDto> GetGameLobbies(int gameId);
        public Task<LobbyDto> GetLobby(int lobbyId);

    }
}
