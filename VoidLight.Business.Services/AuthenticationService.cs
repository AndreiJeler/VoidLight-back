using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business.Authentication;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;
using VoidLight.Infrastructure.Common.Exceptions;

namespace VoidLight.Business.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly VoidLightDbContext _context;
        private readonly IJWTService _jwtService;

        public AuthenticationService(VoidLightDbContext context, IJWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthenticateResponseDto> Authenticate(AuthenticateRequestDto model)
        {
            var user = await _context.Users.Include(user => user.Role).FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == HashingManager.GetHashedPassword(model.Password));

            if (user == null)
            {
                throw new AuthenticationException("Username or password is incorrect");
            }

            if (!user.IsActivated)
            {
                throw new AuthenticationException("Please activate your account first!");
            }

            if ((user.WasPasswordChanged && user.WasPasswordForgotten) || (!user.WasPasswordChanged && !user.WasPasswordForgotten))
            {
                user.WasPasswordChanged = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                var token = _jwtService.GenerateAuthenticationJWT(user);
                var authenticateResponseDto = new AuthenticateResponseDto(user, token);

                return authenticateResponseDto ?? throw new AuthenticationException("Username or password is incorrect");
            }

            throw new AuthenticationException("Username or password is incorrect");
        }


        public async Task<AuthenticateResponseDto> GetUser(string token)
        {
            var user = await _context.Users.Include(user => user.Role).FirstAsync(user => user.Id == _jwtService.DecodeAuthenticationJWT(token));
            return new AuthenticateResponseDto(user, token);
        }


    }
}
