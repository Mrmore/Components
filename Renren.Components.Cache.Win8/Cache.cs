// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Renren.Components.Async;
using Renren.Components.Tools;

namespace Renren.Components.Caching
{
    /// <summary>
    /// The real worker of the block. The Cache class is the traffic cop that prevents 
    /// resource contention among the different threads in the system. It also will act
    /// as the remoting gateway when that feature is added to the cache.
    /// </summary>	
	public class Cache : ICacheOperations, IDisposable
    {
        public readonly AsyncReaderWriterLock CacheRWLock = new AsyncReaderWriterLock();
        private IDictionary<string,CacheItem> inMemoryCache = new Dictionary<string,CacheItem>();
        private IBackingStore backingStore;
        
		/// <summary>
		/// Initialzie a new instance of a <see cref="Cache"/> class with a backing store, and scavenging policy.
		/// </summary>
		/// <param name="backingStore">The cache backing store.</param>
		/// <param name="instrumentationProvider">The instrumentation provider.</param>
		public Cache(IBackingStore backingStore)
        {
            Guarder.NotNull(backingStore, "backingStore");
            this.backingStore = backingStore;       
        }

        /// <summary>
        /// Initialzie Cache
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            await backingStore.Initialize();
            using (var writeReleaser = await CacheRWLock.WriterLockAsync())
            {
                IDictionary<string, CacheItem> loadCache = await backingStore.Load();
                if (loadCache != null)
                    inMemoryCache = loadCache;
            }
        }

        /// <summary>
        /// Gets the count of <see cref="CacheItem"/> objects.
        /// </summary>
		/// <value>
		/// The count of <see cref="CacheItem"/> objects.
		/// </value>
		public async Task<int> GetCount()
        {
            int count = 0;
            using(var releaser = await CacheRWLock.ReaderLockAsync())
            {
                count = inMemoryCache.Count;
            }
            return count;
        }

		/// <summary>
		/// Gets the current cache clone.
		/// </summary>
		/// <returns>
		/// The current cache clone.
		/// </returns>
		public async Task<IDictionary<string,CacheItem>> CurrentCacheState()
		{
            IDictionary<string,CacheItem> cache = null;
            using(var releaser = await CacheRWLock.ReaderLockAsync())
            {
                cache = new Dictionary<string,CacheItem>(inMemoryCache);
            }
            return cache;
		}

		/// <summary>
		/// Determines if a particular key is contained in the cache.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <returns>
		/// <see langword="true"/> if the key is contained in the cache; otherwise, <see langword="false"/>.
		/// </returns>
        public async Task<bool> Contains(string key)
        {
            Guarder.NotNullOrEmpty(key, "key");

            bool contains;
            using (var releaser = await CacheRWLock.ReaderLockAsync())
            {
                contains = inMemoryCache.ContainsKey(key);
            }
            return contains;
        }

		/// <summary>
		/// Add a new keyed object to the cache.
		/// </summary>
		/// <param name="key">The key of the object.</param>
		/// <param name="value">The object to add.</param>
        public async Task Add(string key, byte[] value)
        {
            await Add(key, value, CacheItemPriority.Normal, null);
        }

