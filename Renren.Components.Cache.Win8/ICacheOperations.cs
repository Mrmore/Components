// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
	/// <summary>
	/// Represents cache operations.
	/// </summary>
    public interface ICacheOperations
    {
		/// <summary>
		/// Gets the current cache state.
		/// </summary>
		/// <returns></returns>
        Task<IDictionary<string, CacheItem>> CurrentCacheState();

		/// <summary>
		/// Removes a <see cref="CacheItem"/>.
		/// </summary>
		/// <param name="key">The key of the item to remove.</param>
		/// <param name="removalReason">One of the <see cref="CacheItemRemovedReason"/> values.</param>
        Task RemoveItemFromCache(string key, CacheItemRemovedReason removalReason);

        /// <summary>
        /// Returns the number of items contained in the cache.
        /// </summary>
        Task<int> GetCount();
    }
}
