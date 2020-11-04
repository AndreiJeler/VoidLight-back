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
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
    }
}