		/// <summary>
		/// Add a new keyed object to the cache.
		/// </summary>
		/// <param name="key">The key of the object.</param>
		/// <param name="value">The object to add.</param>
		/// <param name="scavengingPriority">One of the <see cref="CacheItemPriority"/> values.</param>
		/// <param name="refreshAction">An <see cref="ICacheItemRefreshAction"/> object.</param>
		/// <param name="expirations">An array of <see cref="ICacheItemExpiration"/> objects.</param>
        public async Task Add(string key, byte[] value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
        {
            Guarder.NotNullOrEmpty(key, "key");
            using (var writeReleaser = await CacheRWLock.WriterLockAsync())
            {
                CacheItem cacheItem = null;
                if (inMemoryCache.ContainsKey(key))
                {
                    cacheItem = inMemoryCache[key];
                    cacheItem.Replace(refreshAction, scavengingPriority, expirations);
                }
                else
                {
                    cacheItem = new CacheItem(key, scavengingPriority, refreshAction, expirations);
                    inMemoryCache.Add(key, cacheItem);
                }
                cacheItem.TouchedByUserAction(true);
                await backingStore.Add(cacheItem, value);
            }
        }

		/// <summary>
		/// Remove an item from the cache by key.
		/// </summary>
		/// <param name="key">The key of the item to remove.</param>
        public async Task Remove(string key)
        {
            await Remove(key, CacheItemRemovedReason.Removed);
        }

		/// <summary>
		/// Remove an item from the cache by key.
		/// </summary>
		/// <param name="key">The key of the item to remove.</param>
		/// <param name="removalReason">One of the <see cref="CacheItemRemovedReason"/> values.</param>
        public async Task Remove(string key, CacheItemRemovedReason removalReason)
        {
            Guarder.NotNullOrEmpty(key, "key");
            using (var writeReleaser = await CacheRWLock.WriterLockAsync())
            {
                CacheItem cacheItem = null;
                if (inMemoryCache.ContainsKey(key))
                {
                    cacheItem = inMemoryCache[key];
                    inMemoryCache.Remove(key);
                    cacheItem.TouchedByUserAction(true);
                    await backingStore.Remove(key);
                    RefreshActionInvoker.InvokeRefreshAction(cacheItem, removalReason);
                }
            }
        }

		
        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The key to remove.</param>
		/// <param name="removalReason">One of the <see cref="CacheItemRemovedReason"/> values.</param>
		/// <remarks>
		/// This seemingly redundant method is here to be called through the ICacheOperations 
		/// interface. I put this in place to break any dependency from any other class onto 
		/// the Cache class
		/// </remarks>
        public async Task RemoveItemFromCache(string key, CacheItemRemovedReason removalReason)
        {
            await Remove(key, removalReason);
        }

        /// <summary>
        /// Get the object from the cache for the key.
        /// </summary>
        /// <param name="key">
		/// The key whose value to get.
		/// </param>
        /// <returns>
		/// The value associated with the specified key. 
		/// </returns>
		public async Task<byte[]> GetData(string key)
        {
            Guarder.NotNullOrEmpty(key, "key");
            byte[] data = null;
            using (var readReleaser = await CacheRWLock.ReaderLockAsync())
            {
                CacheItem cacheItem = null;
                if (inMemoryCache.ContainsKey(key))
                {
                    cacheItem = inMemoryCache[key];
                    if (!cacheItem.HasExpired())
                    {
                        data = await backingStore.GetValue(key);
                        await backingStore.UpdateLastAccessedTime(cacheItem.Key, DateTime.Now); // Does exception safety matter here?
                        cacheItem.TouchedByUserAction(false);
                    }
                    else
                    {
                        inMemoryCache.Remove(key);
                        cacheItem.TouchedByUserAction(true);
                        await backingStore.Remove(key);
                        RefreshActionInvoker.InvokeRefreshAction(cacheItem, CacheItemRemovedReason.Expired);
                    }
                }
            }
            return data;
        }

		/// <summary>
		/// Flush the cache.
		/// </summary>
		/// <remarks>
        /// There may still be thread safety issues in this class with respect to cacheItemExpirations
        /// and scavenging, but I really doubt that either of those will be happening while
        /// a Flush is in progress. It seems that the most likely scenario for a flush
        /// to be called is at the very start of a program, or when absolutely nothing else
        /// is going on. Calling flush in the middle of an application would seem to be
        /// an "interesting" thing to do in normal circumstances.
        /// </remarks>
		public async Task Flush()
        {
            using (var writeReleaser = await CacheRWLock.WriterLockAsync())
            {
                inMemoryCache.Clear();
                await backingStore.Flush();
            }           
        }

		/// <summary>
		/// Dispose of the backing store before garbage collection.
		/// </summary>
        ~Cache()
        {
            Dispose(false);
        }

		/// <summary>
		/// Dispose of the backing store before garbage collection.
		/// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>
		/// Dispose of the backing store before garbage collection.
		/// </summary>
        /// <param name="disposing">
		/// <see langword="true"/> if disposing; otherwise, <see langword="false"/>.
		/// </param>
		protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                backingStore.Dispose();
                backingStore = null;
            }
        }
    }
}
