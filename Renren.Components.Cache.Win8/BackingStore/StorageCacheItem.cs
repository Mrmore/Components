using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Renren.Components.IO.Serialization;
using System.Collections.Generic;
using System.Diagnostics;

namespace Renren.Components.Caching.BackingStore
{
	/// <summary>
	/// Represents a CacheItem as stored in Isolated Storage. This class is responsible for storing and
	/// restoring the item from the underlying file system store.
	/// </summary>
	public class StorageCacheItem
	{
		private StorageCacheItemField keyField;  //Key
        private StorageCacheItemField valueField;  //value
        private StorageCacheItemField scavengingPriorityField;  //清除优先级
        private StorageCacheItemField refreshActionField; //更新行为
        private StorageCacheItemField expirationsField;   //清除策略
        private StorageCacheItemField lastAccessedField;  //最后一次访问时间


		/// <summary>
		/// Instance constructor. Ensures that the storage location in Isolated Storage is prepared
		/// for reading and writing. This class stores each individual field of the CacheItem into its own
		/// file inside the directory specified by itemDirectoryRoot.
		/// </summary>
		/// <param name="storage">Isolated Storage area to use. May not be null.</param>
		/// <param name="itemDirectoryRoot">Complete path in Isolated Storage where the cache item should be stored. May not be null.</param>
		/// <param name="encryptionProvider">Encryption provider</param>
		public StorageCacheItem(StorageFolder storageFolder, IStorageEncryptionProvider encryptionProvider)
		{
            if (storageFolder == null)
            {
                throw new ArgumentNullException("storageFolder");
            }
                
            keyField = new StorageCacheItemField(storageFolder, "Key.txt", encryptionProvider);
            valueField = new StorageCacheItemField(storageFolder, "Val.txt",  encryptionProvider);
            scavengingPriorityField = new StorageCacheItemField(storageFolder, "ScPr.txt", encryptionProvider);
            refreshActionField = new StorageCacheItemField(storageFolder, "RA.txt", encryptionProvider);
            expirationsField = new StorageCacheItemField(storageFolder, "Exp.txt", encryptionProvider);
            lastAccessedField = new StorageCacheItemField(storageFolder, "LA.txt", encryptionProvider);
		}

		/// <summary>
		/// Stores specified CacheItem into IsolatedStorage at location specified in constructor
		/// </summary>
		/// <param name="itemToStore">The <see cref="CacheItem"/> to store.</param>
		public async Task Store(CacheItem itemToStore,byte[] value)
		{
            if (itemToStore == null)
            {
                throw new ArgumentNullException("itemToStore");
            } 
           await keyField.Write(Encoding.UTF8.GetBytes(itemToStore.Key), false);
           await valueField.Write(value, false);
           await scavengingPriorityField.Write(itemToStore.ScavengingPriority.ToString(), false);
           await refreshActionField.Write(InterfaceToString<ICacheItemRefreshAction>(itemToStore.RefreshAction), false);
           await expirationsField.Write(ExpirationsToByte(itemToStore.GetExpirations()), false);
           await lastAccessedField.Write(itemToStore.LastAccessedTime.ToUniversalTime().Ticks.ToString(), false);
		}

		/// <summary>
		/// Loads a CacheItem from IsolatedStorage from the location specified in the constructor
		/// </summary>
		/// <returns>CacheItem loaded from IsolatedStorage</returns>
		public async Task<CacheItem> Load()
		{
            
            string key = await keyField.ReadToString(false);
            Debug.WriteLine("public async Task<CacheItem> Load()   start=〉" + key);
            CacheItemPriority scavengingPriority = ByteToCacheItemPriority(await scavengingPriorityField.ReadToString(false));
            ICacheItemRefreshAction refreshAction = stringToObject<ICacheItemRefreshAction>(await refreshActionField.ReadToString(false));
            ICacheItemExpiration[] expirations = ByteToExpirations(await expirationsField.ReadToByte(false));
            DateTime lastAccessedTime = ByteToDateTime(await lastAccessedField.ReadToByte(false));
            Debug.WriteLine("public async Task<CacheItem> Load()   end");
            return new CacheItem(lastAccessedTime, key, scavengingPriority, refreshAction, expirations);
            
		}

        public async Task<byte[]> LoadValue()
		{
            byte[] value = await valueField.ReadToByte(false);
            return value;       
		}

		/// <summary>
		/// Updates the last accessed time for the CacheItem stored at this location in Isolated Storage
		/// </summary>
		/// <param name="newTimestamp">New timestamp</param>
		public async Task UpdateLastAccessedTime(DateTime newTimestamp)
		{
            await lastAccessedField.Overwrite(newTimestamp.ToUniversalTime().Ticks.ToString());
		}


        private string InterfaceToString<T>(T obj)
        {
            Dictionary<string, string> dir = new Dictionary<string, string>();
            dir.Add(obj.GetType().FullName, obj.SerializeAsJson<T>());
            return dir.SerializeAsJson<Dictionary<string, string>>();
        }

        private T stringToObject<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return default(T);
            }
            Dictionary<string, string> dir = JsonSerialization.LoadFromJsonString<Dictionary<string, string>>(data, typeof(Dictionary<string, string>));
            T CacheObj = default(T);
            foreach (string type in dir.Keys)
            {
                CacheObj = JsonSerialization.LoadFromJsonString<T>(dir[type], Type.GetType(type));             
            }
            return CacheObj;
        }

        private byte[] ExpirationsToByte(ICacheItemExpiration[] expirations)
        {
            Dictionary<string, string> dir = new Dictionary<string, string>();
            foreach (ICacheItemExpiration ce in expirations)
            {
                dir.Add(ce.GetType().FullName, ce.SerializeAsJson<ICacheItemExpiration>());
            }
            string ExpirationString = dir.SerializeAsJson<Dictionary<string, string>>();
            return Encoding.UTF8.GetBytes(ExpirationString);
        }
 

        private ICacheItemExpiration[] ByteToExpirations(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }
            string es = Encoding.UTF8.GetString(data,0,data.Length);

            Dictionary<string, string> dir = JsonSerialization.LoadFromJsonString<Dictionary<string, string>>(es, typeof(Dictionary<string, string>));
            ICacheItemExpiration[] CacheItemExpirations = new ICacheItemExpiration[dir.Count];
            int index = 0;
            foreach (string type in dir.Keys)
            {
                ICacheItemExpiration CacheExpiration = JsonSerialization.LoadFromJsonString<ICacheItemExpiration>(dir[type], Type.GetType(type));
                CacheItemExpirations[index++] = CacheExpiration;
            }
            return CacheItemExpirations;
        }
 


        private DateTime ByteToDateTime(byte[] data)
        {
            
            if (data == null || data.Length == 0)
            {
                return new DateTime();
            }
            return new DateTime(long.Parse(Encoding.UTF8.GetString(data,0,data.Length)));
        }

        private CacheItemPriority ByteToCacheItemPriority(string data)
        {
            switch (data)
            {
                case "CacheItemPriority.NotRemovable":
                    return CacheItemPriority.NotRemovable;
                case "CacheItemPriority.Normal":
                    return CacheItemPriority.Normal;
                case "CacheItemPriority.None":
                    return CacheItemPriority.None;
                case "CacheItemPriority.Low":
                    return CacheItemPriority.Low;
                case "CacheItemPriority.High":
                    return CacheItemPriority.High;
                default:
                    return CacheItemPriority.None;
            }
        }
	}
}
