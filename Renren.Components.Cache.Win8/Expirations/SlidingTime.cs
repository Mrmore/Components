// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using Renren.Components.Caching;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Renren.Components.Caching.Expirations
{
    /// <summary>
    ///	This provider tests if a item was expired using a time slice schema.
    /// </summary>
    [DataContract]
    public class SlidingTime : ICacheItemExpiration
    {
        [DataMember]
        private long timeTicksLastUsed;
        public long TimeTicksLastUsed
        {
            get { return timeTicksLastUsed; }
        }

        [DataMember]
        private long itemSlidingExpiration;
        public long ItemSlidingExpiration
        {
            get { return itemSlidingExpiration; }
        }

        /// <summary>
        ///	Create an instance of this class with the timespan for expiration.
        /// </summary>
        /// <param name="slidingExpiration">
        ///	Expiration time span
        /// </param>
        public SlidingTime(TimeSpan slidingExpiration)
        {
            // Check that expiration is a valid numeric value
            if (!(slidingExpiration.TotalSeconds >= 1))
            {
                throw new ArgumentOutOfRangeException("slidingExpiration exception");
            }

            this.itemSlidingExpiration = slidingExpiration.Ticks;
        }


        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="slidingExpiration"/>
        /// <param name="originalTimeStamp"/>
		/// <remarks>
		/// This constructor is for testing purposes only. Never, ever call it in a real program
		/// </remarks>
        public SlidingTime(TimeSpan slidingExpiration, DateTime originalTimeStamp) : this(slidingExpiration)
        {
            timeTicksLastUsed = originalTimeStamp.ToUniversalTime().Ticks;
        }        

        /// <summary>
        /// Returns sliding time window that must be exceeded for expiration to occur
        /// </summary>
        

        /// <summary>
        /// Returns time that this object was last touched
        /// </summary>
        
		
		/// <summary>
        ///	Specifies if item has expired or not.
        /// </summary>
        /// <returns>Returns true if the item has expired otherwise false.</returns>
        public bool HasExpired()
        {
            bool expired = CheckSlidingExpiration(DateTime.Now.ToUniversalTime().Ticks, this.timeTicksLastUsed, this.itemSlidingExpiration);
            return expired;
        }

        /// <summary>
        ///	Notifies that the item was recently used.
        /// </summary>
        public void Notify()
        {
            this.timeTicksLastUsed = DateTime.Now.ToUniversalTime().Ticks;
        }

        /// <summary>
        /// Used to set the initial value of TimeLastUsed. This method is invoked during the reinstantiation of
        /// an instance from a persistent store. 
        /// </summary>
        /// <param name="owningCacheItem">CacheItem to which this expiration belongs.</param>
        public void Initialize(CacheItem owningCacheItem)
        {
            if (owningCacheItem == null) throw new ArgumentNullException("owningCacheItem");

            timeTicksLastUsed = owningCacheItem.LastAccessedTime.Ticks;
        }

        /// <summary>
        ///	Check whether the sliding time has expired.
        /// </summary>
        /// <param name="nowDateTime">Current time </param>
        /// <param name="lastUsed">The last time when the item has been used</param>
        /// <param name="slidingExpiration">The span of sliding expiration</param>
        /// <returns>True if the item was expired, otherwise false</returns>
        private static bool CheckSlidingExpiration(long nowDateTimeTicks, long lastUsedTimeTicks, long slidingExpiration)
        {
            // Convert to UTC in order to compensate for time zones
            //DateTime tmpNowDateTime = nowDateTime.ToUniversalTime();

            // Convert to UTC in order to compensate for time zones
            //DateTime tmpLastUsed = lastUsed.ToUniversalTime();

            long expirationTicks = lastUsedTimeTicks + slidingExpiration;

            bool expired = (nowDateTimeTicks >= expirationTicks) ? true : false;

            return expired;
        }
    }
}
