using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data.Entities;

namespace VoidLight.Web.Infrastructure.Authorization
{
    public class AccountCustomRequirementHandler : AuthorizationHandler<AccountCustomRequirement>
    {
        private readonly IUserService _userService;
        public AccountCustomRequirementHandler(IUserService userService)
        {
            _userService = userService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountCustomRequirement requirement)
        {
            var userId = context.User.GetId();
            User user = _userService.FindById(userId).Result;

            if (user == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.RequiredRole == RoleType.General) context.Succeed(requirement);

            if (user.IsActivated && (context.User.GetRoleId() == (int)requirement.RequiredRole)) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
