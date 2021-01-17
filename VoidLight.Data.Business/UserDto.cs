using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarPath { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string PlayedGame { get; set; }
        public int Age { get; set; }
    }

    public class UserDtoComparer : IEqualityComparer<UserDto>
    {
        public bool Equals(UserDto x, UserDto y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(UserDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
