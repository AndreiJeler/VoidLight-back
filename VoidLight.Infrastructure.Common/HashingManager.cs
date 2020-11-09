using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VoidLight.Infrastructure.Common
{
    public static class HashingManager
    {

        public static string GetHashedPassword(string password, string salt)
        {
            using var sha512 = SHA512.Create();

            var bytes = sha512.ComputeHash(
                Encoding.UTF8.GetBytes(password + salt)
            );

            return bytes
                .Select(x => x.ToString("X2"))
                .Aggregate((acc, x) => acc + x);
        }
    }
}
