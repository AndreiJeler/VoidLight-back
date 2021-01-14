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
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private readonly IHubContext<PostsHub> _hub;


        public PostsController(IPostService postService, IHubContext<PostsHub> hub)
        {
            _postService = postService;
            _hub = hub;
        }

        [HttpGet("game/{id}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGamePosts(int id, int userId)
        {
            return Ok(await _postService.GetGamePosts(id, userId));
        }

        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsForUser(int id)
        {
            return Ok(await _postService.GetPostsForUserFeed(id));
        }

        [HttpGet("publisher/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsForPublisher(int id)
        {
            return Ok(await _postService.GetGamePublisherPosts(id));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddPost()
        {
            await _hub.Clients.All.SendAsync("new", "New post");
            return Ok(await _postService.AddPost(Request.Form["post"][0], Request.Form.Files));
        }

        [HttpGet("user/posted/{id}/{feedUserId}")]
        [AllowAnonymous]
        public IActionResult GetPostsByUser(int id, int feedUserId)
        {
            return Ok(_postService.GetPostsByUser(id,feedUserId));
        }

        [HttpPost("like/{postId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> LikePost(int postId, int userId)
        {
            var newLikes = await _postService.UserLikePost(postId, userId);
            await _hub.Clients.All.SendAsync("like-" + postId,newLikes);
            return Ok(newLikes);
        }
        [HttpPost("comment")]
        [AllowAnonymous]
        public async Task<IActionResult> CommentPost([FromBody] CommentDto commentDto)
        {
            return Ok(await _postService.PostComment(commentDto.PostId, commentDto.UserId, commentDto.CommentText));
        }
        [HttpGet("share/{postId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> SharePost(int postId, int userId)
        {
            return Ok(await _postService.PostShare(postId, userId));
        }
        [HttpDelete("/{postId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeletePost(int postId, int userId)
        {
            await _postService.DeletePost(postId, userId);
            return NoContent();
        }
        [HttpGet("comment/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostComments(int id)
        {
            return Ok(await _postService.GetPostComments(id));

        }
    }
}
