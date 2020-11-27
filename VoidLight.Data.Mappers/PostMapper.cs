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
        public static PostDto ConvertEntityToDto(UserPost post)
        {
            return new PostDto()
            {
                Id = post.Post.Id,
                Game = post.Post.Game == null ? "No game" : post.Post.Game.Name,
                Likes = post.Post.Likes.Count,
                Text = post.Post.Text,
                Time = post.Post.Time,
                Contents = post.Post.Content == null ? new List<string>() : post.Post.Content.Select(content => content.ContentPath).ToList(),
                Username = post.User.Username,
                AvatarPath = post.User.AvatarPath,
                UserId = post.User.Id
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
