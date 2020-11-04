using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidLight.Data.Entities;

namespace VoidLight.Web.Infrastructure.Authorization
{
    public class AuthorizeUserCustomAttribute : AuthorizeAttribute
    {
        public AuthorizeUserCustomAttribute(RoleType role) { Role = role; }

        public RoleType Role
        {
            get => (RoleType)Enum.Parse(typeof(UserRole), Policy);
            set => Policy = value.ToString();
        }
    }
}
