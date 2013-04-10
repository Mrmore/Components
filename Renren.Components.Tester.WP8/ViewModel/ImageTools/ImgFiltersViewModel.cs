using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Tester.ViewModel.ImageTools
{
    class ImgFiltersViewModel
    {
        private readonly Uri _imageSource = new Uri("ms-appx:///Images/house2.jpg");

        public Uri ImageSource
        {
            get { return _imageSource; }
        }
    }
}
