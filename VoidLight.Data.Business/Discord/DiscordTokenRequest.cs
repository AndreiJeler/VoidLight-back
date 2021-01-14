using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business.Discord
{
    public class DiscordTokenRequest
    {
        public string client_id;
        public string client_secret;
        public string grant_type;
        public string code;
        public string redirect_uri;
        public string scope;

        public DiscordTokenRequest(string clientid, string clientsecret, string newcode, string redirecturi, string sco)
        {
            client_id = clientid;
            client_secret = clientsecret;
            grant_type = "auhorization_code";
            code = newcode;
            redirect_uri = redirecturi;
            scope = sco;
        }
    }
}
