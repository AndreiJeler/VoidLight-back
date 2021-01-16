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
        public Task<ICollection<LobbyDto>> GetGameLobbies(int gameId);
        public Task<LobbyDto> GetLobby(int lobbyId);
        public Task<string> OpenDiscordChannel(int lobbyId);
        public Task<LobbyDto> CreateLobby(LobbyCreationDto dto);
        public Task<LobbyDto> JoinLobby(int lobbyId, int userId);
        public Task<LobbyDto> LeaveLobby(int lobbyId, int userId);

    }
}
