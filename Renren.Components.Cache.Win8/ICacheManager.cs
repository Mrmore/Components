// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
	/// <summary>
	/// This interface defines the contract that must be implemented by all cache managers. 
	/// </summary>
	public interface ICacheManager
	{
		/// <summary>
		/// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
		/// the new item is added. If any failure occurs during this process, the cache will not contain the item being added. 
		/// Items added with this method will be not expire, and will have a Normal <see cref="CacheItemPriority" /> priority.
		/// </summary>
		/// <param name="key">Identifier for this CacheItem</param>
		/// <param name="value">Value to be stored in cache. May be null.</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        Task Add(string key, byte[] value);

		/// <summary>
		/// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
		/// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
		/// </summary>
		/// <param name="key">Identifier for this CacheItem</param>
		/// <param name="value">Value to be stored in cache. May be null.</param>
		/// <param name="scavengingPriority">Specifies the new item's scavenging priority. 
		/// See <see cref="CacheItemPriority" /> for more information.</param>
		/// <param name="refreshAction">Object provided to allow the cache to refresh a cache item that has been expired. May be null.</param>
		/// <param name="expirations">Param array specifying the expiration policies to be applied to this item. May be null or omitted.</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        Task Add(string key, byte[] value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations);

		/// <summary>
		/// Returns true if key refers to item current stored in cache
		/// </summary>
		/// <param name="key">Key of item to check for</param>
		/// <returns>True if item referenced by key is in the cache</returns>
		Task<bool> Contains(string key);

		/// <summary>
		/// Returns the number of items currently in the cache.
		/// </summary>
        Task<int> GetCount();
		
		/// <summary>
		/// Removes all items from the cache. If an error occurs during the removal, the cache is left unchanged.
		/// </summary>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		Task Flush();

		/// <summary>
		/// Returns the value associated with the given key.
		/// </summary>
		/// <param name="key">Key of item to return from cache.</param>
		/// <returns>Value stored in cache</returns>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        Task<byte[]> GetData(string key);


		/// <summary>
		/// Removes the given item from the cache. If no item exists with that key, this method does nothing.
		/// </summary>
		/// <param name="key">Key of item to remove from cache.</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		Task Remove(string key);
	}
}
