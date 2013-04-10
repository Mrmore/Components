// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System.Collections;
using System.Collections.Generic;

namespace Renren.Components.Caching
{
	/// <summary>
	/// Sorts the cache items in data for scavenging
	/// </summary>>
    public class PriorityDateComparer : Comparer<CacheItem>
    {
		/// <summary>
		/// Compares two <see cref="CacheItem"/> objects and returns a value indicating whether one is less than, equal to or greater than the other in priority by date.
		/// </summary>
		/// <param name="x">
		/// First <see cref="CacheItem"/> to compare.
		/// </param>
		/// <param name="y">
		/// Second <see cref="CacheItem"/> to compare.
		/// </param>
		/// <returns>
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Condition</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description><paramref name="x"/> is less than <paramref name="y"/></description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description><paramref name="x"/> equals <paramref name="y"/></description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description><paramref name="x"/> is greater than <paramref name="y"/></description>
		/// </item>
		/// </list>
		/// </returns>
        public override int Compare(CacheItem x, CacheItem y)
        {
            CacheItem leftCacheItem = x;
            CacheItem rightCacheItem = y;

            lock (rightCacheItem)
            {
                lock (leftCacheItem)
                {
                    if (rightCacheItem == null && leftCacheItem == null)
                    {
                        return 0;
                    }
                    if (leftCacheItem == null)
                    {
                        return -1;
                    }
                    if (rightCacheItem == null)
                    {
                        return 1;
                    }

                    return leftCacheItem.ScavengingPriority == rightCacheItem.ScavengingPriority
                        ? leftCacheItem.LastAccessedTime.CompareTo(rightCacheItem.LastAccessedTime)
                        : leftCacheItem.ScavengingPriority - rightCacheItem.ScavengingPriority;
                }
            }
        }
    }
}
