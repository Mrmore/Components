// ===============================================================================
// PngEncoder.cs
// .NET Image Tools
// ===============================================================================
// Copyright (c) .NET Image Tools Development Group. 
// All rights reserved.
// ===============================================================================


using ImageTools.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace ImageTools.IO.Png
{
    /// <summary>
    /// Image encoder for writing image data to a stream in png format.
    /// </summary>
    public class PngEncoder : IImageEncoder
    {
        #region Constants

        private const int MaxBlockSize = 0xFFFF;

        #endregion

        #region Fields

        private Stream _stream;
        private ExtendedImage _image;

        #endregion

        #region IImageEncoder Members

        /// <summary>
        /// Gets or sets a value indicating whether this encoder
        /// will write the image uncompressed the stream.
        /// </summary>
        /// <value>
        /// <c>true</c> if the image should be written uncompressed to
        /// the stream; otherwise, <c>false</c>.
        /// </value>
        public bool IsWritingUncompressed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is writing
        /// gamma information to the stream. The default value is false.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is writing gamma 
        /// information to the stream.; otherwise, <c>false</c>.
        /// </value>
        public bool IsWritingGamma { get; set; }

        /// <summary>
        /// Gets or sets the gamma value, that will be written
        /// the the stream, when the <see cref="IsWritingGamma"/> property
        /// is set to true. The default value is 2.2f.
        /// </summary>
        /// <value>The gamma value of the image.</value>
        public double Gamma { get; set; }

        /// <summary>
        /// Gets the default file extension for this encoder.
        /// </summary>
        /// <value>The default file extension for this encoder.</value>
        public string Extension
        {
            get { return "PNG"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PngEncoder"/> class.
        /// </summary>
        public PngEncoder()
        {
            Gamma = 2.2f;
        }

        /// <summary>
        /// Indicates if the image encoder supports the specified
        /// file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns><c>true</c>, if the encoder supports the specified
        /// extensions; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="extension"/>
        /// is null (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException"><paramref name="extension"/> is a string
        /// of length zero or contains only blanks.</exception>
        public bool IsSupportedFileExtension(string extension)
        {
            Guard.NotNullOrEmpty(extension, "extension");

#if NETFX_CORE
            string extensionAsUpper = extension.ToUpper();
#else
            string extensionAsUpper = extension.ToUpper(CultureInfo.CurrentCulture);
#endif
            return extensionAsUpper == "PNG";
        }

        /// <summary>
        /// Encodes the data of the specified image and writes the result to
        /// the specified stream.
        /// </summary>
        /// <param name="image">The image, where the data should be get from.
        /// Cannot be null (Nothing in Visual Basic).</param>
        /// <param name="stream">The stream, where the image data should be written to.
        /// Cannot be null (Nothing in Visual Basic).</param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="image"/> is null (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="stream"/> is null (Nothing in Visual Basic).</para>
        /// </exception>
        public async Task Encode(ExtendedImage image, Stream stream)
        {
            Guard.NotNull(image, "image");
            Guard.NotNull(stream, "stream");

            _image = image;
            _stream = stream;

            var memStream = new InMemoryRandomAccessStream();
            BitmapEncoder encoder;
            encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, memStream);

            encoder.SetPixelData(
                _image.PixelFormat,
                _image.AlphaMode,
                (uint)_image.PixelWidth,
                (uint)_image.PixelHeight,
                _image.DensityX,
                _image.DensityY,
                _image.Pixels
                );

            await encoder.FlushAsync();

            await memStream.AsStream().CopyToAsync(stream);
        }

        #endregion
    }
}
