using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business.Discord;

namespace VoidLight.Business.Services.Contracts
{
    public interface IDiscordService
    {
        public Task<string> DecodeAuthenticationCode(string code);
        public Task<DiscordRetrieveUserResponse> DecodeToken(string token);

    }
}
