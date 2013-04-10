using ImageTools;
using ImageTools.Controls;
using Renren.Components.Tester.ViewModel.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace Renren.Components.Tester.Win8.Views.ImageTools
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class ImageEdit : Renren.Components.Tester.Win8.Common.LayoutAwarePage
    {
        public ImageEdit()
        {
            this.InitializeComponent();
            //this.DataContext = new ImgEditViewModel();
            //Container.Source = new ExtendedImage();
            //Container.Source.UriSource = new Uri("ms-appx:///Images/meng.jpeg");

        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="pageState">要使用可序列化状态填充的空字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".gif");

            // Open a stream for the selected file
            StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    ExtendedImage extendedImage = new ExtendedImage();
                    Stream selectedImg = await file.OpenStreamForReadAsync();
                    await extendedImage.SetSource(selectedImg);
                    Container.Source = extendedImage;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }
        }

        //private void Scale_Plus_Click(object sender, RoutedEventArgs e)
        //{
        //    Container.ScalingMode = ImageEditorScalingMode.FixedScaling;
        //    Container.Scaling = Math.Max(0.1, Math.Round(Container.Scaling + 0.1, 2));
        //}

        //private void SacleMinus_btn_Click(object sender, RoutedEventArgs e)
        //{
        //    Container.ScalingMode = ImageEditorScalingMode.FixedScaling;
        //    Container.Scaling = Math.Max(0.1, Math.Round(Container.Scaling - 0.1, 2));
        //}

        private void FlipX_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Container.Source != null && Container.Source.IsFilled)
            {
                Container.Source = ExtendedImage.Transform(Container.Source, RotationType.None, FlippingType.FlipX);
            }
        }

        private void FlipY_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Container.Source != null && Container.Source.IsFilled)
            {
                Container.Source = ExtendedImage.Transform(Container.Source, RotationType.None, FlippingType.FlipY);
            }
        }

        private void RotationLeft_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Container.Source != null && Container.Source.IsFilled)
            {
                Container.Source = ExtendedImage.Transform(Container.Source, RotationType.Rotate270, FlippingType.None);
            }
        }

        //private async void SaveAs_Click(object sender, RoutedEventArgs e)
        //{
        //    ExtendedImage extendedImage = Container.Source;
        //    FileSavePicker picker = new FileSavePicker();
        //    picker.FileTypeChoices.Add("JPEG image", new string[] { ".jpg" });
        //    picker.FileTypeChoices.Add("PNG image", new string[] { ".png" });
        //    picker.FileTypeChoices.Add("BMP image", new string[] { ".bmp" });
        //    picker.FileTypeChoices.Add("GIF image", new string[] { ".gif" });
        //    picker.DefaultFileExtension = ".jpg";
        //    picker.SuggestedFileName = "Output file";
        //    picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        //    StorageFile outputFile = await picker.PickSaveFileAsync();
            

        //    if (outputFile != null)
        //    {
        //        using(Stream stream = await outputFile.OpenStreamForWriteAsync())
        //        {
        //            extendedImage.WriteToStream(stream, outputFile.Name);
        //        }
        //    }
        //}

        private void RotationRight_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Container.Source != null && Container.Source.IsFilled)
            {
                Container.Source = ExtendedImage.Transform(Container.Source, RotationType.Rotate90, FlippingType.None);
            }
        }

        //private void ScalePage_Click(object sender, RoutedEventArgs e)
        //{
        //    Container.ScalingMode = ImageEditorScalingMode.Auto;
        //}
    }
}
