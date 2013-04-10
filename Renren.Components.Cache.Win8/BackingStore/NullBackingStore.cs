
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Renren.Components.Caching.BackingStore
{
    /// <summary>
    /// This class is used when no backing store is needed to support the caching storage policy.
    /// Its job is to provide an implementation of a backing store that does nothing, merely enabling
    /// the cache to provide a strictly in-memory cache.
    /// </summary>
	public class NullBackingStore : IBackingStore
    {   
        /// <summary>
        /// Always returns 0
        /// </summary>
        public async Task<int> GetCount()
        {
            await Task.Delay(0);
            return 0;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="newCacheItem">Not used</param>
        public async Task Add(CacheItem newCacheItem,byte[] value)
        {
            await Task.Delay(0);
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="key">Not used</param>
        public async Task Remove(string key)
        {
            await Task.Delay(0);
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="key">Not used</param>
        /// <param name="timestamp">Not used</param>
        public async Task UpdateLastAccessedTime(string key, DateTime timestamp)
        {
            await Task.Delay(0);
        }

        /// <summary>
        /// Not used
        /// </summary>
        public async Task Flush()
        {
            await Task.Delay(0);
        }

        /// <summary>
        /// Always returns an empty hash table.
        /// </summary>
        /// <returns>Empty hash table</returns>
        public async Task<IDictionary<string, CacheItem>> Load()
        {
            await Task.Delay(0);
            return null;
        }

        /// <summary>
        /// Empty dispose implementation
        /// </summary>
        public void Dispose()
        {
            
        }

        public async Task<CacheItem> GetCacheItem(string key)
        {
            await Task.Delay(0);
            return null;
        }

        public async Task<byte[]> GetValue(string key)
        {
            await Task.Delay(0);
            return null;
        }


        public async Task Initialize()
        {
            await Task.Delay(0);
        }
    }
}
