﻿// ===============================================================================
// JpegEncoder.cs
// .NET Image Tools
// ===============================================================================
// Copyright (c) .NET Image Tools Development Group. 
// All rights reserved.
// ===============================================================================

using ImageTools.Helpers;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI;
using System;
using Windows.Graphics.Imaging;

namespace ImageTools.IO.Jpeg
{

    /// <summary>
    /// Encoder for writing the data image to a stream in jpg format.
    /// </summary>
    public class JpegEncoder : IImageEncoder
    {
        #region Properties

        private Color _transparentColor = Color.FromArgb(0, 255, 255, 255);
        /// <summary>
        /// Gets or sets the color that will be used, when the source pixel is transparent.
        /// The default transparent color is white.
        /// </summary>
        /// <value>The color of the transparent that will be used, 
        /// when the source pixel is transparent.</value>
        public Color TransparentColor
        {
            get { return _transparentColor; }
            set { _transparentColor = value; }
        }

        private int _quality = 100;
        /// <summary>
        /// Gets or sets the quality, that will be used to encode the image. Quality 
        /// index must be between 0 and 100 (compression from max to min). 
        /// </summary>
        /// <value>The quality of the jpg image from 0 to 100.</value>
        public int Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        #endregion

        #region IImageEncoder Members

        /// <summary>
        /// Gets the default file extension for this encoder.
        /// </summary>
        /// <value>The default file extension for this encoder.</value>
        public string Extension
        {
            get { return "JPG"; }
        }

        /// <summary>
        /// Indicates if the image encoder supports the specified
        /// file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns>
        /// 	<c>true</c>, if the encoder supports the specified
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
        /// Encodes the data of the specified image and writes the result to
        /// the specified stream.
        /// </summary>
        /// <param name="image">The image, where the data should be get from.
        /// Cannot be null (Nothing in Visual Basic).</param>
        /// <param name="stream">The stream, where the image data should be written to.
        /// Cannot be null (Nothing in Visual Basic).</param>
        /// <exception cref="System.ArgumentNullException">
        /// 	<para><paramref name="image"/> is null (Nothing in Visual Basic).</para>
        /// 	<para>- or -</para>
        /// 	<para><paramref name="stream"/> is null (Nothing in Visual Basic).</para>
        /// </exception>
        public async Task Encode(ExtendedImage image, Stream stream)
        {
            Guard.NotNull(image, "image");
            Guard.NotNull(stream, "stream");

            var memStream = new InMemoryRandomAccessStream();
            BitmapEncoder encoder;
            encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, memStream);

            encoder.SetPixelData(
                image.PixelFormat,
                image.AlphaMode,
                (uint)image.PixelWidth,
                (uint)image.PixelHeight,
                image.DensityX,
                image.DensityY,
                image.Pixels
                );

            await encoder.FlushAsync();

            await memStream.AsStream().CopyToAsync(stream);
        }

        #endregion
    }
}
