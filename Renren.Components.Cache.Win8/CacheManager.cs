// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using Renren.Components.Caching.BackingStore;
using Renren.Components.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
namespace Renren.Components.Caching
{
	/// <summary>
	/// This class represents the interface to caching as shown to the user. All caching operations are performed through this class.
	/// </summary>
    public class CacheManager : IIInitializeStandardized<CacheManager, CacheConfigSetting>, IDisposable, ICacheManager
	{
		private Cache realCache;
		private ExpirationPollTimer pollTimer;
        private BackgroundScheduler backgroundScheduler;

        /// <summary>
        /// default construt for IIInitializeStandardized
        /// </summary>
        public CacheManager()
        {
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="realCache"></param>
        ///// <param name="backgroundScheduler"></param>
        ///// <param name="pollTimer"></param>
        //public CacheManager(Cache realCache, BackgroundScheduler backgroundScheduler, ExpirationPollTimer pollTimer)
        //{
        //    Guarder.NotNull(pollTimer, "poolTimer");
        //    this.realCache = realCache;
        //    this.pollTimer = pollTimer;
        //    this.backgroundScheduler = backgroundScheduler;

        //    pollTimer.StartPolling(backgroundScheduler.ExpirationTimeoutExpired);
        //}


        public override async Task Initialize(CacheConfigSetting setting)
        {
            Guarder.NotNull(setting, "configuration setting");
            Debug.WriteLine("Cache mgr init start   " + DateTime.Now.ToString("HH-mm-ss fff") + " Key =>" + setting.Key);
            try
            {
                IBackingStore store;
                if (setting.CacheStoreType == StoreType.File)
                {
                    store = new StorageBackingStore(setting.Key);
                }
                else
                {
                    throw new Exception("DataBase is not be realize");
                }

                Renren.Components.Caching.Cache cache = new Caching.Cache(store);
                BackgroundScheduler scheduler = new BackgroundScheduler(new ExpirationTask(cache), new ScavengerTask(setting.RemoveNumber, setting.MaxItemNumber, cache));
                ExpirationPollTimer timer = new ExpirationPollTimer(setting.TimerTicks);

                this.realCache = cache;
                this.backgroundScheduler = scheduler;
                this.pollTimer = timer;

                pollTimer.StartPolling(backgroundScheduler.ExpirationTimeoutExpired);
                await realCache.Initialize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cache mgr init error   " + DateTime.Now.ToString("HH-mm-ss fff"));
                Debug.WriteLine(ex.ToString());
                throw new Exception("Init CacheManager fail");
            }
            finally
            {
                Debug.WriteLine("Cache mgr init finally   " + DateTime.Now.ToString("HH-mm-ss fff"));
            }
           
        }


        //////////by CM
        /// <summary>
        /// Initialize all need init instance
        /// </summary>
        /// <returns></returns>
        //public override async Task Initialize(IConfigurationSetting setting)
        //{
        //    Guarder.NotNull(setting, "configuration setting");

        //    try
        //    {
        //        StorageFile configFile = await Package.Current.InstalledLocation.GetFileAsync(setting.ConfigName);
        //        XElement cacheElement;
        //        using (var stream = await configFile.OpenStreamForReadAsync())
        //        {
        //            using (StreamReader reader = new StreamReader(stream))
        //            {
        //                var xDoc = XDocument.Load(XmlReader.Create(reader));
        //                cacheElement = xDoc.Root;
        //            }
        //        }

        //        IConfigurationSection cacheSection = new CacheConfigurationSection(cacheElement);
        //        string managerName = string.IsNullOrEmpty(setting.InstanceName) ? cacheSection.GetAttributeValue("cacheManagers") : setting.InstanceName;
        //        IConfigurationSection cacheManagerSection = cacheSection.GetConfigurationSection(managerName);
        //        IConfigurationSection backingstoreSection = cacheSection.GetConfigurationSection(cacheManagerSection.GetAttributeValue("backingStores"));

        //        IBackingStore backingStore = (IBackingStore)Activator.CreateInstance(Type.GetType(backingstoreSection.GetAttributeValue("type")));
        //        Cache cache = new Cache(backingStore);

        //        ExpirationTask expirationTask = new ExpirationTask(cache);
        //        ScavengerTask scavengerTask = new ScavengerTask(int.Parse(cacheManagerSection.GetAttributeValue("numberToRemoveWhenScavenging")),
        //            int.Parse(cacheManagerSection.GetAttributeValue("maximumElementsInCacheBeforeScavenging")),
        //            cache);
        //        ExpirationPollTimer pollTimer = new ExpirationPollTimer(int.Parse(cacheManagerSection.GetAttributeValue("expirationPollFrequencyInSeconds")));

        //        this.realCache = cache;
        //        this.backgroundScheduler = new BackgroundScheduler(expirationTask, scavengerTask);
        //        this.pollTimer = pollTimer;

        //        string uid = string.Empty;
        //        if (setting.InitArgs != null && setting.InitArgs.ContainsKey(typeof(IBackingStore)))
        //        {
        //            uid = ((int)setting.InitArgs[typeof(IBackingStore)]).ToString() + @"\";
        //        }
        //        await realCache.Initialize(uid + cacheManagerSection.GetAttributeValue("backingStoreFolderName"));
        //        pollTimer.StartPolling(backgroundScheduler.ExpirationTimeoutExpired);
        //    }
        //    catch
        //    {
        //        throw new Exception("Init CacheManager fail");
        //    }
        //}

		/// <summary>
		/// Returns the number of items currently in the cache.
		/// </summary>
		public async Task<int> GetCount()
		{
            return await realCache.GetCount();
		}

		/// <summary>
		/// Returns true if key refers to item current stored in cache
		/// </summary>
		/// <param name="key">Key of item to check for</param>
		/// <returns>True if item referenced by key is in the cache</returns>
		public async Task<bool> Contains(string key)
		{
			return await realCache.Contains(key);
		}

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
        public async Task Add(string key, byte[] value)
        {
            Guarder.NotNullOrEmpty(key, "key");
            await Add(key, value, CacheItemPriority.Normal, null);
        }
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
       public async Task Add(string key, byte[] value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
        {
            Guarder.NotNullOrEmpty(key, "key");
            await realCache.Add(key, value, scavengingPriority, refreshAction, expirations);

            await backgroundScheduler.StartScavengingIfNeeded();
        }
		/// <summary>
		/// Removes the given item from the cache. If no item exists with that key, this method does nothing.
		/// </summary>
		/// <param name="key">Key of item to remove from cache.</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public async Task Remove(string key)
		{
			await realCache.Remove(key);
		}

		/// <summary>
		/// Returns the value associated with the given key.
		/// </summary>
		/// <param name="key">Key of item to return from cache.</param>
		/// <returns>Value stored in cache</returns>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        public async Task<byte[]> GetData(string key)
        {
            Guarder.NotNullOrEmpty(key, "key");
            return await realCache.GetData(key);
        }
		/// <summary>
		/// Removes all items from the cache. If an error occurs during the removal, the cache is left unchanged.
		/// </summary>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public async Task Flush()
		{
			await realCache.Flush();
		}

		/// <summary>
		/// Not intended for public use. Only public due to requirements of IDisposable. If you call this method, your
		/// cache will be unusable.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);

			if (pollTimer != null)
			{
				pollTimer.StopPolling();
				pollTimer = null;
			}
			if (realCache != null)
			{
				realCache.Dispose();
				realCache = null;
			}
		}
    }
}
