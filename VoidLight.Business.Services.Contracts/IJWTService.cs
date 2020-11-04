using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Entities;

namespace VoidLight.Business.Services.Contracts
{
    public interface IJWTService
    {
        string GenerateAuthenticationJWT(User user);
        string GenerateRegisterJWT(User user);
        string DecodeRegisterToken(string token);
        int DecodeAuthenticationJWT(string token);
    }
}
