using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Common
{
    public class GenerateHashSha512
    {
        public static string Sha512(string hashpassword, string salt)
        {
            string saltAndPwd = String.Concat(hashpassword, salt);
            var bytes = System.Text.Encoding.UTF8.GetBytes(saltAndPwd);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                {
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                }
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
