using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data.Business;
using VoidLight.Data.Business.Authentication;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;
using VoidLight.Web.Hubs;
using VoidLight.Web.Infrastructure.Authorization;

namespace VoidLight.Web.Controllers
{
    /// <summary>
    /// Lobby controller responsible for the lobby operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private ILobbyService _lobbyService;
        private readonly IHubContext<LobbyHub> _hub;

        public LobbyController(ILobbyService service, IHubContext<LobbyHub> hub)
        {
            _lobbyService = service;
            _hub = hub;
        }

        /// <summary>
        /// This GET method looks for a lobby
        /// </summary>
        /// <param name="id">The ID of the lobby</param>
        /// <returns>The lobby</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLobby(int id)
        {
            return Ok(await _lobbyService.GetLobby(id));
        }

        /// <summary>
        /// This GET method creates a channel for the Discord server
        /// </summary>
        /// <param name="id">The ID of the lobby</param>
        /// <returns>The name of the channel</returns>
        [HttpGet("start/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> StartChannel(int id)
        {
            return Ok(await _lobbyService.OpenDiscordChannel(id));
        }

        /// <summary>
        /// This GET method looks for all the details of the lobby
        /// </summary>
        /// <param name="userId">The ID of the lobby's creator</param>
        /// <returns>The list of the games</returns>
        [HttpGet("games/{userId}")]
        [AllowAnonymous]
        public IActionResult GetAllGameLobbyDetails(int userId)
        {
            return Ok(_lobbyService.GetAllGameInfoForUser(userId));
        }

        /// <summary>
        /// This GET method looks for all the lobbies corresponding to a specific game
        /// </summary>
        /// <param name="gameId">The ID of the game</param>
        /// <returns>The list of lobbies</returns>
        [HttpGet("game/{gameId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllGameLobbies(int gameId)
        {
            return Ok(await _lobbyService.GetGameLobbies(gameId));
        }

        /// <summary>
        /// This GET method looks for details of the lobbies specific to favourite games
        /// </summary>
        /// <param name="userId">The ID of user</param>
        /// <returns>The list of favourite games</returns>
        [HttpGet("favourite/{userId}")]
        [AllowAnonymous]
        public IActionResult GetAllFavouriteGameLobbyDetails(int userId)
        {
            return Ok(_lobbyService.GetFavouriteGameInfoForUser(userId));
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public IActionResult CreateLobby([FromBody] LobbyCreationDto dto)
        {
            return Ok(_lobbyService.CreateLobby(dto));
        }
    }
}
