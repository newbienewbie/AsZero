using System;
using System.Security.Cryptography;
using System.Text;

namespace AsZero.Core.Services.Auth
{
    internal class DefaultPasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// hard-coded KEY
        /// </summary>
        private const string KEY = "2add3d1b-43d6-402b-8948-b279abe1d7d2";


        /// <summary>
        /// 为明文密码生成hash  
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string ComputeHash(string plaintext, string salt)
        {
            using (var crypto = new SHA1CryptoServiceProvider())
            {
                var msg = Encoding.UTF8.GetBytes($"{plaintext}/{KEY}/{salt}");
                var encrypted = crypto.ComputeHash(msg);
                return Convert.ToBase64String(encrypted);
            }
        }
    }

}
