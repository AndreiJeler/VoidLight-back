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
    public class AchievementsController : ControllerBase
    {
        private IAchievementService _achievementService;


        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet("game/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameAchievements(int id)
        {
            await _achievementService.GetGameAchievements(id);
            return NoContent();
        }
        [HttpGet("user/{id}/game/{gameId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserGameAchievements(int id, int gameId)
        {
            return Ok(_achievementService.GetCurrentUserGameAchievementsDTO(id, gameId));
        }
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewAchievementsForGame(GameAchievementDto dto)
        {
            return Ok(await _achievementService.AddNewUserGameAchievements(dto.UserId, dto.GameId));
        }
    }
}
