// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Renren.Components.Caching
{
    /// <summary>
    /// Represents the task to start scavenging items in a <see cref="CacheManager"/>.
    /// </summary>
    public class ScavengerTask
    {
        private readonly int maximumElementsInCacheBeforeScavenging;
        private readonly int numberToRemoveWhenScavenging;
        private ICacheOperations cacheOperations;

        /// <summary>
        /// Initialize a new instance of the <see cref="ScavengerTask"/> with a <see cref="CacheManager"/> name, the <see cref="CacheCapacityScavengingPolicy"/> and the <see cref="ICacheOperations"/>.
        /// </summary>
        /// <param name="numberToRemoveWhenScavenging">The number of items that should be removed from the cache when scavenging.</param>
        /// <param name="maximumElementsInCacheBeforeScavenging"></param>
        /// <param name="cacheOperations">The <see cref="ICacheOperations"/> to perform.</param>
        /// <param name="instrumentationProvider">An instrumentation provider.</param>
        public ScavengerTask(int numberToRemoveWhenScavenging,
                               int maximumElementsInCacheBeforeScavenging,
                               ICacheOperations cacheOperations)
        {
            this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
            this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            this.cacheOperations = cacheOperations;
        }

        /// <summary>
        /// Performs the scavenging.
        /// </summary>
        public async Task DoScavenging()
        {
            if (NumberOfItemsToBeScavenged == 0) return;

            if (await IsScavengingNeeded())
            {
                IDictionary<string,CacheItem> liveCacheRepresentation = await cacheOperations.CurrentCacheState();
                await ResetScavengingFlagInCacheItems(liveCacheRepresentation);
                var orderCache = liveCacheRepresentation.OrderBy(item => item.Value, new PriorityDateComparer());
                await RemoveScavengableItems(orderCache);
            }
        }

        private static async Task ResetScavengingFlagInCacheItems(IDictionary<string,CacheItem> liveCacheRepresentation)
        {
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                using (var releaser = await cacheItem.ItemLock.LockAsync())
                {
                    cacheItem.MakeEligibleForScavenging();
                }
            }
        }

        internal int NumberOfItemsToBeScavenged
        {
            get { return this.numberToRemoveWhenScavenging; }
        }

        private async Task RemoveScavengableItems(IEnumerable<KeyValuePair<string, CacheItem>> scavengableItems)
        {
            int scavengedItemCount = 0;
            foreach (KeyValuePair<string, CacheItem> scavengableItem in scavengableItems)
            {
                bool wasRemoved = await RemoveItemFromCache(scavengableItem.Value);
                if (wasRemoved)
                {
                    scavengedItemCount++;
                    if (scavengedItemCount == NumberOfItemsToBeScavenged)
                    {
                        break;
                    }
                }
            }
        }

        private async Task<bool> RemoveItemFromCache(CacheItem itemToRemove)
        {
            using (var releaser = await itemToRemove.ItemLock.LockAsync())
            {
                if (itemToRemove.EligibleForScavenging)
                {
                    try
                    {
                        await cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Scavenged);
                        return true;
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
            }

            return false;
        }

        internal async Task<bool> IsScavengingNeeded()
        {
            int curCount = await cacheOperations.GetCount();
            return curCount > this.maximumElementsInCacheBeforeScavenging;
        }
    }
}
