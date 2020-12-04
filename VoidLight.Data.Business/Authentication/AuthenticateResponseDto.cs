using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;

namespace VoidLight.Data.Business.Authentication
{
    public class AuthenticateResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public string Role { get; set; }
        public string PlayedGame { get; set; }


        public AuthenticateResponseDto(User user, string token)
        {
            Id = user.Id;
            FullName = user.FullName;
            Username = user.Username;
            Nickname = user.Nickname;
            AvatarPath = Constants.APP_URL + user.AvatarPath;
            Email = user.Email;
            Token = token;
            Role = user.Role.Name;
        }
    }
}
