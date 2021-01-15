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
    /// <summary>
    /// Friends controller responsible for the friends operations
    /// </summary>
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

        /// <summary>
        /// This GET method looks for the friends of the user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The list of friends</returns>
        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFriendsOfUser(int id)
        {
            return Ok(await _friendService.GetFriendsOfUser(id));
        }

        /// <summary>
        /// This POST method sends a friend request to another user
        /// </summary>
        /// <param name="requestDto">The request dto</param>
        /// <returns>The hope it works</returns>
        [HttpPost("request")]
        [AllowAnonymous]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            await _friendService.SendFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            await _hub.Clients.All.SendAsync("new-" + requestDto.ReceiverId, "You have a new friend request");
            return NoContent();
        }

        /// <summary>
        /// This POST method confirms the friend request received
        /// </summary>
        /// <param name="requestDto">The request dto</param>
        /// <returns>Nothing</returns>
        [HttpPost("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            var initializer = await _friendService.ConfirmFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            await _hub.Clients.All.SendAsync("accept-" + requestDto.InitializerId, $"{initializer} has accepted your friend request");
            return NoContent();
        }

        /// <summary>
        /// This POST method declines the friend request received
        /// </summary>
        /// <param name="requestDto">The request dto</param>
        /// <returns>Nothing</returns>
        [HttpPost("decline")]
        [AllowAnonymous]
        public async Task<IActionResult> DeclineFriendRequest([FromBody] FriendRequestDto requestDto)
        {
            var initializer = await _friendService.DeclineFriendRequest(requestDto.InitializerId, requestDto.ReceiverId);
            await _hub.Clients.All.SendAsync("decline-" + requestDto.InitializerId, $"{initializer} has declined your friend request");
            return NoContent();
        }

        /// <summary>
        /// This GET method looks for received friend requests for a user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The list of friend requests</returns>
        [HttpGet("requests/{id}")]
        [AllowAnonymous]
        public IActionResult GetFriendRequests(int id)
        {
            return Ok(_friendService.GetUserFriendRequests(id));
        }

        /// <summary>
        /// This DELETE method removes a friend from a user's friend list
        /// </summary>
        /// <param name="initId">The ID of the user</param>
        /// <param name="recId">The ID of the user's friend</param>
        /// <returns></returns>
        [HttpDelete("{initId}/{recId}")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveFriend(int initId, int recId)
        {
            await _friendService.DeleteFriends(initId, recId);
            return NoContent();
        }

        /// <summary>
        /// This DELETE method removes a friend request
        /// </summary>
        /// <param name="initId">The ID of the user</param>
        /// <param name="recId">The ID of the friend that sent the request</param>
        /// <returns></returns>
        [HttpDelete("request/{initId}/{recId}")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveFriendRequest(int initId, int recId)
        {
            await _friendService.RemoveFriendRequest(initId, recId);
            await _hub.Clients.All.SendAsync("remove-" + recId, $"One friendRequest was removed");
            return NoContent();
        }

        /// <summary>
        /// This GET method checks for the friendship status of two users
        /// </summary>
        /// <param name="initId">The ID of the user</param>
        /// <param name="recId">The ID of the user's friend</param>
        /// <returns>The type of friendship</returns>
        [HttpGet("status/{initId}/{recId}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckFriendshipStatus(int initId, int recId)
        {
            return Ok(await _friendService.GetFriendType(initId, recId));
        }
    }
}
