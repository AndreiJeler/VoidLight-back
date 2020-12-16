using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;

namespace VoidLight.Data.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ConvertEntityToDto(PostComment comment)
        {
            return new CommentDto()
            {
                CommentText = comment.Text,
                PostId = comment.PostId,
                UserId = comment.UserId,
                User = UserMapper.ConvertEntityToDto(comment.User)
            };
        }
    }
}
