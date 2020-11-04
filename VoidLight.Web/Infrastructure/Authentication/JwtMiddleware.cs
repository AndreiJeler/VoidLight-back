using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Infrastructure.Common;

namespace VoidLight.Web.Infrastructure.Authentication
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate requestDelegate, IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
            this._next = requestDelegate;
        }

        public async Task Invoke(HttpContext context, IUserService userDataService)
        {
            var token = context.Request.Headers[Constants.AUTHORIZATION_HEADER].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachUserToContext(context, userDataService, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == Constants.AUTHORIZATION_ID).Value);

                var user = userService.FindById(userId).Result;

                var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("roleId", user.RoleId.ToString())
                });


                context.User = new ClaimsPrincipal(claimsIdentity);


            }
            catch
            {
            }
        }
    }
}
