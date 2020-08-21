using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace TicketManagement.Concrete.CacheLibrary
{
    public static class CacheHelper
    {
        public static void AddToCacheWithExpiration(string key, object cacheObject, DateTime absoluteExpiration)
        {
            HttpRuntime.Cache.Insert(key, cacheObject, null, absoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        public static void AddToCacheWithNoExpiration(string key, object cacheObject)
        {
            HttpRuntime.Cache.Insert(key, cacheObject, null, DateTime.Now.AddDays(365), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        public static object GetStoreCachebyKey(string key)
        {
            if (HttpRuntime.Cache[key] != null)
            {
                // -> Found in cache AND we are not forcing update
                return HttpRuntime.Cache.Get(key);
            }
            return null;
        }

        public static bool CheckExists(string key)
        {
            if (HttpRuntime.Cache.Get(key) == null)
            {
                return false;
            }

            return true;
        }

        public static object RemoveKey(string key)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                return null;
            }
            else
            {
                return HttpRuntime.Cache.Remove(key);
            }
        }

        public static void RemoveallKeys()
        {
            foreach (System.Collections.DictionaryEntry entry in HttpRuntime.Cache)
            {
                HttpRuntime.Cache.Remove((string)entry.Key);
            }
        }

        public static List<string> ActiveallKeys()
        {
            List<string> listofKey = new List<string>();
            foreach (System.Collections.DictionaryEntry entry in HttpRuntime.Cache)
            {
                listofKey.Add(entry.Key.ToString());
            }
     
            return listofKey;
        }

        public static int TotalCachecount()
        {
            return HttpRuntime.Cache.Count;
        }
    }
}
