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

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLobby(int id)
        {
            return Ok(await _lobbyService.GetLobby(id));
        }

        [HttpGet("start/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> StartChannel(int id)
        {
            return Ok(await _lobbyService.OpenDiscordChannel(id));
        }

        [HttpGet("games/{userId}")]
        [AllowAnonymous]
        public IActionResult GetAllGameLobbyDetails(int userId)
        {
            return Ok(_lobbyService.GetAllGameInfoForUser(userId));
        }

        [HttpGet("game/{gameId}")]
        [AllowAnonymous]
        public IActionResult GetAllGameLobbies(int gameId)
        {
            return Ok(_lobbyService.GetGameLobbies(gameId));
        }

        [HttpGet("favourite/{userId}")]
        [AllowAnonymous]
        public IActionResult GetAllFavouriteGameLobbyDetails(int userId)
        {
            return Ok(_lobbyService.GetFavouriteGameInfoForUser(userId));
        }
    }
}
