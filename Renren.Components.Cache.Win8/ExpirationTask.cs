// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
	/// <summary>
	/// Represents a task to perform expiration on cached items.
	/// </summary>
    public class ExpirationTask
    {
        private ICacheOperations cacheOperations;

		/// <summary>
		/// Initialize an instance of the <see cref="ExpirationTask"/> class with an <see cref="ICacheOperations"/> object.
		/// </summary>
		/// <param name="cacheOperations">An <see cref="ICacheOperations"/> object.</param>
		/// <param name="instrumentationProvider">An instrumentation provider.</param>
		public ExpirationTask(ICacheOperations cacheOperations)
        {
            this.cacheOperations = cacheOperations;
        }

		/// <summary>
		/// Perform the cacheItemExpirations.
		/// </summary>
        public async Task DoExpirations()
        {
            IDictionary<string,CacheItem> liveCacheRepresentation = await cacheOperations.CurrentCacheState();
            await MarkAsExpired(liveCacheRepresentation);
            PrepareForSweep();
            int expiredItemsCount = await SweepExpiredItemsFromCache(liveCacheRepresentation);
        }

		/// <summary>
		/// Mark each <see cref="CacheItem"/> as expired. 
		/// </summary>
		/// <param name="liveCacheRepresentation">The set of <see cref="CacheItem"/> objects to expire.</param>
		/// <returns>
		/// The number of items marked.
		/// </returns>
        public virtual async Task<int> MarkAsExpired(IDictionary<string,CacheItem> liveCacheRepresentation)
        {
		    if (liveCacheRepresentation == null) throw new ArgumentNullException("liveCacheRepresentation");

            int markedCount = 0;
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                using(var releaser = await cacheItem.ItemLock.LockAsync())
                {
                    if (cacheItem.HasExpired())
                    {
                        markedCount++;
                        cacheItem.WillBeExpired = true;
                    }
                }
            }

            return markedCount;
        }

		/// <summary>
		/// Sweep and remove the <see cref="CacheItem"/>s.
		/// </summary>
		/// <param name="liveCacheRepresentation">
		/// The set of <see cref="CacheItem"/> objects to remove.
		/// </param>
		public virtual async Task<int> SweepExpiredItemsFromCache(IDictionary<string,CacheItem> liveCacheRepresentation)
        {
		    if (liveCacheRepresentation == null) throw new ArgumentNullException("liveCacheRepresentation");

			int expiredItems = 0;

            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                bool bRemoved = await RemoveItemFromCache(cacheItem);
                if (bRemoved)
					expiredItems++;
            }

			return expiredItems;
        }

		/// <summary>
		/// Prepare to sweep the <see cref="CacheItem"/>s.
		/// </summary>
        public virtual void PrepareForSweep()
        {
        }
		
		private async Task<bool> RemoveItemFromCache(CacheItem itemToRemove)
        {
			bool expired = false;

            using(var releaser = await itemToRemove.ItemLock.LockAsync())
            {
                if (itemToRemove.WillBeExpired)
                {
					try
					{
						expired = true;
						await cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Expired);
					}
					catch (Exception e)
					{
						
					}                    
                }
            }

			return expired;
        }
    }
}
