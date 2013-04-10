using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Renren.Components.Caching.BackingStore
{
    /// <summary>
    /// 
    /// </summary>
    internal class StorageCacheItemField
    {
        private string fieldName;
        private StorageFolder storageFolder;
        private IStorageEncryptionProvider encryptionProvider;

        /// <summary>
        /// Instance constructor
        /// </summary>
        /// <param name="storage">IsolatedStorage area to use. May not be null.</param>
        /// <param name="fieldName">Name of the file in which the field value is stored. May not be null.</param>
        /// <param name="fileSystemLocation">Complete path to directory where file specified in fieldName is to be found. May not be null.</param>
        /// <param name="encryptionProvider">Encryption provider</param>
        public StorageCacheItemField(StorageFolder storageFolder, string fieldName, IStorageEncryptionProvider encryptionProvider)
        {
            this.fieldName = fieldName;
            this.storageFolder = storageFolder;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Writes value to file
        /// </summary>
        /// <param name="itemToWrite">Object to write into Isolated Storage</param>
        /// <param name="encrypted">True if item written is to be encrypted</param>
        public async Task Write(byte[] itemToWrite, bool encrypted)
        {
            if (this.storageFolder == null || string.IsNullOrEmpty(this.fieldName))
            {
                return;
            }

            if (itemToWrite == null || itemToWrite.Length == 0)
            {
                return;
            }
            try
            {
                StorageFile storageFile = await storageFolder.CreateFileAsync(fieldName, CreationCollisionOption.OpenIfExists);
                await WriteField(itemToWrite, storageFile, encrypted);
            }
            catch
            {
 
            }
            
        }

        public async Task Write(string itemToWrite, bool encrypted)
        {
            if (string.IsNullOrEmpty(itemToWrite))
            {
                return;
            }

            await Write(Encoding.UTF8.GetBytes(itemToWrite), encrypted);
        }

        /// <summary>
        /// Overwrites given field. Item will not be encrypted
        /// </summary>
        /// <param name="itemToWrite">Object to write into Isolated Storage</param>
        public async Task Overwrite(byte[] itemToWrite)
        {
            await Write(itemToWrite, false);
        }

        public async Task Overwrite(string itemToWrite)
        {
            await Write(itemToWrite, false);
        }

        /// <summary>
        /// Reads value from StorageFile
        /// </summary>
        /// <param name="encrypted">True if field is stored as encrypted</param>
        /// <returns>Value read from IsolatedStorage. This value may be null if the value stored is null.</returns>
        public async Task<byte[]> ReadToByte(bool encrypted)
        {
            if (this.storageFolder == null || string.IsNullOrEmpty(this.fieldName))
            {
                return null;
            }

            StorageFile storageFile = await storageFolder.CreateFileAsync(fieldName, CreationCollisionOption.OpenIfExists);
            return await ReadField(storageFile,encrypted);
        }

        public async Task<string> ReadToString(bool encrypted)
        {
            byte[] result = await ReadToByte(encrypted);
            if (result == null || result.Length == 0)
            {
                return null;
            }
            else
            {
                return Encoding.UTF8.GetString(result, 0, result.Length);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemToWrite">Value to write. May be null.</param>
        /// <param name="fileStream">Stream to which value should be written. May not be null.</param>
        /// <param name="encrypted">True if item is to be encrypted</param>
        protected virtual async Task WriteField(byte[] itemToWrite, StorageFile storageFile, bool encrypted)
        {
            if (encrypted)
            {
                itemToWrite = EncryptValue(itemToWrite);
            }
            await Task.Run(async () =>
                 {
                     using (var fs = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                     {
                         using (var outStream = fs.GetOutputStreamAt(0))
                         {
                             using (var dataWriter = new DataWriter(outStream))
                             {
                                 if (itemToWrite != null)
                                 {
                                     dataWriter.WriteBytes(itemToWrite);
                                 }
                                 await dataWriter.StoreAsync();
                                 dataWriter.DetachStream();
                             }
                             await outStream.FlushAsync();
                         }
                     }
                 }
                 );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        protected virtual async Task<byte[]> ReadField(StorageFile storageFile, bool encrypted)
        {
            var props = await storageFile.GetBasicPropertiesAsync();
            if (props.Size == 0)
            {
                return null;
            }
            return await Task.Run(async () =>
                {
                    using (var fs = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        using (var inStream = fs.GetInputStreamAt(0))
                        {
                            using (var reader = new DataReader(inStream))
                            {
                                await reader.LoadAsync((uint)fs.Size);
                                byte[] fieldValue = new byte[(uint)fs.Size];
                                reader.ReadBytes(fieldValue);
                                reader.DetachStream();
                                if (encrypted)
                                {
                                    fieldValue = DecryptValue(fieldValue);
                                }
                                return fieldValue;
                            }
                        }
                    }
                }
                );

        }

        private byte[] EncryptValue(byte[] valueBytes)
        {
            if (encryptionProvider != null)
            {
                valueBytes = encryptionProvider.Encrypt(valueBytes);
            }

            return valueBytes;
        }

        private byte[] DecryptValue(byte[] fieldBytes)
        {
            if (encryptionProvider != null)
            {
                fieldBytes = encryptionProvider.Decrypt(fieldBytes);
            }
            return fieldBytes;
        }
    }
}
