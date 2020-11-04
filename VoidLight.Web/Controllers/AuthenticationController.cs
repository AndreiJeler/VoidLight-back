using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoidLight.Data.Business.Authentication;
using VoidLight.Business.Services.Contracts;

namespace VoidLight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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
    }
}
