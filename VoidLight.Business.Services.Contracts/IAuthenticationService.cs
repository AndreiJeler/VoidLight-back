using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business.Authentication;

namespace VoidLight.Business.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResponseDto> Authenticate(AuthenticateRequestDto model);
        Task<AuthenticateResponseDto> GetUser(string token);
    }
}
