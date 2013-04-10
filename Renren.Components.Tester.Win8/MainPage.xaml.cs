using GalaSoft.MvvmLight.Threading;
using Renren.Components.RestApis;
using Renren.Components.RestApis.Entities;
using Renren.Components.Tester.Model.TestCases;
using Renren.Components.Tester.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Renren.Components.Async;

namespace Renren.Components.Tester.Win8
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => DispatcherHelper.Initialize();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void TestCaseSelectionChanged(object sender, ItemClickEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (e.ClickedItem == null)
                return;

            if ((e.ClickedItem as TestCase).Id != 0 && SocialSDKContext.LoginContext == null)
                return;

            var item = e.ClickedItem as TestCase;
            ViewModelLocator.Main.CurrentCase = item;

            if (item.NeedJumpPage)
            {
                this.Frame.Navigate(item.JumpPageType);
            }
            else
            {
                item.RunAction(item, this);
            }
        }
    }
}
