// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using Renren.Components.Tools;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Renren.Components.Caching.Expirations
{
    /// <summary>
    ///	This provider tests if a item was expired using a extended format.
    /// </summary>
    [DataContract]
    public class ExtendedFormatTime : ICacheItemExpiration
    {
        [DataMember]
        private string extendedFormat;
        public string TimeFormat
        {
            get { return extendedFormat; }
        }

        [DataMember]
        private long lastUsedTimeTicks;       

        /// <summary>
        ///	Convert the input format to the extented time format.
        /// </summary>
        /// <param name="timeFormat">
        ///	This contains the expiration information
        /// </param>
        public ExtendedFormatTime(string timeFormat)
        {
            // check arguments
            if (string.IsNullOrEmpty(timeFormat))
            {
                throw new ArgumentException("timeFormat exception");
            }
            
            ExtendedFormat.Validate(timeFormat);

            // Get the modified extended format
            this.extendedFormat = timeFormat;

            // Convert to UTC in order to compensate for time zones		
            this.lastUsedTimeTicks = DateTime.Now.ToUniversalTime().Ticks;
        }       

		/// <summary>
		/// Gets the extended time format.
		/// </summary>
		/// <value>
		/// The extended time format.
		/// </value>
		

        /// <summary>
        ///	Specifies if item has expired or not.
        /// </summary>
        /// <returns>
        ///	Returns true if the data is expired otherwise false
        /// </returns>
        public bool HasExpired()
        {
            // Convert to UTC in order to compensate for time zones		
            DateTime nowDateTime = DateTime.Now.ToUniversalTime();

            ExtendedFormat format = new ExtendedFormat(extendedFormat);
            // Check expiration
            if (format.IsExpired(new DateTime(lastUsedTimeTicks), nowDateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///	Notifies that the item was recently used.
        /// </summary>
        public void Notify()
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="owningCacheItem">Not used</param>
        public void Initialize(CacheItem owningCacheItem)
        {
        } 
    }
}
