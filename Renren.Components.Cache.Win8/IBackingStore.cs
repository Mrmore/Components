// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
	/// <summary>
	/// <P>This interface defines the contract that must be implemented by all backing stores. 
	/// Implementors of this method are responsible for interacting with their underlying
	/// persistence mechanisms to store and retrieve CacheItems. All methods below must guarantee 
	/// Weak Exception Safety. This means that operations must complete entirely, or they must completely
	/// clean up from the failure and leave the cache in a consistent state. The mandatory
	/// cleanup process will remove all traces of the item that caused the failure, causing that item
	/// to be expunged from the cache entirely.
	/// </P>
	/// </summary>
	/// <remarks>
	/// Due to the way the Caching class is implemented, implementations of this class will always be called in 
	/// a thread-safe way. There is no need to make derived classes thread-safe.
	/// </remarks>
	public interface IBackingStore : IDisposable
	{
		/// <summary>
		/// Number of objects stored in the backing store
		/// </summary>
        Task<int> GetCount();

		/// <summary>
		/// <p>
		/// This method is responsible for adding a CacheItem to the BackingStore. This operation must be successful 
		/// even if an item with the same key already exists. This method must also meet the Weak Exception Safety guarantee
		/// and remove the item from the backing store if any part of the Add fails.
		/// </p> 
		/// </summary>
		/// <param name="newCacheItem">CacheItem to be added</param>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Add
		/// </p>
		/// </remarks>
		Task Add(CacheItem newCacheItem,byte[] value);

		/// <summary>
		/// Removes an item with the given key from the backing store
		/// </summary>
		/// <param name="key">Key to remove. Must not be null.</param>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Remove
		/// </p>
		/// </remarks>
		Task Remove(string key);

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
		Task UpdateLastAccessedTime(string key, DateTime timestamp);

		/// <summary>
		/// Flushes all CacheItems from backing store. This method must meet the Weak Exception Safety guarantee.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Flush
		/// </p>
		/// </remarks>
		Task Flush();

		/// <summary>
		/// Loads all CacheItems from backing store. 
		/// </summary>
		/// <returns>IDictionary<string,CacheItem> filled with all existing CacheItems.</returns>
		/// <remarks>
		/// <p>
		/// Other exceptions can be thrown, depending on what individual Backing Store implementations throw during Load
		/// </p>
		/// </remarks>
        Task<IDictionary<string, CacheItem>> Load();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<CacheItem> GetCacheItem(string key);

        Task<byte[]> GetValue(string key);

        Task Initialize();
	}
}
