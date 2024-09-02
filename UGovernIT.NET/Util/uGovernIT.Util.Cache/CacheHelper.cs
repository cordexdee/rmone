using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace uGovernIT.Util.Cache
{
    public class CacheHelper<T>
    {
        private static BaseCacheManager<T> manager = null;
        public static void AddCacheInstance() 
        {
            manager = new BaseCacheManager<T>(
                  new CacheManagerConfiguration()
                      .Builder
                      .WithSystemRuntimeCacheHandle()
                      .EnableStatistics()
                      .Build());
        }

        public static T AddOrUpdate(string key, T value)
        {
            if (manager == null)
                return default(T);


            return manager.AddOrUpdate(key, "default" ,value, _ => value);
        }

        public static bool IsExists(string key)
        {
            if (manager == null)
                return false;

            return manager.Exists(key, "default");
        }

        public static T Get(string key)
        {
            if (manager == null)
                return default(T);

            return manager.Exists(key, "default") ? manager.Get(key, "default") : default(T);
        }

        public static bool Delete(string key)
        {
            try
            {
                if (manager == null)
                    return false;

                return manager.Exists(key, "default") && manager.Remove(key, "default");
            }
            catch
            {
                return false;
            }
        }

        public static bool ClearWithRegion(string region)
        {
            try
            {
                if (manager == null)
                    return false;

                manager.ClearRegion(region);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T AddOrUpdate(string key, string region, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return value;

            Task.Run(() =>
            {
                if (manager == null)
                    return;

                if (value == null)
                    value = default(T);

                if (value == null)
                {
                    Delete(key, region);
                    return;
                }
                manager.AddOrUpdate(key, region, value, _ => value);
            });

            return value;
        }

        public static bool IsExists(string key, string region)
        {
            if (manager == null)
                return false;

            return manager.Exists(key, region);
        }

        public static T Get(string key, string region)
        {
            if (manager == null)
                return default(T);

            return manager.Exists(key, region) ? manager.Get(key, region) : default(T);
        }

        public static bool Delete(string key, string region)
        {
            try
            {
                if (manager == null)
                    return false;

                return manager.Exists(key, region) && manager.Remove(key, region);
            }
            catch
            {
                return false;
            }
        }

        public static long GetCount(string region)
        {
            var handler = manager.CacheHandles.FirstOrDefault();
            if (handler != null)
            {
               return handler.Stats.GetStatistic(CacheManager.Core.Internal.CacheStatsCounterType.Items, region);
            }

            return -1;
        }

        public static bool Clear()
        {
            try
            {
                manager.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

