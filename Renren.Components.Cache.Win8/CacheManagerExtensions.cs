using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
    public static class CacheManagerExtensions
    {
        public static async Task Add<T>(this ICacheManager manager, string key, T value)
        {
            Guarder.NotNull(manager, "manager");
            Guarder.NotNullOrEmpty(key, "key");
            byte[] data = Encoding.UTF8.GetBytes(value.SerializeAsJsonString(typeof(T)));
            await manager.Add(key, data);
        }

        public static async Task Add<T>(this ICacheManager manager, string key, T value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
        {
            Guarder.NotNull(manager, "manager");
            Guarder.NotNullOrEmpty(key, "key");
            byte[] data = Encoding.UTF8.GetBytes(value.SerializeAsJsonString(typeof(T)));
            await manager.Add(key, data, scavengingPriority, refreshAction, expirations);
        }

        public static async Task<T> GetData<T>(this ICacheManager manager, string key)
        {
            Guarder.NotNull(manager, "manager");
            Guarder.NotNullOrEmpty(key, "key");
            byte[] memData = await manager.GetData(key);
            if (memData == null)
                return default(T);
            var stream = new MemoryStream(memData);
            try
            {
                object rtObject = stream.DeserializeJsonStreamAs(typeof(T));
                return (T)rtObject;
            }
            catch
            {
                throw new Exception("the" + key + "of data's type is not" + typeof(T).ToString());
            }           
        }
    }
}
