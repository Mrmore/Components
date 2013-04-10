/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Renren.Components.Tester.WP8.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Renren.Components.Tester.Model.TestCases;
using Renren.Components.Tester.ViewModel.ImageTools;

namespace Renren.Components.Tester.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<ITestCasesProvider, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<ITestCasesProvider, TestCasesProvider>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AudioUgcViewModel>();
            SimpleIoc.Default.Register<FeedViewModel>();
            SimpleIoc.Default.Register<ImageToolsViewModel>();
            SimpleIoc.Default.Register<MultiUploadViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public static MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static AudioUgcViewModel Audios
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AudioUgcViewModel>();
            }
        }

        public static MultiUploadViewModel MultiUploader
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MultiUploadViewModel>();
            }
        }

        public static FeedViewModel Feeds
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FeedViewModel>();
            }
        }
        public static ImageToolsViewModel Images
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ImageToolsViewModel>();
            }
        }        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}