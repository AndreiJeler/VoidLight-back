using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class PostDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Game { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }
        public ICollection<string> Contents { get; set; }
        public string Username { get; set; }
        public string AvatarPath { get; set; }
        public int UserId { get; set; }
        public bool IsLiked {get; set;}
        public IEnumerable<CommentDto> Comments { get; set; }
        public string OriginalUser { get; set; }
        public string OriginalUserAvatar { get; set; }
        public bool IsShared { get; set; }


    }

    public class PostDtoComparer : IEqualityComparer<PostDto>
    {
        public bool Equals(PostDto x, PostDto y)
        {
            return x.Id.Equals(y.Id) && x.UserId.Equals(y.UserId);
        }

        public int GetHashCode(PostDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
