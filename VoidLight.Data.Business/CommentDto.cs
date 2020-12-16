using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class CommentDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public string CommentText { get; set; }
    }
}
