using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Thanks for Orktane
// http://www.orktane.com/post/2009/02/11/GZip-and-Deflate-Helpers.aspx
// Thanks for SharpZipLib
// http://slsharpziplib.codeplex.com/

namespace Renren.Components.IO.Compression.Deflate
{
    // Using Deflate Compression:
    public static class DeflateHelper
    {

        public static long Compress(Stream input, Stream output)
        {

            // Create a buffer to transfer the data
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            long totalBytes = 0;

            // Get the stream that will perform the decompression
            using (Stream zip = new DeflaterOutputStream(output))
            {

                // Use the buffer to move data to the compression stream until complete
                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                {

                    totalBytes += bytesRead;
                    zip.Write(buffer, 0, bytesRead);

                }
            }

            // Return the total number of bytes of compressed data
            return totalBytes;

        }

        public static long Decompress(Stream input, Stream output)
        {

            // Create a buffer to transfer the data
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            long totalBytes = 0;

            // Get the stream that will perform the decompression
            using (Stream zip = new InflaterInputStream(input))
            {

                // Use the buffer to move data to the compression stream until complete
                while ((bytesRead = zip.Read(buffer, 0, buffer.Length)) > 0)
                {

                    totalBytes += bytesRead;
                    output.Write(buffer, 0, bytesRead);

                }
            }


            // Returns the total number of bytes of decompressed data
            return totalBytes;

        }

        public static byte[] Compress(byte[] data)
        {

            byte[] final;

            // Create a stream to hold the output
            using (MemoryStream output = new MemoryStream(), input = new MemoryStream(data))
            {

                // Process the compression
                Compress(input, output);

                // Get the resultant data
                final = output.ToArray();
            }

            // Convert the output stream to a byte array
            return final;

        }

        public static byte[] Decompress(byte[] deflateData)
        {

            byte[] final;

            // Create a stream to hold the output
            using (MemoryStream output = new MemoryStream(), input = new MemoryStream(deflateData))
            {

                // Process the compression
                Decompress(input, output);

                // Get the resultant data
                final = output.ToArray();

            }

            // Convert the output stream to a byte array
            return final;

        }

        public static string Compress(string data)
        {

            // get a byte array of the data and pass to the Compress method.
            return Convert.ToBase64String(Compress(Encoding.UTF8.GetBytes(data)));

        }

        public static string Decompress(string deflateString)
        {

            // Decompress and convert data to a string
            var bytes = Decompress(Convert.FromBase64String(deflateString));
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);

        }

        public static byte[] Decompress(Stream input)
        {

            byte[] final;

            // Create a stream to hold the output
            using (MemoryStream output = new MemoryStream())
            {

                // Process the compression
                Decompress(input, output);

                // Get the resultant data
                final = output.ToArray();
            }

            return final;
        }

        public static string DecompressAsString(Stream input)
        {

            byte[] final;

            // Create a stream to hold the output
            using (MemoryStream output = new MemoryStream())
            {

                // Process the compression
                Decompress(input, output);

                // Get the resultant data
                final = output.ToArray();

            }

            // we get the string
            return Encoding.UTF8.GetString(final, 0, final.Length);

        }


        public static byte[] Compress(Stream input)
        {

            byte[] final;

            // Create a stream to hold the output
            using (MemoryStream output = new MemoryStream())
            {

                // Process the compression
                Compress(input, output);

                // Get the resultant data
                final = output.ToArray();
            }

            return final;
        }

        public static string CompressAsString(Stream input)
        {

            byte[] final;

            // Create a stream to hold the output
            using (MemoryStream output = new MemoryStream())
            {

                // Process the compression
                Compress(input, output);

                // Get the resultant data
                final = output.ToArray();
            }

            // we get the string
            return Encoding.UTF8.GetString(final, 0, final.Length);

        }
    }
}
