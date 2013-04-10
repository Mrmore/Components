// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Renren.Components.Caching.Expirations
{
    /// <summary>
    ///	This class tests if a data item was expired using a absolute time 
    ///	schema.
    /// </summary>
    [DataContract]
    public class AbsoluteTime : ICacheItemExpiration
    {
        [DataMember]
        private long absoluteExpirationTime;
        public long AbsoluteExpirationTime
        {
            get { return absoluteExpirationTime; }
        }
        /// <summary>
        ///	Create an instance of the class with a time value as input and 
        ///	convert it to UTC.
        /// </summary>
        /// <param name="absoluteTime">
        ///	The time to be checked for expiration
        /// </param>
        public AbsoluteTime(DateTime absoluteTime)
        {
            if (absoluteTime > DateTime.Now)
            {
                // Convert to UTC in order to compensate for time zones	
                this.absoluteExpirationTime = absoluteTime.ToUniversalTime().Ticks;
            }
            else
            {
                throw new ArgumentOutOfRangeException("absoluteTime exception");
            }
        }

		/// <summary>
		/// Gets the absolute expiration time.
		/// </summary>
		/// <value>
		/// The absolute expiration time.
		/// </value>
		

        /// <summary>
        /// Creates an instance based on a time interval starting from now.
        /// </summary>
        /// <param name="timeFromNow">Time interval</param>
        public AbsoluteTime(TimeSpan timeFromNow) : this(DateTime.Now + timeFromNow)
        {
        }
		
        /// <summary>
        ///	Specifies if item has expired or not.
        /// </summary>
        /// <remarks>
        ///	bool isExpired = ICacheItemExpiration.HasExpired();
        /// </remarks>
        /// <returns>
        ///	"True", if the data item has expired or "false", if the data item 
        ///	has not expired
        /// </returns>
        public bool HasExpired() //ICacheItemExpiration
        {
            // Convert to UTC in order to compensate for time zones		
            DateTime nowDateTime = DateTime.Now.ToUniversalTime();

            // Check expiration
            return nowDateTime.Ticks >= this.absoluteExpirationTime;
        }

        /// <summary>
        ///	Called to notify this object that the CacheItem owning this expiration was just touched by a user action
        /// </summary>
        public void Notify()
        {
        }

        /// <summary>
        /// Called to give this object an opportunity to initialize itself from data inside a CacheItem
        /// </summary>
        /// <param name="owningCacheItem">CacheItem provided to read initialization information from. Will never be null.</param>
        public void Initialize(CacheItem owningCacheItem)
        {
        }        
    }
}
