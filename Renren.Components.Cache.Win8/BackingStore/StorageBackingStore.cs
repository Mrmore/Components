using Renren.Components.Async;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Renren.Components.Tools;
using Renren.Components.Shared;
using System.Diagnostics;

namespace Renren.Components.Caching.BackingStore
{
    /// <summary>
    /// Implementation of IBackingStore that stores its CacheItems into IsolatedStorage.
    /// </summary>
    /// <remarks>
    /// This class assumes a tree-structured storage schema. Each named instance of an Isolated Storage area creates a 
    /// separate, top-level directory in Isolated Storage. This is to allow a user to segregate different areas in Isolated Storage
    /// to allow multiple applications to use their own logically separate areas. Inside each of these areas, each CacheItem is stored
    /// in its own subdirectory, with separate files in those subdirectories representing the different pieces of a CacheItem. 
    /// The item was split like this to allow for several optimizations. The first optimization is that now, the essence of a CacheItem
    /// can be restored independently of the underlying value. It is the deserialization of the value object that could conceivably 
    /// be very time consuming, so by splitting it off into its own file, that deserialization process could be delayed until the value is 
    /// actually needed. The second optimization is that we are now able to update the last accessed time for a CacheItem without 
    /// bringing the entire CacheItem into memory, make the update, and then reserialize it.
    /// </remarks>
    public class StorageBackingStore : BaseBackingStore
    {
        private string folderName;
        private StorageFolder storageFolder;
        private IStorageEncryptionProvider encryptionProvider;
        private AsyncReaderWriterLock readerWriterLock;

        /// <summary>
        /// default construct for IInitializeStandardized
        /// </summary>
        public StorageBackingStore()
        {
        }
        /// <summary>
        /// A <see cref="IBackingStore"/> that stores objects in Isolated Storage, identified by <paramref name="storageAreaName"/>.
        /// </summary>
        /// <param name="storageAreaName">Identifier for this Isolated Storage area. May not be null.</param>
        /// <permission cref="IsolatedStorageFilePermission">Demanded to ensure caller has permission to access Isolated Storage.</permission>
        public StorageBackingStore(string folderName)
            : this(folderName, null)
        {

        }

