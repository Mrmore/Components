using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Renren.Components.RestApis;
using Renren.Components.RestApis.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PlayerFramework;
using Renren.Components.Network;
using Renren.Components.Tools;
using Renren.Components.IO;


#if NETFX_CORE
using Windows.UI.Xaml.Media;
using Windows.UI;
#else
using System.Windows.Media;
using System.IO.IsolatedStorage;
#endif

namespace Renren.Components.Tester.ViewModel
{
    public class AudioUgcViewModel : ViewModelBase
    {
        public AudioUgcViewModel()
        {
            Command = new RelayCommand(() => this.Load());
        }

        private ObservableCollection<AudioItem> _items = new ObservableCollection<AudioItem>();
        public ObservableCollection<AudioItem> Items
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

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { this.Set("IsLoading", ref _isLoading, value); this.RaisePropertyChanged("IsLoading"); }
        }

        private static bool _needDownload = true;
        public static bool NeedDownload
        {
            get { return _needDownload; }
            set { if (value != _needDownload) _needDownload = value; }
        }

        private WeakReference _player = null;
        public void SetPlayer(MediaPlayer player)
        {
            Guarder.NotNull(player, "The player injected should not be null!");

            this._player = new WeakReference(player);

            if (IsPlayerAlive)
            {
                var mediaPlayer = this._player.Target as MediaPlayer;
                mediaPlayer.TimeRemainingChanged -= _player_TimeRemainingChanged;
                mediaPlayer.TimeRemainingChanged += _player_TimeRemainingChanged;

                mediaPlayer.PlayerStateChanged -= _player_PlayerStateChanged;
                mediaPlayer.PlayerStateChanged += _player_PlayerStateChanged;
            }

            this.Items.Foreach((item) => item.Player = this._player);
        }

        void _player_PlayerStateChanged(object sender, object e)
        {
            if (IsPlayerAlive)
            {
                var mediaPlayer = this._player.Target as MediaPlayer;
                if (mediaPlayer.PlayerState == PlayerState.Ending)
                {
                    var audio = (sender as MediaPlayer).Tag;
                    if (audio != null && audio is AudioItem)
                    {
                        (audio as AudioItem).CountdownReset();
                    }
                }
            }
        }

        void _player_TimeRemainingChanged(object sender, object e)
        {
            if (IsPlayerAlive)
            {
                var audio = (sender as MediaPlayer).Tag;
                if (audio != null && audio is AudioItem &&
                    (sender as MediaPlayer).CurrentState == MediaElementState.Playing)
                {
                    (audio as AudioItem).Countdown = (sender as MediaPlayer).TimeRemaining.Seconds.ToString();
                }
            }
        }

        public void Load()
        {
            clearItems();
            this.IsLoading = true;

            var feedToken = RestApis.Feed.Get<FeedList>(
            (t) =>
            {
                if (t.Status == Network.NetworkStatus.Completed &&
                    t.Response.Status == RespStatus.Succeed)
                {
                    var result = t.Response as RenrenRespMessage<FeedList, RenrenRemoteError>;
                    if (result.Result.Feeds == null) { DispatcherHelper.RunAsync(() => this.IsLoading = false); return; }

                    var queries = from feed in result.Result.Feeds
                                  where (feed.Type == 504 || feed.Type == 701) && feed.Voice != null
                                  select feed;

                    foreach (var item in queries)
                    {
                        //var src = item.AttachementList.FirstOrDefault((a) => a.Type == "audio");
                        var src = item.Voice;

                        var audio = new AudioItem();
                        audio.Audio = audio;
                        audio.Player = _player;
                        audio.Feed = item;
                        audio.Line1 = "URL: " + src.VoiceUrl;
                        audio.Line2 = "Length: " + src.VoiceLength.ToString();
                        audio.Countdown = src.VoiceLength.ToString();
                        audio.Command = new RelayCommand<AudioItem>((a) =>
                        {
                            (a.Player.Target as MediaPlayer).ConnectAudioItem(a);
                        }, (a) => a != null && a.Player != null && a.Player.IsAlive && a.Audio != null);

                        DispatcherHelper.RunAsync(() =>
                            {
                                Items.Add(audio);
                            });
                    }

                    DispatcherHelper.RunAsync(() => this.IsLoading = false);
                }
            }, this, 1, 1000);
        }

