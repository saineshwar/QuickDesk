using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TicketManagement.Common;

namespace TicketManagement.CommonData
{
    public class GenerateTicketNo
    {
        public string ApplicationNo(string category)
        {
            try
            {
                if (!string.IsNullOrEmpty(category))
                {
                    var currentDate = DateTime.Now;
                    var lastTwoDigitsOfYear = currentDate.ToString("yy");
                    var randomone = GenerateRandomStrings.RandomString(5);
                    var randomtwo = GenerateId().ToString().Substring(1, 4);
                    var mainrandom = string.Concat(randomone, randomtwo);
                    return string.Concat(category, "-", lastTwoDigitsOfYear, "-", mainrandom);
                }
                else
                {
                    return "###########################";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private long GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

    }
}