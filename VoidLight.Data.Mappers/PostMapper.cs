using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;

namespace VoidLight.Data.Mappers
{
    public static class PostMapper
    {
        public static PostDto ConvertEntityToDto(UserPost post, int userId)
        {
            var isPostLiked = post.Post.Likes == null ? false : post.Post.Likes.Any(like => like.UserId == userId);
            var originalPost = post.Post.UserPosts.FirstOrDefault(up => up.IsShared == false);
            return new PostDto()
            {
                Id = post.Post.Id,
                Game = post.Post.Game == null ? "No game" : post.Post.Game.Name,
                Likes = post.Post.Likes.Count,
                Text = post.Post.Text,
                Time = post.Timestamp,
                Contents = post.Post.Content == null ? new List<string>() : post.Post.Content.Select(content => content.ContentPath).ToList(),
                Username = post.User.Username,
                AvatarPath = post.User.AvatarPath,
                UserId = post.User.Id,
                IsLiked = isPostLiked,
                //Comments = post.Post.Comments.Select(comm => CommentMapper.ConvertEntityToDto(comm)).AsEnumerable(),
                IsShared = post.IsShared,
                OriginalUser = originalPost.User.Username,
                OriginalUserAvatar = originalPost.User.AvatarPath,
                OriginalUserId = originalPost.User.Id
            };
        }

        public static PostDto ConvertEntityToDto(UserPost post, int userId, UserPost originalPost)
        {
            var isPostLiked = post.Post.Likes == null ? false : post.Post.Likes.Any(like => like.UserId == userId);
            return new PostDto()
            {
                Id = post.Post.Id,
                Game = post.Post.Game == null ? "No game" : post.Post.Game.Name,
                Likes = post.Post.Likes.Count,
                Text = post.Post.Text,
                Time = post.Timestamp,
                Contents = post.Post.Content == null ? new List<string>() : post.Post.Content.Select(content => content.ContentPath).ToList(),
                Username = post.User.Username,
                AvatarPath = post.User.AvatarPath,
                UserId = post.User.Id,
                IsLiked = isPostLiked,
                //Comments = post.Post.Comments.Select(comm => CommentMapper.ConvertEntityToDto(comm)).AsEnumerable(),
                IsShared = post.IsShared,
                OriginalUser = originalPost.User.Username,
                OriginalUserAvatar = originalPost.User.AvatarPath,
                OriginalUserId = originalPost.User.Id
            };
        }

        public static UserPost ConvertDtoToEntity(PostDto dto)
        {
            return new UserPost()
            {
                IsShared = false,
                PostId = dto.Id,
                UserId = dto.UserId
            };
        }
    }
}
