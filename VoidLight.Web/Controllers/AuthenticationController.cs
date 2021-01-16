using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoidLight.Data.Business.Authentication;
using VoidLight.Business.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using VoidLight.Data;
using Microsoft.Extensions.Options;

namespace VoidLight.Web.Controllers
{
    /// <summary>
    /// Authentication controller responsible for the authentication operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly Business.Services.Contracts.IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(Business.Services.Contracts.IAuthenticationService authenticationService, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// This POST method authenticates a user
        /// </summary>
        /// <param name="model">The authentication request dto</param>
        /// <returns>The user</returns>
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(AuthenticateRequestDto model)
        {
            return Ok(await _authenticationService.Authenticate(model));
        }

        /// <summary>
        /// This POST method checks if user is authenticated
        /// </summary>
        /// <param name="token">The user token</param>
        /// <returns>The user</returns>
        [HttpPost("isAuthenticated")]
        public async Task<IActionResult> CheckIfAuthenticated([FromBody] TokenDto token)
        {
            return Ok(await _authenticationService.GetUser(token.Token));
        }

        /// <summary>
        /// This GET method authenticates the user using the Steam API
        /// </summary>
        /// <returns>Redirects to the Steam login page</returns>
        [HttpGet("steam-login")]
        [AllowAnonymous]
        public IActionResult SteamLogin()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = $"{_appSettings.AppUrl}/api/authentication/steam-done" }, "Steam");
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

            var id = await _userService.GetUserIdSteamLogin(nameIdentifier, name);

            return Redirect($"{_appSettings.WebFrontEndUrl}/steam-return/?id={id.ToString()}");
        }

        /// <summary>
        /// This GET method authenticates a user by ID
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The user with the given ID</returns>
        [HttpGet("authenticateId/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateId(int id)
        {
            return Ok(await _authenticationService.GetUserById(id));
        }


        [HttpGet("steam-sync")]
        [AllowAnonymous]
        public IActionResult SteamSync()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = $"{_appSettings.AppUrl}/api/authentication/steam-sync-done" }, "Steam");
        }

        [HttpGet("steam-sync-done")]
        [AllowAnonymous]
        public async Task<IActionResult> SteamSyncDone()
        {
            var claims = HttpContext.User.Claims;
            var nameIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.Split('/').Last();
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            //var id = await _userService.GetUserIdSteamLogin(nameIdentifier, name);

            return Redirect($"{_appSettings.WebFrontEndUrl}/steam-return/?id={nameIdentifier}&sync=1&username={name}");
        }
    }
}
