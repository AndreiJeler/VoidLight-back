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
    /// <summary>
    /// User controller responsible for the user operations
    /// </summary>
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

        /// <summary>
        /// This POST method creates a new user
        /// </summary>
        /// <param name="userData">The user dto</param>
        /// <returns>The created user</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto userData)
        {
            await _userService.CreateUser(userData);
            return Created(Constants.HTTP_CREATED, userData);
        }

        /// <summary>
        /// This POST method activates a new user's account
        /// </summary>
        /// <param name="token">The user</param>
        /// <returns></returns>
        [HttpPost("userToken")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivateUserAccount([FromQuery] string token)
        {
            await _userService.ActivateAccount(token);
            return Ok();
        }

        /// <summary>
        /// This PUT method resets a user's password
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <param name="newPassword">The user's new password</param>
        /// <param name="isForgotten">Field for whether the password was forgotten or not</param>
        /// <returns></returns>
        [HttpPut("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string password, [FromQuery] string newPassword, [FromQuery] bool isForgotten)
        {
            await _userService.ResetPassword(email, isForgotten, password, newPassword);
            return Ok();
        }

        /// <summary>
        /// This PUT method updates a users information
        /// </summary>
        /// <param name="userData">The user dto</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser()
        {
            await _userService.UpdateUser(Request.Form["user"][0], Request.Form.Files);
            return Created(Constants.HTTP_UPDATED, Request.Form["user"][0]);
        }

        /// <summary>
        /// This GET method looks for a user that has a specific ID
        /// </summary>
        /// <param name="id">The searched ID</param>
        /// <returns>The user</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userService.GetById(id));
        }

        /// <summary>
        /// This GET method creates a new account using the Steam API
        /// </summary>
        /// <returns>Redirects to the Steam login page</returns>
        [HttpGet("steam-register")]
        [AllowAnonymous]
        public IActionResult SteamRegister()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = $"{_appSettings.AppUrl}/api/users/steam-done" }, "Steam");
        }

        /// <summary>
        /// This GET method registers a user using Steam
        /// </summary>
        /// <returns>Redirects back to the webpage with the corresponding ID</returns>
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

        /// <summary>
        /// This GET method looks for users corresponding to a name
        /// </summary>
        /// <param name="name">The searched name</param>
        /// <returns>The list of users</returns>
        [HttpGet("username/{name}")]
        [AllowAnonymous]
        public IActionResult GetUsersWithName(string name)
        {
            return Ok(_userService.GetUsersWithName(name));
        }

        /// <summary>
        /// This GET method authenticates the user with Discord
        /// </summary>
        /// <param name="code">The Oauth code</param>
        /// <returns>The user's ID</returns>
        [HttpGet("discord/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> DiscordOAuth(string code)
        {
            return Ok(await _userService.DiscordAuthentication(code));
        }
    }
}
