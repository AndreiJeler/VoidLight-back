using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business.Discord
{
    public class DiscordUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DiscordId { get; set; }
        public string DiscordToken { get; set; }
        public string AvatarPicture { get; set; }
    }
}
