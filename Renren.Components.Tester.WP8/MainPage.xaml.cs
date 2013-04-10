using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Renren.Components.Tester.WP8.Resources;
using Renren.Components.RestApis.Entities;
using GalaSoft.MvvmLight.Threading;
using Renren.Components.Async;
using System.Threading.Tasks;
using Renren.Components.Tester.Model.TestCases;
using Renren.Components.RestApis;
using Renren.Components.Tester.ViewModel;

namespace Renren.Components.Tester.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += (s, e) => DispatcherHelper.Initialize();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void ListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            if ((MainLongListSelector.SelectedItem as TestCase).Id != 0 && SocialSDKContext.LoginContext == null)
                return;

            var item = MainLongListSelector.SelectedItem as TestCase;
            ViewModelLocator.Main.CurrentCase = item;

            if (item.NeedJumpPage)
            {
                App.RootFrame.Navigate(new Uri(item.JumpPageUri, UriKind.Relative));
            }
            else
            {
                item.RunAction(item, this);
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}