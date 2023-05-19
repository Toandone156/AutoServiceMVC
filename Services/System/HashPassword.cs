using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System.Text;

namespace AutoServiceMVC.Services.System
{
    public interface IHashPassword
    {
        public string GetHashPassword(string password);
    }
    public class HashPassword : IHashPassword
    {
        private readonly string SALT;

        public HashPassword(IOptionsMonitor<AppSettings> monitor)
        {
            SALT = monitor.CurrentValue.Salt;
        }

        public string GetHashPassword(string password)
        {
            byte[] salt = Encoding.Default.GetBytes(SALT);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
