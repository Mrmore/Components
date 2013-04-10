using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.ImageToolsTester.Win8
{
    class ImageShowViewModel
    {
        private readonly Uri _bmpimageSource = new Uri("ms-appx:///Images/book.bmp");
        private readonly Uri _jpgimageSource = new Uri("ms-appx:///Images/baby.jpg");
        private readonly Uri _pngimageSource = new Uri("ms-appx:///Images/wood.png");
        private readonly Uri _gifImageSource = new Uri("ms-appx:///Images/Micky.gif");
        private List<GifEnitty> _gifs = new List<GifEnitty>();

        public class GifEnitty
        {
            public Uri GifImageSource { get; set; }
        }

        public Uri GifImageSource
        {
            get { return _gifImageSource; }
        }

        public Uri BmpImageSource
        {
            get { return _bmpimageSource; }
        }

        public Uri JpgImageSource
        {
            get { return _jpgimageSource; }
        }

        public Uri PngImageSource
        {
            get { return _pngimageSource; }
        }

        public ICollection<GifEnitty> Gifs
        {
            get { return _gifs; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageShowViewModel"/> class.
        /// </summary>
        public ImageShowViewModel()
        {
        }
    }
}
