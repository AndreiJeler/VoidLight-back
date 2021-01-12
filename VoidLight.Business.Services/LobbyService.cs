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
    public class LobbyService : ILobbyService
    {
        public IGameService _gameService;
        public IDiscordService _discordService;
        private readonly VoidLightDbContext _context;
        private readonly ISteamClient _steamClient;
        public LobbyService(IGameService gameService, IDiscordService discordService, VoidLightDbContext context, ISteamClient steamClient)
        {
            _gameService = gameService;
            _discordService = discordService;
            _context = context;
            _steamClient = steamClient;
        }

        public IAsyncEnumerable<GameLobbiesInfoDto> GetAllGameInfoForUser(int userId)
        {
            return _context.GameUsers.Include(g => g.Game).ThenInclude(g => g.Lobbies)
                .Select(g => new GameLobbiesInfoDto()
                {
                    GameId = g.Game.Id,
                    GameIcon = g.Game.Icon,
                    GameName = g.Game.Name,
                    NrLobbies = g.Game.Lobbies.Count()
                }).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<GameLobbiesInfoDto> GetFavouriteGameInfoForUser(int userId)
        {
            return _context.GameUsers.Include(g => g.Game).ThenInclude(g => g.Lobbies)
                .Select(g => new GameLobbiesInfoDto()
                {
                    GameId = g.Game.Id,
                    GameIcon = g.Game.Icon,
                    GameName = g.Game.Name,
                    NrLobbies = g.Game.Lobbies.Count()
                }).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<LobbyDto> GetGameLobbies(int gameId)
        {
            return _context.Lobbies
                .Include(g => g.Game)
                .Include(l => l.UserLobbies).ThenInclude(ul => ul.User)
                .Select(lobby =>

                new LobbyDto()
                {
                    Id = lobby.Id,
                    GameId = lobby.GameId,
                    GameName = lobby.Game.Name,
                    HasStarted = lobby.HasStarted,
                    ParticipantsNr = lobby.UserLobbies.Count()
                }
            ).AsAsyncEnumerable();
        }

        public async Task<LobbyDto> GetLobby(int lobbyId)
        {
            throw new Exception("sal");
        }
    }
}
