using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Jpeg;
using ImageTools.IO.Png;
using Renren.Components.Caching;
using Renren.Components.Caching.BackingStore;
using Renren.Components.Caching.Expirations;
using Renren.Components.Network;
using Renren.Components.RestApis;
using Renren.Components.RestApis.Entities;
using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Renren.Components.Tester.ViewModel
{
    public class FeedViewModel :ViewModelBase
    {
        static FeedViewModel() { Task.Run(() => Initial()); }

        static private CacheManager manager;

        public FeedViewModel()
        {
            Command = new RelayCommand(() => this.Load());   
        }

        private ObservableCollection<FeedItem> _items = new ObservableCollection<FeedItem>();
        public ObservableCollection<FeedItem> Items
        {
            get { return _items; }
            set { this.Set("Items", ref _items, value); }
        }

        private RelayCommand _command;
        public RelayCommand Command
        {
            get { return _command; }
            set { this.Set("Command", ref _command, value); }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { this.Set("IsChecked", ref isChecked, value); this.RaisePropertyChanged("IsChecked"); }
        }

        public async void Load()
        {
            clearItems();
            this.IsChecked = true;
            await Initial();
            var loginToken = RestApis.Feed.Get<FeedList>(
            async (t) =>
            {
                if (t.Status == Network.NetworkStatus.Completed &&
                    t.Response.Status == RespStatus.Succeed)
                {

                    var result = t.Response as RenrenRespMessage<FeedList, RenrenRemoteError>;
                    if (result.Result.Feeds == null) {await DispatcherHelper.RunAsync(() =>this.IsChecked = false); return; }
                    ICacheItemExpiration[] ex = {new AbsoluteTime(DateTime.Now.AddMinutes(1))};
                    await manager.Add<FeedList>("feed", result.Result, CacheItemPriority.High, new DefaultRefreshAction(), ex);
                    foreach (var item in result.Result.Feeds)
                    {
                        var feed = new FeedItem();

                        feed.HeadImg = new Uri(item.HeadUrl, UriKind.RelativeOrAbsolute);
                        feed.UserName = item.UserName;
                        feed.State = item.Title;
                        feed.Date = TimeConverter(item.Time);

                        await DispatcherHelper.RunAsync(() =>
                        {
                            Items.Add(feed);
                        });
                    }
                    
                }
                else
                {
                    Debug.WriteLine("Start  " + DateTime.Now.ToString("HH-mm-ss fff"));
                    var result = await manager.GetData<FeedList>("feed");
                    Debug.WriteLine("End   "+ DateTime.Now.ToString("HH-mm-ss fff"));
                    if (result == null)
                    {

                        var feed = new FeedItem();
                        feed.HeadImg = new Uri("ms-appx:///Images/meng.jpeg");
                        feed.UserName = "缓存已删除";
                        feed.State = "缓存已删除";
                        feed.Date = DateTime.Now.ToString();
                        DispatcherHelper.RunAsync(() =>
                        {
                            Items.Add(feed);
                        });
                    }
                    else
                    {
                        foreach (var item in result.Feeds)
                        {
                            var feed = new FeedItem();

                            feed.HeadImg = new Uri(item.HeadUrl, UriKind.RelativeOrAbsolute);
                            feed.UserName = item.UserName;
                            feed.State = item.Title;
                            feed.Date = TimeConverter(item.Time);

                            DispatcherHelper.RunAsync(() =>
                            {
                                Items.Add(feed);
                            });
                        }
                    }
                    
                }
                DispatcherHelper.RunAsync(() =>
                {
                    IsChecked = false;
                });
                
            }, this, 1, 1000);         
        }

        private void clearItems()
        {
            while (this.Items.Count > 0)
            {
                this.Items.RemoveAt(0);
            }
        }

        public static async Task Initial()
        {        
            CacheConfigSetting setting = CacheConfigSetting.CreatSetting("feed");
            manager = await CacheManager.CreateInitedInstance(setting, setting.Key);
        }

        public static string TimeConverter(long time)
        {
            if (time < 0)
            {
                return string.Empty;
            }
            string dateResult = string.Empty;
            string recentUpdate = "刚刚更新";
            string minuteBefore = "分钟前";

            //时间维度：

            //1分钟内 刚刚更新

            //1小时内 xx分钟前

            //今天内：今天XX：XX

            //昨天内：昨天XX：XX

            //更早日期：X月X日 XX：XX

            //一年前更早: xx年X月X日 XX:XX

            time *= 10000;

            TimeSpan ts = new TimeSpan(time);

            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.Add(ts);
            dt = dt.AddHours(8);


            TimeSpan timeSpan = DateTime.Now - dt;

            if (null != recentUpdate && null != minuteBefore)
            {
                if ((timeSpan.TotalMinutes <= 1))
                {
                    dateResult = recentUpdate;
                }
                else if (timeSpan.TotalMinutes > 1 && timeSpan.TotalMinutes < 60)
                {
                    dateResult = (int)timeSpan.TotalMinutes + minuteBefore;
                }
                else if (timeSpan.TotalMinutes >= 60 && timeSpan.TotalMinutes <= DateTime.Now.Hour * 60 + DateTime.Now.Minute)
                {
                    dateResult = dt.ToString("今天 HH:mm");
                }
                else if (timeSpan.TotalMinutes <= DateTime.Now.Hour * 60 + DateTime.Now.Minute + 1440)
                {
                    dateResult = dt.ToString("昨天 HH:mm");
                }
                //else if (timeSpan.TotalDays <= DateTime.Now.Day )
                //{
                //    dateResult = dt.ToString("MM-dd  HH:mm");
                //}
                else
                {
                    dateResult = dt.ToString("MM月dd日 HH:mm");
                    //dateResult = dt.ToString("yyyy-MM-dd  HH:mm");
                }
            }
            else
            {
                if (dt.Date != DateTime.Now.Date)
                {
                    dateResult = dt.ToString("yyyy-MM-dd");
                }
                else
                {
                    dateResult = dt.ToString("HH:mm");
                }
            }
            return dateResult;
        }
    }



    public class FeedItem : BindableBase
    {
        public FeedItem()
        {
           HeadImg =  new Uri("ms-appx:///Images/meng.jpeg");
        }

        private Uri headImg;
        public Uri HeadImg
        {
            get { return headImg; }
            set { this.SetProperty(ref headImg, value, "HeadImg"); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { this.SetProperty(ref userName, value, "UserName"); }
        }

        private string state;
        public string State
        {
            get { return state; }
            set { this.SetProperty(ref state, value, "State"); }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set { this.SetProperty(ref date, value, "Date"); }
        }
    }
}
