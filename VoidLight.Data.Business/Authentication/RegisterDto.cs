using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business.Authentication
{
    public class RegisterDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int LoginService { get; set; }
        public string LoginToken { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime AccountCreated { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
    }
}
