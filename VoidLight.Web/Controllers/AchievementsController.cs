using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    /// Achievements controller responsible for the achievements operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private IAchievementService _achievementService;


        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        /// <summary>
        /// This GET method returns all the achievements of a specific game
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET : /game/330
        ///     {
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">The id of the game</param>
        /// <returns>The list of achievements</returns>
        /// <response code="200">Return the list of achievements</response>
        /// <response code="500">An error occured on the server</response>
        [HttpGet("game/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameAchievements(int id)
        {
            await _achievementService.GetGameAchievements(id);
            return NoContent();
        }

        /// <summary>
        /// This GET method returns all the achievements of a specific game obtained by a user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <param name="gameId">The ID of the game</param>
        /// <returns>The list of achievements of a user in a given game</returns>
        /// <response code="200">Return the list of achievements</response>
        /// <response code="500">An error occured on the server</response>
        [HttpGet("user/{id}/game/{gameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public IActionResult GetUserGameAchievements(int id, int gameId)
        {
            return Ok(_achievementService.GetCurrentUserGameAchievementsDTO(id, gameId));
        }

        /// <summary>
        /// This POST method looks for new achievements obtained by a user in a game
        /// </summary>
        ///  <remarks>
        /// Sample request:
        /// 
        ///     POST : /refresh
        ///       {
        ///            userId: 1,
        ///            gameId: 330
        ///       }
        /// </remarks>
        /// <param name="dto">The achievement dto, which should contain the userId and GameId</param>
        /// <returns>The list of new achievements obtained</returns>
        /// <response code="200">Return the list of new achievements</response>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewAchievementsForGame(GameAchievementDto dto)
        {
            return Ok(await _achievementService.AddNewUserGameAchievements(dto.UserId, dto.GameId));
        }
    }
}
