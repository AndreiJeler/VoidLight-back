using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoidLight.Data.Business.Authentication;
using VoidLight.Business.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;

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

        public AuthenticationController(Business.Services.Contracts.IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(AuthenticateRequestDto model)
        {
            return Ok(await _authenticationService.Authenticate(model));
        }

        [HttpPost("isAuthenticated")]
        public async Task<IActionResult> CheckIfAuthenticated([FromBody] TokenDto token)
        {
            return Ok(await _authenticationService.GetUser(token.Token));
        }

        [HttpGet("steam-login")]
        [AllowAnonymous]
        public IActionResult SteamLogin()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:44324/api/authentication/steam-done" }, "Steam");
        }

        [HttpGet("steam-done")]
        [AllowAnonymous]
        public async Task<IActionResult> SteamRegisterSuccess()
        {
            var claims = HttpContext.User.Claims;
            var nameIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.Split('/').Last();
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var id = await _userService.GetUserIdSteamLogin(nameIdentifier, name);

            return Redirect($"http://localhost:4200/steam-return/?id={id.ToString()}");
        }

        [HttpGet("authenticateId/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateId(int id)
        {
            return Ok(await _authenticationService.GetUserById(id));
        }
    }
}
