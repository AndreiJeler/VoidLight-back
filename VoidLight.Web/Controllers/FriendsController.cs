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
    public class FriendsController : ControllerBase
    {
        private IFriendService _friendService;

        public FriendsController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFriendsOfUser(int id)
        {
            return Ok(await _friendService.GetFriendsOfUser(id));
        }

        [HttpPost("request")]
        [AllowAnonymous]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            await _friendService.SendFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            return NoContent();
        }

        [HttpPost("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            await _friendService.ConfirmFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            return NoContent();
        }

        [HttpPost("decline")]
        [AllowAnonymous]
        public async Task<IActionResult> DeclineFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            await _friendService.DeclineFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            return NoContent();
        }

        [HttpGet("requests/{id}")]
        [AllowAnonymous]
        public IActionResult GetFriendRequests(int id)
        {
            return Ok(_friendService.GetUserFriendRequests(id));
        }
    }
}