        private void clearItems()
        {
            while (this.Items.Count > 0)
            {
                this.Items.RemoveAt(0);
            }
        }

        public bool IsPlayerAlive
        {
            get
            {
                if (_player == null)
                {
                    return false;
                }

                return _player.IsAlive;
            }
        }
    }

    public class AudioItem : BindableBase
    {
        public WeakReference Player { get; set; }

        private string line1;
        public string Line1
        {
            get { return line1; }
            set { this.SetProperty(ref line1, value, "Line1"); }
        }

        private string line2;
        public string Line2
        {
            get { return line2; }
            set { this.SetProperty(ref line2, value, "Line2"); }
        }

        private string countdown;
        public string Countdown
        {
            get { return countdown; }
            set { this.SetProperty(ref countdown, value, "Countdown"); }
        }

        private string playing = "Play";
        public string Playing
        {
            get { return playing; }
            set { this.SetProperty(ref playing, value, "Playing"); this.OnPropertyChanged("PlayingColor"); }
        }

        public SolidColorBrush PlayingColor
        {
            get { return Playing == "Play" ? new SolidColorBrush() { Color = Colors.Black } :
                new SolidColorBrush() { Color = Colors.Green };
            }
        }

        private RelayCommand<AudioItem> command;
        public RelayCommand<AudioItem> Command
        {
            get { return command; }
            set { this.SetProperty(ref command, value, "Command"); }
        }

        private FeedEntity feed;
        public FeedEntity Feed
        {
            get { return feed; }
            set { this.SetProperty(ref feed, value, "Feed"); }
        }

        private AudioItem audio;
        public AudioItem Audio
        {
            get { return audio; }
            set { this.SetProperty(ref audio, value, "Audio"); }
        }

        public void CountdownReset()
        {
            Countdown = Feed.Voice.VoiceLength.ToString();
            Playing = "Play";
        }
    }

    public static class MediaPlayerExtension
    {
        public static void DisconnectAudioItem(this MediaPlayer player)
        {
            // Disconnect the older one
            var older = player.Tag;
            player.Tag = null;

            player.Stop();
 
            // Reset the older countdown
            if (older != null && older is AudioItem)
            {
                (older as AudioItem).CountdownReset();
            }
        }

        public async static void ConnectAudioItem(this MediaPlayer player, AudioItem item)
        {
            var older = player.Tag;
            var newer = item;

            if (object.ReferenceEquals(older, newer))
            {
                // Switch play and pause state
                player.SwitchPlayPause(newer);
            }
            else
            {
                player.DisconnectAudioItem();

                var uri = new Uri(item.Feed.Voice.VoiceUrl, UriKind.Absolute);

                // You can assign the uri to the source property directly
                if (!AudioUgcViewModel.NeedDownload)
                    player.Source = uri;
                else
                {
                    // Or you can download the content firstly, and set the source as stream then.
#if NETFX_CORE
                    player.SetSource(await uri.DownloadAsRandomStream(), "");
#else
                    var stream = await uri.DownloadAsIsoStream();
                    player.SetSource(stream);
#endif
                }
                player.Play();
                player.Tag = item;
                item.Playing = "Pause";
            }
        }

        public static void SwitchPlayPause(this MediaPlayer player, AudioItem item)
        {
            if (player.CurrentState == MediaElementState.Playing)
            {
                player.Pause();
                item.Playing = "Play";
            }
            else
            {
                player.Play();
                item.Playing = "Pause";
            }
        }
    }
}
