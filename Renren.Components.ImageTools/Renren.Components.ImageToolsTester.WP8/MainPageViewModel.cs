using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageTools.IO;
using ImageTools.IO.Jpeg;
using ImageTools.IO.Bmp;
using ImageTools.IO.Png;
using ImageTools.IO.Gif;

namespace Renren.Components.ImageToolsTester.WP8
{
    /// <summary>
    /// Simple view model that holds a property to the image source.
    /// </summary>
    public sealed class MainPageViewModel
    {
        private readonly Uri _imageSource = new Uri("Images/tiny1.jpg", UriKind.Relative);
        /// <summary>
        /// Gets or sets the path to the source image.
        /// </summary>
        /// <value>The path to the source image.</value>
        public Uri ImageSource
        {
            get { return _imageSource; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<JpegDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
            Encoders.AddEncoder<JpegEncoder>();
        }
    }
}
