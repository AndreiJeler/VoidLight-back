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
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService= gameService;
        }

        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public IActionResult GetUserGames(int id)
        {
            return Ok(_gameService.GetUserGames(id));
        }

        [HttpGet("favorite/user/{id}")]
        [AllowAnonymous]
        public IActionResult GetUserFavouriteGames(int id)
        {
            return Ok(_gameService.GetUserFavouriteGames(id));
        }
    }
}
