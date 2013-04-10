using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Renren.Components.Network;
using Renren.Components.RestApis.Entities;
using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Diagnostics;
using Renren.Components.Async;

#if NETFX_CORE
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Tasks;
using System.Threading;
#endif

namespace Renren.Components.Tester.ViewModel
{
    public class MultiUploadViewModel : ViewModelBase
    {
        public MultiUploadViewModel()
        {
            ChoosePhotosCmd = new RelayCommand(async () => { try { await this.Choose(); } catch(Exception e) { Debug.WriteLine("ChoosePhotosCmd error: " + e.ToString()); }; });
            StartAllCommand = new RelayCommand(() => { try { this.Items.Foreach((p) => p.StartCommand.Execute(p)); } catch (Exception e) { Debug.WriteLine("StartAllCommand error: " + e.ToString()); }; });
        }

        private static AsyncLock _chooserLocker = null;
        private static AsyncLock _removeLocker = null;
        static MultiUploadViewModel()
        { 
            _chooserLocker = new AsyncLock();
            _removeLocker = new AsyncLock();
        }

        #if NETFX_CORE
        public async Task Choose()
        {
            clearItems();
            // Set up and launch the Open Picker
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            fileOpenPicker.CommitButtonText = "Enjoy it!";
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Clear();
            fileOpenPicker.FileTypeFilter.Add(".bmp");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".gif");

            List<StorageFile> photos = new List<StorageFile>();
            if (NeedMultiterm)
            {
                var files = await fileOpenPicker.PickMultipleFilesAsync();
                photos.AddRange(files);
            }
            else
            {
                StorageFile file = await fileOpenPicker.PickSingleFileAsync();
                photos.Add(file);
            }

            foreach (var photo in photos)
            {
                await addTerm(photo);
            }
        }
#else

        private static int sequence = 0;
        PhotoChooserTask _chooser = null;
        public async Task Choose()
        {
            //clearItems();
            if (_chooser == null)
            {
                // Set up and launch the Open Picker
                _chooser = new PhotoChooserTask();
                _chooser.ShowCamera = true;
                _chooser.Completed += async (object sender, PhotoResult e) =>
                {
                    Interlocked.Increment(ref sequence);
                    if (e.TaskResult == TaskResult.OK)
                    {
                        var photo = e.ChosenPhoto;
                        var extension = photo.TryCheckImageType().ToFileExtension();
                        string fileName = string.Format("multiterm-uploading.temp.photo.{0}{1}", sequence, extension);

                        var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("multiterm-uploading.temp",
                            CreationCollisionOption.OpenIfExists);
                        var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
                        using (var target = await file.OpenStreamForWriteAsync())
                        {
                            await photo.CopyToAsync(target);
                            await target.FlushAsync();
                        }

                        await addTerm(file);
                    }
                };
            }

            using (var release = await _chooserLocker.LockAsync())
            {
                _chooser.Show();
            }
        }
#endif

