using System;
using Microsoft.AspNetCore.Identity;

namespace AuthMicroservice.Models
{
    public class User : IdentityUser<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public bool VerifyPassword(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != PasswordHash[i])
                    return false;
            }
            return true;
        }
    }

}

