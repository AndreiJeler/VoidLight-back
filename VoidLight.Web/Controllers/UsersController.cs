using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;
using VoidLight.Data.Business.Authentication;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;
using VoidLight.Web.Infrastructure.Authorization;

namespace VoidLight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;


        public UsersController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto userData)
        {
            await _userService.CreateUser(userData);
            return Created(Constants.HTTP_CREATED, userData);
        }

        [HttpPost("userToken")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivateUserAccount([FromQuery] string token)
        {
            await _userService.ActivateAccount(token);
            return Ok();
        }

        [HttpPut("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string password, [FromQuery] string newPassword, [FromQuery] bool isForgotten)
        {
            await _userService.ResetPassword(email, isForgotten, password, newPassword);
            return Ok();
        }

        [AuthorizeUserCustom(RoleType.General)]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userData)
        {
            await _userService.UpdateUser(userData);
            return Created(Constants.HTTP_UPDATED, userData);
        }

        [HttpGet("regular")]
        [AuthorizeUserCustom(RoleType.Regular)]
        public IActionResult GetAllRegular()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("admin")]
        [AuthorizeUserCustom(RoleType.Admin)]
        public IActionResult GetAllAdmin()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("general")]
        [AuthorizeUserCustom(RoleType.Regular)]
        public IActionResult GetAllGeneral()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("streamer")]
        [AuthorizeUserCustom(RoleType.Streamer)]
        public IActionResult GetAllStreamer()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userService.GetById(id));
        }

        [HttpGet("steam-register")]
        [AllowAnonymous]
        public IActionResult SteamRegister()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = $"{_appSettings.AppUrl}/api/users/steam-done" }, "Steam");
        }

        [HttpGet("steam-done")]
        [AllowAnonymous]
        public async Task<IActionResult> SteamRegisterSuccess()
        {
            var claims = HttpContext.User.Claims;
            var nameIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.Split('/').Last();
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            await _userService.SteamRegister(nameIdentifier, name);

            return Redirect($"{_appSettings.WebFrontEndUrl}/steam-return");
        }
        [HttpGet("username/{name}")]
        [AllowAnonymous]
        public IActionResult GetUsersWithName(string name)
        {
            return Ok(_userService.GetUsersWithName(name));
        }
        [HttpGet("discord/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> DiscordOAuth(string code)
        {
            return Ok(await _userService.DiscordAuthentication(code));
        }
        [HttpGet("username/{name}")]
        [AllowAnonymous]
        public IActionResult GetUsersWithName(string name)
        {
            return Ok(_userService.GetUsersWithName(name));
        }
        [HttpGet("discord/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> DiscordOAuth(string code)
        {
            return Ok(await _userService.DiscordAuthentication(code));
        }
    }
}
