using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace uGovernIT.Util.Cache
{
    public class RedisCacheManager
    {
        // public static RedisClient redisClient = null;
        public static ConnectionMultiplexer redisClient = null;
        public static IDatabase db = null;


        private static object syncLock = new object();
        public static ConnectionMultiplexer GetPooledClient()
        {

            lock (syncLock)
            {
                if (redisClient == null)
                {
                    //redisClient = new RedisClient("localhost");
                    //redisClient = ConnectionMultiplexer.Connect("localhost");
                }
            }
            return redisClient;
        }
        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }
        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        public void SetData<T>(string key, T value)
        {
            // RedisClient redisClient = GetPooledClient();
            ConnectionMultiplexer redisClient = GetPooledClient();
            if (redisClient != null && redisClient.IsConnected)
            {
                lock (syncLock)
                {
                    db = redisClient.GetDatabase();
                    db.StringSet(key, Serialize(value));
                }
            }
        }
        public object GetData<T>(string key)
        {
            object cachevalue = null;
            ConnectionMultiplexer redisClient = GetPooledClient();
            if (redisClient != null && redisClient.IsConnected)
            {
                //cachevalue = redisClient.Get<T>(key);
                db = redisClient.GetDatabase();
                cachevalue = Deserialize<T>(db.StringGet(key));
            }

            return cachevalue;
        }
        public void ClearData(string key)
        {
            ConnectionMultiplexer redisClient = GetPooledClient();
            if (redisClient != null && redisClient.IsConnected)
            {
                //redisClient;
                db = redisClient.GetDatabase();
                db.KeyDelete(key);
            }
        }
    }
}
