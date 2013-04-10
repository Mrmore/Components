namespace Renren.Components.Caching.BackingStore
{
    /// <summary>
    /// Not intended for direct use.  Provides symmetric encryption and decryption services 
    /// to Isolated and Database backing stores.  Allows this block to use 
    /// Security.Cryptography without having a direct reference to that assembly.
    /// </summary>
    public interface IStorageEncryptionProvider 
    {
        /// <summary>
        /// Encrypt backing store data.
        /// </summary>
        /// <param name="plaintext">Clear bytes.</param>
        /// <returns>Encrypted bytes.</returns>
        byte[] Encrypt(byte[] plaintext);

        /// <summary>
        /// Decrypt backing store data.
        /// </summary>
        /// <param name="ciphertext">Encrypted bytes.</param>
        /// <returns>Decrypted bytes.</returns>
        byte[] Decrypt(byte[] ciphertext);
    }
}
