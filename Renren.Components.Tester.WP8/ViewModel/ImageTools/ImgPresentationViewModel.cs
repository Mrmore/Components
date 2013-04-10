using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Jpeg;
using ImageTools.IO.Png;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Tester.ViewModel.ImageTools
{
    class ImgPresentationViewModel
    {
        private readonly Uri _bmpimageSource = new Uri("ms-appx:///Images/book.bmp");
        private readonly Uri _jpgimageSource = new Uri("ms-appx:///Images/meng.jpeg");
        private readonly Uri _pngimageSource = new Uri("ms-appx:///Images/wood.png");
        private readonly Uri _gifImageSource = new Uri("ms-appx:///Images/Micky.gif");

        private readonly Uri _orgImageSource = new Uri("ms-appx:///Images/house2.jpg");

        public Uri OrgImageSource
        {
            get { return _orgImageSource; }
        }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ImgPresentationViewModel"/> class.
        /// </summary>
        public ImgPresentationViewModel()
        {
        }
    }
}
