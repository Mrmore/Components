using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
#else
#endif

namespace Renren.Components.IO.Compression
{
    public static class CompressionExtensions
    {
        static public byte[] CompressAsGzip(this byte[] data)
        {
            return Gzip.GZipHelper.Compress(data);
        }

        static public byte[] CompressAsDefalte(this byte[] data)
        {
            return Deflate.DeflateHelper.Compress(data);
        }

        static public byte[] DecompressFromGzip(this byte[] data)
        {
            return Gzip.GZipHelper.Decompress(data);
        }

        static public byte[] DecompressFromDefalte(this byte[] data)
        {
            return Deflate.DeflateHelper.Decompress(data);
        }

        static public string CompressAsGzip(this string data)
        {
            return Gzip.GZipHelper.Compress(data);
        }

        static public string CompressAsDefalte(this string data)
        {
            return Deflate.DeflateHelper.Compress(data);
        }

        static public string DecompressFromGzip(this string data)
        {
            return Gzip.GZipHelper.Decompress(data);
        }

        static public string DecompressFromDefalte(this string data)
        {
            return Deflate.DeflateHelper.Decompress(data);
        }

        static public string CompressAsGzipString(this Stream data)
        {
            return Gzip.GZipHelper.CompressAsString(data);
        }

        static public string CompressAsDefalteString(this Stream data)
        {
            return Deflate.DeflateHelper.CompressAsString(data);
        }

        static public string DecompressFromGzipString(this Stream data)
        {
            return Gzip.GZipHelper.DecompressAsString(data);
        }

        static public string DecompressFromDefalteString(this Stream data)
        {
            return Deflate.DeflateHelper.DecompressAsString(data);
        }


        static public byte[] CompressAsGzipArray(this Stream data)
        {
            return Gzip.GZipHelper.Compress(data);
        }

        static public byte[] CompressAsDefalteArray(this Stream data)
        {
            return Deflate.DeflateHelper.Compress(data);
        }

        static public byte[] DecompressFromGzipArray(this Stream data)
        {
            return Gzip.GZipHelper.Decompress(data);
        }

        static public byte[] DecompressFromDefalteArray(this Stream data)
        {
            return Deflate.DeflateHelper.Decompress(data);
        }

        public static string ToMD5String(this string str)
        {
#if NETFX_CORE
            var alg = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
#else
            return MD5.GetMd5String(str);
#endif
        }
    }
}
