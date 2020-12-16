using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;

namespace VoidLight.Business.Services.Contracts
{
    public interface IPostService
    {
        public Task<ICollection<PostDto>> GetPostsForUserFeed(int userId);
        public Task<ICollection<PostDto>> GetGamePosts(int gameId);
        public Task<ICollection<PostDto>> GetGamePublisherPosts(int gamePublisherId);
        public ICollection<PostDto> GetPostsByUser(int userId);
        public Task<PostDto> AddPost(PostDto post);
        public Task<int> UserLikePost(int postId, int userId);
        public Task<CommentDto> PostComment(int postId, int userId, string commentText);
        public Task<Post> FindPost(int postId);
        public Task<PostDto> PostShare(int postId, int userId);
    }
}
