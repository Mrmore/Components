// ===============================================================================
// JpegDecoder.cs
// .NET Image Tools
// ===============================================================================
// Copyright (c) .NET Image Tools Development Group. 
// All rights reserved.
// ===============================================================================

using ImageTools.Helpers;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System;
namespace ImageTools.IO.Jpeg
{
    /// <summary>
    /// Image decoder for generating an image out of an jpg stream.
    /// </summary>
    public class JpegDecoder : IImageDecoder
    {
        #region IImageDecoder Members

        /// <summary>
        /// Gets the size of the header for this image type.
        /// </summary>
        /// <value>The size of the header.</value>
        public int HeaderSize
        {
            get { return 11; }
        }

        /// <summary>
        /// Indicates if the image decoder supports the specified
        /// file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns>
        /// <c>true</c>, if the decoder supports the specified
        /// extensions; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="extension"/>
        /// is null (Nothing in Visual Basic).</exception>
        /// <exception cref="System.ArgumentException"><paramref name="extension"/> is a string
        /// of length zero or contains only blanks.</exception>
        public bool IsSupportedFileExtension(string extension)
        {
            Guard.NotNullOrEmpty(extension, "extension");

            string extensionAsUpper = extension.ToUpper();
            return extensionAsUpper == "JPG" ||
                   extensionAsUpper == "JPEG" ||
                   extensionAsUpper == "JFIF";
        }

        /// <summary>
        /// Indicates if the image decoder supports the specified
        /// file header.
        /// </summary>
        /// <param name="header">The file header.</param>
        /// <returns>
        /// <c>true</c>, if the decoder supports the specified
        /// file header; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="header"/>
        /// is null (Nothing in Visual Basic).</exception>
        public bool IsSupportedFileFormat(byte[] header)
        {
            Guard.NotNull(header, "header");

            bool isSupported = false;

            if (header.Length >= 11)
            {
                bool isJpeg = IsJpeg(header);
                bool isExif = IsExif(header);

                isSupported = isJpeg || isExif;
            }

            return isSupported;
        }

        private bool IsExif(byte[] header)
        {
            bool isExif =
                header[6] == 0x45 && // E
                header[7] == 0x78 && // x
                header[8] == 0x69 && // i
                header[9] == 0x66 && // f
                header[10] == 0x00;

            return isExif;
        }

        private static bool IsJpeg(byte[] header)
        {
            bool isJpg =
                header[6] == 0x4A && // J
                header[7] == 0x46 && // F
                header[8] == 0x49 && // I
                header[9] == 0x46 && // F
                header[10] == 0x00;

            return isJpg;
        }

        /// <summary>
        /// Decodes the image from the specified stream and sets
        /// the data to image.
        /// </summary>
        /// <param name="image">The image, where the data should be set to.
        /// Cannot be null (Nothing in Visual Basic).</param>
        /// <param name="stream">The stream, where the image should be
        /// decoded from. Cannot be null (Nothing in Visual Basic).</param>
        /// <exception cref="System.ArgumentNullException">
        /// 	<para><paramref name="image"/> is null (Nothing in Visual Basic).</para>
        /// 	<para>- or -</para>
        /// 	<para><paramref name="stream"/> is null (Nothing in Visual Basic).</para>
        /// </exception>
        public async Task Decode(ExtendedImage image, Stream stream)
        {
            Guard.NotNull(image, "image");
            Guard.NotNull(stream, "stream");

            //var memStream = new InMemoryRandomAccessStream();
            //await stream.CopyToAsync(memStream.AsStream());
            //memStream.Seek(0);

            var memStream = new InMemoryRandomAccessStream();
            var outputStream = memStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(stream.AsInputStream(), outputStream); 

            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(memStream);
            BitmapPixelFormat format = decoder.BitmapPixelFormat;
            BitmapAlphaMode alpha = decoder.BitmapAlphaMode;

            PixelDataProvider pixelProvider = await decoder.GetPixelDataAsync();
            byte[] pixels = pixelProvider.DetachPixelData();
            image.SetPixels((int)decoder.PixelWidth, (int)decoder.PixelHeight, pixels, format, alpha);
        }

        #endregion
    }
}
