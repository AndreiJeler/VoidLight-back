using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data.Business;
using VoidLight.Data.Business.Authentication;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;
using VoidLight.Web.Infrastructure.Authorization;

namespace VoidLight.Web.Controllers
{
    /// <summary>
    /// Games controller responsible for games operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService= gameService;
        }

        /// <summary>
        /// This GET method looks for a user's games
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The list of games</returns>
        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public IActionResult GetUserGames(int id)
        {
            return Ok(_gameService.GetUserGames(id));
        }

        /// <summary>
        /// This GET method looks for a user's favourite games
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The list of favourite games</returns>
        [HttpGet("favorite/user/{id}")]
        [AllowAnonymous]
        public IActionResult GetUserFavouriteGames(int id)
        {
            return Ok(_gameService.GetUserFavouriteGames(id));
        }
    }
}
