using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoidLight.Business.Services.Contracts
{
    public interface ISteamGameCollection
    {
        public Task<JToken> GetGameName(string appId);
    }
}
