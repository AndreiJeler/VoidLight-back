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
    /// Posts controller responsible for posts operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IHubContext<PostsHub> _hub;


        public PostsController(IPostService postService, IHubContext<PostsHub> hub)
        {
            _postService = postService;
            _hub = hub;
        }

        /// <summary>
        /// This GET method looks for the posts related to a specific game
        /// </summary>
        /// <param name="id">The ID of the game</param>
        /// <param name="userId">The ID of the user for the like feature</param>
        /// <returns>The list of posts related to the game</returns>
        [HttpGet("game/{id}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGamePosts(int id, int userId)
        {
            return Ok(await _postService.GetGamePosts(id, userId));
        }

        /// <summary>
        /// This GET method looks for the posts of a specific user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The list of posts</returns>
        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsForUser(int id)
        {
            return Ok(await _postService.GetPostsForUserFeed(id));
        }

        /// <summary>
        /// This GET method looks for posts related to a specific publisher
        /// </summary>
        /// <param name="id">The ID of the publisher</param>
        /// <returns>The list of posts</returns>
        [HttpGet("publisher/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsForPublisher(int id)
        {
            return Ok(await _postService.GetGamePublisherPosts(id));
        }

        /// <summary>
        /// This POST method adds a post to the database
        /// </summary>
        /// <returns>The added post</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddPost()
        {
            await _hub.Clients.All.SendAsync("new", "New post");
            return Ok(await _postService.AddPost(Request.Form["post"][0], Request.Form.Files));
        }

        /// <summary>
        /// This GET method orders the posts by user
        /// </summary>
        /// <param name="id">The ID of the searched user</param>
        /// <param name="feedUserId">The ID of the authenticated user</param>
        /// <returns>The list of posts</returns>
        [HttpGet("user/posted/{id}/{feedUserId}")]
        [AllowAnonymous]
        public IActionResult GetPostsByUser(int id, int feedUserId)
        {
            return Ok(_postService.GetPostsByUser(id,feedUserId));
        }

        /// <summary>
        /// This POST method marks a post as liked for a user
        /// </summary>
        /// <param name="postId">The post's ID</param>
        /// <param name="userId">The user's ID</param>
        /// <returns>The number of likes</returns>
        [HttpPost("like/{postId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> LikePost(int postId, int userId)
        {
            var newLikes = await _postService.UserLikePost(postId, userId);
            await _hub.Clients.All.SendAsync("like-" + postId,newLikes);
            return Ok(newLikes);
        }

        /// <summary>
        /// This POST method adds a comment to a post
        /// </summary>
        /// <param name="commentDto">The comment's dto</param>
        /// <returns>The comment</returns>
        [HttpPost("comment")]
        [AllowAnonymous]
        public async Task<IActionResult> CommentPost([FromBody] CommentDto commentDto)
        {
            return Ok(await _postService.PostComment(commentDto.PostId, commentDto.UserId, commentDto.CommentText));
        }

        /// <summary>
        /// This GET method shares a post on another user's feed
        /// </summary>
        /// <param name="postId">The post's ID</param>
        /// <param name="userId">The user's ID</param>
        /// <returns>The shared post</returns>
        [HttpGet("share/{postId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> SharePost(int postId, int userId)
        {
            return Ok(await _postService.PostShare(postId, userId));
        }

        /// <summary>
        /// This DELETE method deletes a post from the database
        /// </summary>
        /// <param name="postId">The post's ID</param>
        /// <param name="userId">The user's ID which the post belongs to</param>
        /// <returns>Nothing</returns>
        [HttpDelete("/{postId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeletePost(int postId, int userId)
        {
            await _postService.DeletePost(postId, userId);
            return NoContent();
        }

        /// <summary>
        /// This GET method looks for a post's comments
        /// </summary>
        /// <param name="id">The post's ID</param>
        /// <returns>The list of comments</returns>
        [HttpGet("comment/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostComments(int id)
        {
            return Ok(await _postService.GetPostComments(id));
        }
    }
}
