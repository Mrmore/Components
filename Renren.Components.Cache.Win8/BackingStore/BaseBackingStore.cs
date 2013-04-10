// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Renren.Components.Caching.BackingStore
{
    /// <summary>
    /// Base class for backing stores. Contains implementations of common policies
    /// and utilities usable by all backing stores.
    /// </summary>
    public abstract class BaseBackingStore : IBackingStore, IDisposable
    {
        /// <summary>
        /// Inherited constructor
        /// </summary>
        protected BaseBackingStore()
        {
        }

        /// <summary>
        /// Finalizer for BaseBackingStore
        /// </summary>
        ~BaseBackingStore()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method for all backing stores. This implementation is sufficient for any class that does not need any finalizer behavior
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing method as used in the Dispose pattern
        /// </summary>
        /// <param name="disposing">True if called during Dispose. False if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Number of objects stored in the backing store
        /// </summary>
        public abstract Task<int> GetCount();

        /// <summary>
        /// Removes an item with the given key from the backing store
        /// </summary>
        /// <param name="key">Key to remove. Must not be null.</param>
        /// <remarks>
        /// <p>
        /// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Remove
        /// </p>
        /// </remarks>
        public async Task Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            await Remove(key.GetHashCode());
        }

        public async Task<CacheItem> GetCacheItem(string key)
        {
            if (key != null)
            {
                return await GetCacheItem(key.GetHashCode());
            }
            else
            {
                return null;
            }
        }

        public async Task<byte[]> GetValue(string key)
        {
            if (key != null)
            {
                return await GetValue(key.GetHashCode());
            }
            else
            {
                return null;
            }
        }

        protected abstract Task<CacheItem> GetCacheItem(int key);

        protected abstract Task<byte[]> GetValue(int key);

        /// <summary>
        /// Removes an item with the given storage key from the backing store.
        /// </summary>
        /// <param name="storageKey">Unique storage key for the cache item to be removed</param>
        /// <remarks>
        /// <p>
        /// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Remove
        /// </p>
        /// </remarks>
        protected abstract Task Remove(int storageKey);

        /// <summary>
        /// Updates the last accessed time for a cache item.
        /// </summary>
        /// <param name="key">Key to update</param>
        /// <param name="timestamp">Time at which item updated</param>
        /// <remarks>
        /// <p>
        /// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during UpdateLastAccessedTime
        /// </p>
        /// </remarks>
        public async Task UpdateLastAccessedTime(string key, DateTime timestamp)
        {
            if (key == null) throw new ArgumentNullException("key");

            await UpdateLastAccessedTime(key.GetHashCode(), timestamp);
        }

        /// <summary>
        /// Updates the last accessed time for a cache item referenced by this unique storage key
        /// </summary>
        /// <param name="storageKey">Unique storage key for cache item</param>
        /// <param name="timestamp">Time at which item updated</param>
        protected abstract Task UpdateLastAccessedTime(int storageKey, DateTime timestamp);

        /// <summary>
        /// Flushes all CacheItems from backing store. This method must meet the Strong Exception Safety guarantee.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Flush
        /// </p>
        /// </remarks>
        public abstract Task Flush();

        /// <summary>
        /// <p>
        /// This method is responsible for adding a CacheItem to the BackingStore. This operation must be successful 
        /// even if an item with the same key already exists. This method must also meet the exception safety guarantee
        /// and make sure that all traces of the new or old item are gone if the add fails in any way.
        /// </p> 
        /// </summary>
		/// <param name="newCacheItem">CacheItem to be added</param>
        /// <remarks>
        /// <p>
        /// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Add
        /// </p>
        /// </remarks>
        public virtual async Task Add(CacheItem newCacheItem,byte[] value)
        {
            if (newCacheItem == null && value == null)
            {
                throw new ArgumentNullException("newCacheItem");
            }

            await Remove(newCacheItem.Key);

            try
            {               
                await AddNewItem(newCacheItem.Key.GetHashCode().ToString(NumberFormatInfo.InvariantInfo), newCacheItem,value);
            }
            catch
            {            
                throw;
            }
        }

        /// <summary>
        /// Loads all CacheItems from underlying persistence mechanism.
        /// </summary>
        /// <returns>IDictionary<string,CacheItem> containing all existing CacheItems.</returns>
        /// <remarks>Exceptions thrown depend on the implementation of the underlying database.</remarks>
        public virtual async Task<IDictionary<string, CacheItem>> Load()
        {
            return await LoadDataFromStore();
        }

        public abstract Task Initialize();
        /// <summary>
        /// Adds new item to persistence store
        /// </summary>
        /// <param name="storageKey">Unique key for cache item</param>
        /// <param name="newItem">Item to be added to cache. May not be null.</param>
        protected abstract Task AddNewItem(string storageKey, CacheItem newItem, byte[] value);

        /// <summary>
        /// Responsible for loading items from underlying persistence store. This method should do
        /// no filtering to remove expired items.
        /// </summary>
        /// <returns>Hash table of all items loaded from persistence store</returns>
        protected abstract Task<IDictionary<string, CacheItem>> LoadDataFromStore();
    }
}