        /// <summary>
        /// Initialize Isolated Storage for this CacheItem by creating the directory where it will be stored. This 
        /// constructor should only be used for testing, and never called from production code.
        /// </summary>
        /// <param name="storageAreaName">Identifier for this Isolated Storage area. May not be null.</param>
        /// <param name="encryptionProvider">
        /// The <see cref="IStorageEncryptionProvider"/> to use to encrypt data in storage. This value can be <see langword="null"/>.
        /// </param>
        /// <permission cref="IsolatedStorageFilePermission">Demanded to ensure caller has permission to access Isolated Storage.</permission>
        public StorageBackingStore(string folderName, IStorageEncryptionProvider encryptionProvider)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentException("storageAreaName");
            }
            this.folderName = folderName;
            this.encryptionProvider = encryptionProvider;
        }

        public override async Task<int> GetCount()
        {
            using (var releaser = await readerWriterLock.ReaderLockAsync())
            {
                IReadOnlyList<StorageFolder> folders = await storageFolder.GetFoldersAsync();
                return folders.Count;
            }

        }

        /// <summary>
        /// Removes all items from this Isolated Storage area.
        /// </summary>
        public override async Task Flush()
        {
            await storageFolder.DeleteAsync();
            
            ///Del for CM
            ///
            //IReadOnlyList<StorageFolder> folders = await storageFolder.GetFoldersAsync();
            //foreach (StorageFolder folder in folders)
            //{
            //    await RemoveItem(folder.Name);
            //}



        }

        /// <summary>
        /// Removes the named item from Isolated Storage.
        /// </summary>
        /// <param name="storageKey">Identifier for CacheItem to remove.</param>
        protected override async Task Remove(int storageKey)
        {
            if (await ItemExists(storageKey))
            {
                await RemoveItem(storageKey.ToString(NumberFormatInfo.InvariantInfo));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override async Task<CacheItem> GetCacheItem(int key)
        {
            if (await ItemExists(key))
            {
                using (var releaser = await readerWriterLock.ReaderLockAsync())
                {
                    StorageFolder folder = await storageFolder.GetFolderAsync(key.ToString(NumberFormatInfo.InvariantInfo));
                    StorageCacheItem loadedItem = new StorageCacheItem(folder, this.encryptionProvider);
                    CacheItem itemLoadedFromStore = await loadedItem.Load();
                    return itemLoadedFromStore;
                }               
            }
            else
            {
                return null;
            }
        }

        protected override async Task<byte[]> GetValue(int key)
        {
            if (await ItemExists(key))
            {
                using (var releaser = await readerWriterLock.ReaderLockAsync())
                {
                    StorageFolder folder = await storageFolder.GetFolderAsync(key.ToString(NumberFormatInfo.InvariantInfo));
                    StorageCacheItem loadedItem = new StorageCacheItem(folder, this.encryptionProvider);
                    byte[] value = await loadedItem.LoadValue();
                    return value;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates the last accessed time for the specified CacheItem stored in Isolated Storage
        /// </summary>
        /// <param name="storageKey">Identifer for CacheItem to remove.</param>
        /// <param name="timestamp">New timestamp for CacheItem.</param>
        protected override async Task UpdateLastAccessedTime(int storageKey, DateTime timestamp)
        {
            using (var releaser = await readerWriterLock.WriterLockAsync())
            {
                try
                {
                    StorageFolder folder = await storageFolder.GetFolderAsync(storageKey.ToString(NumberFormatInfo.InvariantInfo));
                    StorageCacheItem storageItem = new StorageCacheItem(folder, this.encryptionProvider);
                    await storageItem.UpdateLastAccessedTime(timestamp);
                }
                catch (IOException)
                {
                    // do nothing
                }
            }       
        }


        /// <summary>
        /// Loads data from persistence store
        /// </summary>
        /// <returns>A Hashtable containing the cache items.</returns>
        protected override async Task<IDictionary<string, CacheItem>> LoadDataFromStore()
        {
            using (var releaser = await readerWriterLock.ReaderLockAsync())
            {
                Dictionary<string, CacheItem> itemsLoadedFromStore = new Dictionary<string, CacheItem>();

                IReadOnlyList<StorageFolder> folders = await storageFolder.GetFoldersAsync();
                foreach (StorageFolder folder in folders)
                {
                    try
                    {
                        StorageCacheItem loadedItem = new StorageCacheItem(folder, this.encryptionProvider);
                        CacheItem itemLoadedFromStore = await loadedItem.Load();
                        if (!itemsLoadedFromStore.ContainsKey(itemLoadedFromStore.Key))
                        {
                            itemsLoadedFromStore.Add(itemLoadedFromStore.Key, itemLoadedFromStore);                            
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("LoadDataFromStore" + ex.ToString());
                        throw new Exception("Load file faile");
                    }
                }
                return itemsLoadedFromStore;
            }     
        }

        /// <summary>
        /// Adds new item to persistence store
        /// </summary>
        /// <param name="storageKey">Unique key for storage item</param>
        /// <param name="newItem">Item to be added to cache. May not be null.</param>
        protected override async Task AddNewItem(string storageKey, CacheItem newItem, byte[] data)
        {
            using (var releaser = await readerWriterLock.WriterLockAsync())
            {
                StorageFolder folder = await storageFolder.CreateFolderAsync(storageKey, CreationCollisionOption.OpenIfExists);
                StorageCacheItem cacheItem = new StorageCacheItem(folder, this.encryptionProvider);
                await cacheItem.Store(newItem, data);
            }
        }

        private async Task<bool> ItemExists(int possibleItem)
        {
            using (var releaser = await readerWriterLock.ReaderLockAsync())
            {
                try
                {
                    await storageFolder.GetFolderAsync(possibleItem.ToString(NumberFormatInfo.InvariantInfo));
                    return true;
                }
                catch
                {
                    return false;
                }
            }       
        }

        private async Task RemoveItem(string storageKey)
        {
            using (var releaser = await readerWriterLock.WriterLockAsync())
            {
                StorageFolder folder = await storageFolder.GetFolderAsync(storageKey);
                ///////Del for CM
                
                //IReadOnlyList<StorageFile> storageFileList = await folder.GetFilesAsync();

                //foreach (StorageFile file in storageFileList)
                //{
                //    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                //}
                await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }        
        }



        public override async Task Initialize()
        {
            string sample = "Renren Windows Components team";
            string cacheRoot = "Cache_Root." + sample.ToMD5String();
            readerWriterLock = new AsyncReaderWriterLock();
            StorageFolder rootFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(cacheRoot, CreationCollisionOption.OpenIfExists);
            storageFolder = await rootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
        }
    }
}
