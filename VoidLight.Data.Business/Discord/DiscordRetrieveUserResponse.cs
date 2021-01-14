using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business.Discord
{
    public class DiscordRetrieveUserResponse
    {
        public string id;
        public string username;
        public string discriminator;
        public string email;

        public DiscordRetrieveUserResponse(string id,string username, string discriminator, string email)
        {
            this.id = id;
            this.username = username;
            this.discriminator = discriminator;
            this.email = email;
        }
    }
}
