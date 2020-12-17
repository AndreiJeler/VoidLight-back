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
    public class FriendsController : ControllerBase
    {
        private IFriendService _friendService;
        private readonly IHubContext<FriendsHub> _hub;


        public FriendsController(IFriendService friendService, IHubContext<FriendsHub> hub)
        {
            _friendService = friendService;
            _hub = hub;
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
            await _hub.Clients.All.SendAsync("new-" + requestDto.ReceiverId, "You have a new friend request");
            return NoContent();
        }

        [HttpPost("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            var initializer = await _friendService.ConfirmFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            await _hub.Clients.All.SendAsync("accept-" + requestDto.InitializerId, $"{initializer} has accepted your friend request");
            return NoContent();
        }

        [HttpPost("decline")]
        [AllowAnonymous]
        public async Task<IActionResult> DeclineFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            var initializer = await _friendService.DeclineFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            await _hub.Clients.All.SendAsync("decline-" + requestDto.InitializerId, $"{initializer} has declined your friend request");
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
