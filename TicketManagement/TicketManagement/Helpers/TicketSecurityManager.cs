using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketManagement.Common.Algorithm;


namespace TicketManagement.Helpers
{
    public class TicketSecurityManager
    {
        private static readonly int _expirationMinutes = 10080;

        public static string[] SplitToken(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            AesAlgorithm aesAlgorithm = new AesAlgorithm();
            var decryptkey = aesAlgorithm.DecryptFromBase64String(key);
            string[] parts = decryptkey.Split(new char[] { ':' });
            return parts;
        }

        public static Int16 IsTokenValid(string[] parts, string receivedtoken, string storedtoken)
        {
            if (!string.Equals(receivedtoken, storedtoken, StringComparison.Ordinal))
            {
                return 1;
            }

            long ticks = long.Parse(parts[0]);
            string userid = parts[1];
            DateTime timeStamp = new DateTime(ticks);
            bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
            if (expired)
            {
                return 2;
            }

            return 0;
        }
    }
}