        private async Task addTerm(StorageFile photo)
        {
            var term = new PhotoTerm();
            term.Context = this;
            term.File = photo;

            term.CancelCommand = new RelayCommand<PhotoTerm>(
                async (p) =>
                {
                    if (p.Token != null) p.Token.Cancel();
                    await (p.Context as MultiUploadViewModel).RemoveTerm(p);
                }, (p) => (p != null));

            term.StartCommand = new RelayCommand<PhotoTerm>(
                async (p) =>
                {
                    var token = await RestApis.Photo.UploadBin<UploadPhoto>(
                    (t) =>
                    {
                        if (t.Status == Network.NetworkStatus.Completed &&
                            t.Response.Status == RespStatus.Succeed)
                        {
                            DispatcherHelper.RunAsync(async () => { await ((t.Tag as PhotoTerm).Context as MultiUploadViewModel).RemoveTerm(p); });
                        }
                        else
                        {
                            DispatcherHelper.RunAsync(() => (t.Tag as PhotoTerm).IsStarting = false);
                        }
                    },
                    p, p, p.File, -1, "Test");

                    token.Tag = p;
                    p.Token = token;
                    p.IsStarting = true;
                }, (p) => p != null && p.Token == null);

            term.Created = photo.DateCreated.ToString();

#if NETFX_CORE
            using (var thumbnail = await photo.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView))
            {
                term.Image = new BitmapImage();
                await term.Image.SetSourceAsync(thumbnail);
            }
#else
            term.Image = await toImage(photo);
#endif
            this.Items.Add(term);
        }

        public async Task RemoveTerm(PhotoTerm term)
        {
            // Since the ObservableCollection<T> is not a thread safe container,
            // so you should always ensure this concurrency operation safe
            using (var release = await _removeLocker.LockAsync())
            {
                try
                {
                    //lock (Items)
                    {
                        //int index = this.Items.IndexOf(term);
                        //if (index >= 0) this.Items.RemoveAt(index);

                        // Note: Seems ObservableCollection has an issue for deleting in windows phone 8,
                        // it will raise a out of range excetpion for last two items
                        // Who konws how to fix this issue?
                        Debug.WriteLine("Remove term with the total count: " + this.Items.Count);
#if !NETFX_CORE
                        if (this.Items.Count > 0) 
#endif
                            this.Items.Remove(term);
                    }

#if NETFX_CORE
#else
                    await term.File.DeleteAsync();
#endif
                }
                catch (Exception e) { Debug.WriteLine("RemoveTerm error: " + e.ToString()); };
            }
        }

        private async Task<BitmapImage> toImage(StorageFile file)
        {
            BitmapImage image = new BitmapImage();
#if NETFX_CORE
            await image.SetSourceAsync(await file.OpenReadAsync());
#else
            var tmpStream = await file.OpenReadAsync();
            image.SetSource(tmpStream.AsStreamForRead());
#endif
            return image;
        }

        private ObservableCollection<PhotoTerm> _items = new ObservableCollection<PhotoTerm>();
        public ObservableCollection<PhotoTerm> Items
        {
            get { return _items; }
            set { this.Set("Items", ref _items, value); }
        }

        private RelayCommand _command;
        public RelayCommand ChoosePhotosCmd
        {
            get { return _command; }
            set { this.Set("ChoosePhotosCmd", ref _command, value); }
        }

        private RelayCommand _startAll;
        public RelayCommand StartAllCommand
        {
            get { return _startAll; }
            set { this.Set("StartAllCommand", ref _startAll, value); }
        }

        private static bool _needMultiterm = true;
        public static bool NeedMultiterm
        {
            get { return _needMultiterm; }
            set { if (value != _needMultiterm) _needMultiterm = value; }
        }

        private void clearItems()
        {
            while (this.Items.Count > 0)
            {
                this.Items.RemoveAt(0);
            }
        }

    }

    public class PhotoTerm : BindableBase, IProgressIndicator<int>
    {
        private RelayCommand<PhotoTerm> _cancelCommand;
        public RelayCommand<PhotoTerm> CancelCommand
        {
            get { return _cancelCommand; }
            set { this.SetProperty(ref _cancelCommand, value, "CancelCommand"); }
        }

        private RelayCommand<PhotoTerm> _startCommand;
        public RelayCommand<PhotoTerm> StartCommand
        {
            get { return _startCommand; }
            set { this.SetProperty(ref _startCommand, value, "StartCommand"); }
        }

        private StorageFile _file;
        public StorageFile File
        {
            get { return _file; }
            set
            {
                this.SetProperty(ref _file, value, "File");
            }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { this.SetProperty(ref _progress, value, "Progress"); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { this.SetProperty(ref _description, value, "Description"); }
        }

        private bool _isStarting;
        public bool IsStarting
        {
            get { return _isStarting; }
            set { this.SetProperty(ref _isStarting, value, "IsStarting"); }
        }

        private INetworkAsyncToken _token;
        public INetworkAsyncToken Token
        {
            get { return _token; }
            set { this.SetProperty(ref _token, value, "Token"); }
        }

        private object _context;
        public object Context
        {
            get { return _context; }
            set { this.SetProperty(ref _context, value, "Context"); }
        }

        public PhotoTerm Term
        {
            get { return this; }
        }

        private BitmapImage _image = null;
        public BitmapImage Image
        {
            get { return _image; }
            set { this.SetProperty(ref _image, value, "Image"); }
        }

        private string _created;
        public string Created
        {
            get { return _created; }
            set { this.SetProperty(ref _created, value, "Created"); }
        }

        public void Report(object sender, int value, string description)
        {
            DispatcherHelper.RunAsync(() =>
            {
                this.Progress = value;
                this.Description = description;
            });

        }
    }
}
