// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using Renren.Components.Tools;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Renren.Components.Caching.Expirations
{
    /// <summary>
    ///	This class tracks a file cache dependency.
    /// </summary>
    [DataContract]
    public class FileDependency :IIInitializeStandardized<FileDependency, StorageFile>, ICacheItemExpiration
    {
        [DataMember]
        private string dependencyFileName;
        public string FileName
        {
            get { return dependencyFileName; }
        }

        [DataMember]
        private long lastModifiedTimeTicks;
        public long LastModifiedTimeTicks
        {
            get { return lastModifiedTimeTicks; }
        }


        //private StorageFile dependencyFile;
        /// <summary>
        ///	Constructor with one argument.
        /// </summary>
        /// <param name="fullFileName">
        ///	Indicates the name of the file
        /// </param>
        public FileDependency()
        {
        }


        public override async Task Initialize(StorageFile file)
        {
            try
            {
                dependencyFileName = file.Path;
                BasicProperties properties = await file.GetBasicPropertiesAsync();
                this.lastModifiedTimeTicks = properties.DateModified.ToUniversalTime().Ticks;
            }
            catch
            {
                throw new FileNotFoundException("file is not found or can't IOPermiss");
            }
        }
     

        /// <summary>
        ///	Specifies if the item has expired or not.
        /// </summary>
        /// <returns>Returns true if the item has expired, otherwise false.</returns>
        public async Task<bool> HasExpiredAsync()
        {
            try
            {
                StorageFile dependencyFile = await StorageFile.GetFileFromPathAsync(this.dependencyFileName);
                BasicProperties properties = await dependencyFile.GetBasicPropertiesAsync();
                DateTimeOffset currentModifiedTime = properties.DateModified;
                DateTimeOffset lastModifiedTime = new DateTimeOffset(new DateTime(lastModifiedTimeTicks));
                if (DateTimeOffset.Compare(lastModifiedTime, currentModifiedTime) == 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return true;
            }
        }

        public bool HasExpired()
        {
            return false;
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
