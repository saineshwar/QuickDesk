namespace TicketManagement.Concrete.CacheLibrary
{
    public static class MaskingHelper
    {
        // "123456789".MaskFront results in "****56789"
        public static string MaskFront(this string str, int len, char c)
        {
            var strArray = str.ToCharArray();

            for (var i = 0; i < len; i++)
            {
                if (i < strArray.Length)
                {
                    strArray[i] = c;
                }
                else
                {
                    break;
                }
            }

            return string.Join("", strArray);
        }

        // "123456789".MaskBack results in "12345****"
        public static string MaskBack(this string str, int len, char c)
        {
            var strArray = str.ToCharArray();

            var tracker = strArray.Length - 1;
            for (var i = 0; i < len; i++)
            {
                if (tracker > -1)
                {
                    strArray[tracker] = c;
                    tracker--;
                }
                else
                {
                    break;
                }
            }

            return string.Join("", strArray);
        }
    }
}