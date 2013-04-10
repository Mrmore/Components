// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/
using System;

namespace Renren.Components.Caching.Expirations
{
    /// <summary>
    /// This class reflects an expiration policy of never being expired.
    /// </summary>
    public class NeverExpired : ICacheItemExpiration
    {
        /// <summary>
        /// Always returns false
        /// </summary>
        /// <returns>False always</returns>
        public bool HasExpired()
        {
            return false;
        }

        /// <summary>
        /// Not used
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
