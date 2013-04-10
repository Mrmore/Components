// ===============================================================================
// PngDecoder.cs
// .NET Image Tools
// ===============================================================================
// Copyright (c) .NET Image Tools Development Group. 
// All rights reserved.
// ===============================================================================


using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace ImageTools.IO.Png
{
    /// <summary>
    /// Encoder for generating a image out of a png stream.
    /// </summary>
    /// <remarks>
    /// At the moment the following features are supported:
    /// <para>
    /// <b>Filters:</b> all filters are supported.
    /// </para>
    /// <para>
    /// <b>Pixel formats:</b>
    /// <list type="bullet">
    ///     <item>RGB (Truecolor) with alpha (8 bit).</item>
    ///     <item>RGB (Truecolor) without alpha (8 bit).</item>
    ///     <item>Greyscale with alpha (8 bit).</item>
    ///     <item>Greyscale without alpha (8 bit).</item>
    ///     <item>Palette Index with alpha (8 bit).</item>
    ///     <item>Palette Index without alpha (8 bit).</item>
    /// </list>
    /// </para> 
    /// </remarks>
    public class PngDecoder : IImageDecoder
    {
        #region Fields
        private ExtendedImage _image;
        private Stream _stream;
        private PngHeader _header;

        #endregion

        static PngDecoder()
        {
        }

        #region IImageDecoder Members

        /// <summary>
        /// Gets the size of the header for this image type.
        /// </summary>
        /// <value>The size of the header.</value>
        public int HeaderSize
        {
            get { return 8; }
        }

        /// <summary>
        /// Indicates if the image decoder supports the specified
        /// file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns>
        /// 	<c>true</c>, if the decoder supports the specified
        /// extensions; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="extension"/>
        /// is null (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException"><paramref name="extension"/> is a string
        /// of length zero or contains only blanks.</exception>
        public bool IsSupportedFileExtension(string extension)
        {
#if NETFX_CORE
            string extensionAsUpper = extension.ToUpper();
#else
            string extensionAsUpper = extension.ToUpper(CultureInfo.CurrentCulture);
#endif
            return extensionAsUpper == "PNG";
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
        /// <exception cref="ArgumentNullException"><paramref name="header"/>
        /// is null (Nothing in Visual Basic).</exception>
        public bool IsSupportedFileFormat(byte[] header)
        {
            bool isPng = false;

            if (header.Length >= 8)
            {
                isPng =
                    header[0] == 0x89 &&
                    header[1] == 0x50 && // P
                    header[2] == 0x4E && // N
                    header[3] == 0x47 && // G
                    header[4] == 0x0D && // CR
                    header[5] == 0x0A && // LF
                    header[6] == 0x1A && // EOF
                    header[7] == 0x0A;   // LF
            }

            return isPng;
        }

        /// <summary>
        /// Decodes the image from the specified stream and sets
        /// the data to image.
        /// </summary>
        /// <param name="image">The image, where the data should be set to.
        /// Cannot be null (Nothing in Visual Basic).</param>
        /// <param name="stream">The stream, where the image should be
        /// decoded from. Cannot be null (Nothing in Visual Basic).</param>
        /// <exception cref="ArgumentNullException">
        /// 	<para><paramref name="image"/> is null (Nothing in Visual Basic).</para>
        /// 	<para>- or -</para>
        /// 	<para><paramref name="stream"/> is null (Nothing in Visual Basic).</para>
        /// </exception>
        public async Task Decode(ExtendedImage image, Stream stream)
        {
            _image = image;

            _stream = stream;

            //var memStream = new InMemoryRandomAccessStream();
            //await _stream.CopyToAsync(memStream.AsStream());
            //memStream.Seek(0);

            var memStream = new InMemoryRandomAccessStream();
            var outputStream = memStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(_stream.AsInputStream(), outputStream); 

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
