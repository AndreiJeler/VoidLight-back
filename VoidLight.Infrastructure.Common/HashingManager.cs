using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VoidLight.Infrastructure.Common
{
    public static class HashingManager
    {
        private const string PASSWORD_SALT = "QwErTy1@3$5";

        public static string GetHashedPassword(string password)
        {
            using var sha512 = SHA512.Create();

            var bytes = sha512.ComputeHash(
                Encoding.UTF8.GetBytes(password + PASSWORD_SALT)
            );

            return bytes
                .Select(x => x.ToString("X2"))
                .Aggregate((acc, x) => acc + x);
        }
    }
}
