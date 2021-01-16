using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;
using VoidLight.Data.Business.Discord;
using VoidLight.Data.Entities;

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

        public async Task<ICollection<LobbyDto>> GetGameLobbies(int gameId)
        {
            var discordPlatform = await _context.Platforms.FirstOrDefaultAsync(platf => platf.Name == "Discord");
            return _context.Lobbies
                .Include(g => g.Game)
                .Include(l => l.UserLobbies).ThenInclude(ul => ul.User).ThenInclude(u => u.UserPlatforms)
                .Where(l => l.GameId == gameId && l.HasStarted == false)
                .AsEnumerable()
                .Select(lobby =>
               {
                   var user = lobby.UserLobbies.FirstOrDefault(u => u.IsInitializer == true);
                   var userPlatf = user.User.UserPlatforms.FirstOrDefault(p => p.PlatformId == discordPlatform.Id);
                   var initializer = new DiscordUserDto()
                   {
                       UserId = user.UserId,
                       AvatarPicture = user.User.AvatarPath,
                       DiscordId = userPlatf.LoginId,
                       DiscordToken = userPlatf.LoginToken,
                       Username = userPlatf.User.Username

                   };
                   return new LobbyDto()
                   {
                       Id = lobby.Id,
                       GameId = lobby.GameId,
                       GameName = lobby.Game.Name,
                       HasStarted = lobby.HasStarted,
                       ParticipantsNr = lobby.UserLobbies.Count(),
                       Initializer = initializer
                   };
               }
            ).ToList();
        }

        public async Task<LobbyDto> GetLobby(int lobbyId)
        {
            var discordPlatform = await _context.Platforms.FirstOrDefaultAsync(platf => platf.Name == "Discord");
            var lobby = await _context.Lobbies
                .Include(l => l.UserLobbies).ThenInclude(u => u.User).ThenInclude(u => u.UserPlatforms)
                .Include(g => g.Game)
                .FirstOrDefaultAsync(l => l.Id == lobbyId);
            var users = lobby.UserLobbies.Select(u =>
            {
                var userPlatf = u.User.UserPlatforms.FirstOrDefault(p => p.PlatformId == discordPlatform.Id); return new DiscordUserDto()
                {
                    UserId = u.UserId,
                    AvatarPicture = u.User.AvatarPath,
                    DiscordId = userPlatf.LoginId,
                    DiscordToken = userPlatf.LoginToken,
                    Username = userPlatf.User.Username
                };
            });
            var user = lobby.UserLobbies.FirstOrDefault(ul => ul.IsInitializer == true);
            var userPlatf = user.User.UserPlatforms.FirstOrDefault(p => p.PlatformId == discordPlatform.Id);

            var initializer = new DiscordUserDto()
            {

                UserId = user.UserId,
                AvatarPicture = user.User.AvatarPath,
                DiscordId = userPlatf.LoginId,
                DiscordToken = userPlatf.LoginToken,
                Username = userPlatf.User.Username

            };

            return new LobbyDto()
            {
                Id = lobby.Id,
                HasStarted = lobby.HasStarted,
                GameId = lobby.GameId,
                GameName = lobby.Game.Name,
                ParticipantsNr = users.Count(),
                Users = users,
                Initializer = initializer,
            };

        }

        public async Task<string> OpenDiscordChannel(int lobbyId)
        {
            var lobbyDto = await GetLobby(lobbyId);
            foreach (var user in lobbyDto.Users)
            {
                await _discordService.AddUserToGuild(user);
            }

            var lobby = await _context.Lobbies.FirstOrDefaultAsync(l => l.Id == lobbyId);

            lobby.HasStarted = true;
            _context.Update(lobby);
            await _context.SaveChangesAsync();

            return await _discordService.CreateLobbyChannel(lobbyDto);

        }

        public async Task<LobbyDto> CreateLobby(LobbyCreationDto dto)
        {
            if (_context.Lobbies.Include(l => l.UserLobbies).Where(l => !l.HasStarted && l.UserLobbies.Any(l => l.UserId == dto.UserId && l.IsInitializer == true)).Count() != 0)
            {
                throw new Exception("You already have an open lobby");
            }
            var user = await _context.Users.Include(u => u.UserLobbies).FirstOrDefaultAsync(u => u.Id == dto.UserId);
            var game = await _context.Games.Include(g => g.Lobbies).FirstOrDefaultAsync(g => g.Id == dto.GameId);
            var lobby = new Lobby()
            {
                GameId = dto.GameId,
                Game = game,
                HasStarted = false,
                Name = user.Username + "'s lobby",
                UserLobbies = new List<UserLobby>()
            };
            var userLobby = new UserLobby()
            {
                IsInitializer = true,
                Lobby = lobby,
                LobbyId = lobby.Id,
                User = user,
                UserId = user.Id
            };
            lobby.UserLobbies.Add(userLobby);
            await _context.AddAsync(lobby);
            await _context.SaveChangesAsync();
            return await GetLobby(lobby.Id);
        }

        public async Task<LobbyDto> JoinLobby(int lobbyId, int userId)
        {
            if (_context.Lobbies.Include(l => l.UserLobbies).Where(l => !l.HasStarted && l.UserLobbies.Any(l => l.UserId == userId)).Count() != 0)
            {
                //throw new Exception("You already have joined a lobby. Leave it in order to enter another");
                var auxLobby = await _context.Lobbies.Include(l=>l.UserLobbies).FirstOrDefaultAsync(l => l.Id == lobbyId);
                if (auxLobby.UserLobbies.FirstOrDefault(u => u.UserId == userId)!=null){
                    return await GetLobby(lobbyId);
                }
            }
            var lobby = await _context.Lobbies.Include(l => l.UserLobbies).ThenInclude(l => l.User).FirstOrDefaultAsync(l => l.Id == lobbyId);
            var user = await _context.Users.Include(l => l.UserLobbies).FirstOrDefaultAsync(u => u.Id == userId);
            lobby.UserLobbies.Add(new UserLobby() { IsInitializer = false, Lobby = lobby, LobbyId = lobby.Id, User = user, UserId = user.Id });
            _context.Update(lobby);
            await _context.SaveChangesAsync();
            return await GetLobby(lobbyId);

        }
    }
}
