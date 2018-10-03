using System;
using System.Security.Cryptography;
using System.Text;
using Oxite.Model;

namespace Ecard.Models
{
    public static class TokenKeyExtensions
    {
        public static void GenTokenKey(this ITokenKey tokenKey)
        {
            var key = Guid.NewGuid().ToString("N");
            byte[] salted = Encoding.UTF8.GetBytes(key);

            SHA256 hasher = new SHA256Managed();
            byte[] hashed = hasher.ComputeHash(salted);

            tokenKey.TokenKey = Convert.ToBase64String(hashed);
        }
    }
}