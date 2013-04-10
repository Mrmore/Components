using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Renren.Components.Tester.ViewModel;

namespace Renren.Components.Tester.WP8.Views
{
    public partial class AudioUGCPage : PhoneApplicationPage
    {
        public AudioUGCPage()
        {
            InitializeComponent();

            this.Loaded += (s, e) => ViewModelLocator.Audios.SetPlayer(player);
        }

    }
}