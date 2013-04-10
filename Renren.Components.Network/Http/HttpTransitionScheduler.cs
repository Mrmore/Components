using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// The http request scheduler
    /// It's a default implementation of http netwrok requst scheduler
    /// </summary>
    /// <typeparam name="TTransition">Used to specify the transiton type</typeparam>
    public class HttpTransitionScheduler<TTransition> : INetworkTransitionScheduler, IDisposable
        where TTransition : INetworkTransition<INetworkAsyncToken, AutoResetEvent>, new()
    {
        class Channel
        {
            public AutoResetEvent CompletedFactor { get; set; }
            public INetworkTransition<INetworkAsyncToken, AutoResetEvent> Transition { get; set; }
            public INetworkAsyncToken CurrentToken { get; set; }
            public AutoResetEvent StopWaitor { get; set; }
        }

        private volatile bool _isRunnning = false;
        private SemaphoreSlim _syncRoot = null;
        private IDictionary<Task, Channel> _workItems = null;
        private ITokenArbitrationContainer<INetworkAsyncToken> _productsContainer = null;

        private static Lazy<HttpTransitionScheduler<TTransition>> _lazyImpl =
            new Lazy<HttpTransitionScheduler<TTransition>>(() => new HttpTransitionScheduler<TTransition>(), true);

        /// <summary>
        /// Internal default constructor
        /// </summary>
        internal HttpTransitionScheduler()
        { }

        /// <summary>
        /// Get the default implementation
        /// Note: it's a lazy creation property, so it will be created till at first call
        /// </summary>
        public static INetworkTransitionScheduler Default
        {
            get
            {
                return _lazyImpl.Value;
            }
        }

        /// <summary>
        /// Start the build transiton channel according injected configuration
        /// </summary>
        /// <param name="config">The injected configuration</param>
        public void Start(TransitionSchedulerConfig config)
        {
            if (this._isRunnning == false)
            {
                lock (this)
                {
                    if (this._isRunnning == false)
                    {
                        _isRunnning = true;
                        init();
                        for (int i = 0; i < config.ChannelCount; i++)
                        {
                            var item = new Channel()
                            {
                                CompletedFactor = new AutoResetEvent(true),
                                Transition =
                                (INetworkTransition<INetworkAsyncToken, AutoResetEvent>)
                                Activator.CreateInstance(typeof(TTransition)),

                                StopWaitor = new AutoResetEvent(false)
                            };

                            var task = kickoffOneTransitionChannel(item);
                            this._workItems.Add(task, item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stop this scheduler
        /// </summary>
        public void Stop()
        {
            if (this._isRunnning == true)
            {
                lock (this)
                {
                    if (this._isRunnning == true)
                    {
                        this._isRunnning = false;
                        cleanup();
                    }
                }
            }
        }

        /// <summary>
        /// Indicate if this scheduler is running
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunnning; }
        }

        /// <summary>
        /// Add new token to scheduler
        /// </summary>
        /// <param name="token">The token added</param>
        public void AddToken(INetworkAsyncToken token)
        {
            _productsContainer.AddToken(token);
        }

        /// <summary>
        /// Update the older token's priority
        /// </summary>
        /// <param name="older">The older token</param>
        /// <param name="newPriority">The new priority</param>
        public void Update(INetworkAsyncToken older, TransitionPriority newPriority)
        {
            _productsContainer.Update(older, newPriority);
        }

        /// <summary>
        /// Cancel the token specified
        /// </summary>
        /// <param name="token">The token specified</param>
        public void Cancel(INetworkAsyncToken token)
        {
            token.Handler.MarkForDeletion();
            cancelItem(token);
            token.SetStatus(NetworkStatus.Canceled);
        }

        /// <summary>
        /// Try to cancel the token specified asynchronously
        /// </summary>
        /// <param name="token">The token specified</param>
        /// <returns></returns>
        public async Task<bool> TryCancelAsync(INetworkAsyncToken token)
        {
            return await Task.Run<bool>(() =>
            {
                bool result = true;
                try
                {
                    token.Handler.MarkForDeletion();
                    cancelItem(token);
                    token.SetStatus(NetworkStatus.Canceled);
                }
                catch (Exception)
                {
                    result = false;
                }

                return result;
            });
        }

        /// <summary>
        /// Dispose implementation
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Kick off one transition channel
        /// </summary>
        /// <param name="channel">The channel context</param>
        /// <returns></returns>
        private Task kickoffOneTransitionChannel(Channel channel)
        {
            return Task.Factory.StartNew(() =>
            {
                while (this._isRunnning == true)
                {
                    channel.CompletedFactor.WaitOne();

                    if (this._isRunnning == false)
                    {
                        break;
                    }

                    var token = this._productsContainer.Next();

                    if (token != null &&
                        token.Handler.IsAlive &&
                        token.Status == NetworkStatus.Waitting &&
                        this._isRunnning == true)
                    {
                        channel.CurrentToken = token;
                        channel.Transition.Send(token, channel.CompletedFactor);
                    }
                }

                channel.StopWaitor.Set();
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Initialation function
        /// </summary>
        private void init()
        {
            _workItems = new Dictionary<Task, Channel>();
            _syncRoot = new SemaphoreSlim(0);
            _productsContainer = new TokenArbitrationContainer(_syncRoot);
        }

        /// <summary>
        /// Clean up the current scheduler
        /// </summary>
        private void cleanup()
        {
            try
            {
                List<AutoResetEvent> allStopWaitors = new List<AutoResetEvent>();
                foreach (var item in this._workItems)
                {
                    item.Value.CompletedFactor.Set();
                    _syncRoot.Release(_syncRoot.CurrentCount);
                    allStopWaitors.Add(item.Value.StopWaitor);
                }

                AutoResetEvent.WaitAll(allStopWaitors.ToArray());

                _syncRoot = null;
                _workItems = null;
                _productsContainer = null;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Wait for all work items completion
        /// </summary>
        /// <returns></returns>
        private IEnumerator<AutoResetEvent> getAllWaitors()
        {
            foreach (var item in this._workItems)
            {
                yield return item.Value.CompletedFactor;
            }
        }

        /// <summary>
        /// Cancel a item pendding in requst
        /// </summary>
        /// <param name="token">The pendding token</param>
        private void cancelItem(INetworkAsyncToken token)
        {
            foreach (var item in this._workItems)
            {
                if (item.Value.CurrentToken.Equals(token))
                {
                    item.Value.Transition.Cancel
                        (
                        token,
                        item.Value.CompletedFactor
                        );
                    break;
                }
            }
        }
    }
}
