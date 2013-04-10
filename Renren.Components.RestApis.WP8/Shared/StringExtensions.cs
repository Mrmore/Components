using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
#endif

namespace Renren.Components.Shared
{
    /// <summary>
    /// A collection of simple helper extension methods.
    /// </summary>
    public static class StringExtensions
    {
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
