using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.IO
{
    public static class StreamExtensions
    {
        public static byte[] ToArray(this Stream input)
        {
            byte[] bytes = null;
            using (var mem = new MemoryStream())
            {
                input.CopyTo(mem);

                input.Flush();
                mem.Flush();
                bytes = mem.ToArray();
            }

            return bytes;
        }

        public static string ConvertToString(this Stream input, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            var bytes = input.ToArray();
            return encode.GetString(bytes, 0, bytes.Length);
        }
    }
}
