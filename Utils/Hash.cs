using System;
using System.Security.Cryptography;
using System.Text;

namespace A23017_Cloud.Utils
{
    internal class Hash
    {
        public static string HashString(string payload)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }
    }
}