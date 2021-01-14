using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SteamKey { get; set; }
        public string DiscordClientId { get; set; }
        public string DiscordClientSecret { get; set; }
        public string WebFrontEndUrl { get; set; }
        public string AppUrl { get; set; }

    }
}
