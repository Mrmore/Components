// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

namespace Renren.Components.Caching
{
	/// <summary>
	/// Cache scavenging policy based on capacity.
	/// </summary>
	public class CacheCapacityScavengingPolicy
	{
		private readonly int maximumElementsInCacheBeforeScavenging;

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheCapacityScavengingPolicy"/> class with the name of the cache manager and the proxy to the configuration data.
		/// </summary>
		/// <param name="maximumElementsInCacheBeforeScavenging">The proxy to the latest configuration data.</param>
		public CacheCapacityScavengingPolicy(int maximumElementsInCacheBeforeScavenging)
		{
			this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
		}

		/// <summary>
		/// Gets the maximum items to allow before scavenging.
		/// </summary>
		/// <value>
		/// The maximum items to allow before scavenging.
		/// </value>
		public int MaximumItemsAllowedBeforeScavenging
		{
			get { return this.maximumElementsInCacheBeforeScavenging; }
		}

		/// <summary>
        /// Determines if scavenging is needed.
		/// </summary>
		/// <param name="currentCacheItemCount">The current number of <see cref="CacheItem"/> objects in the cache.</param>
		/// <returns>
		/// <see langword="true"/> if scavenging is needed; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsScavengingNeeded(int currentCacheItemCount)
		{
			return currentCacheItemCount > MaximumItemsAllowedBeforeScavenging;
		}
	}
}
