using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidLight.Data.Entities;

namespace VoidLight.Web.Infrastructure.Authorization
{
    public class AccountCustomRequirement : IAuthorizationRequirement
    {
        public RoleType RequiredRole { get; set; }

        public AccountCustomRequirement(RoleType role)
        {
            RequiredRole = role;
        }

    }
}
