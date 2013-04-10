using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// The facade class of this network library, All http network operations are performed through this class.
    /// It's in charge of: Create token, Send token, Cancel token
    /// </summary>
    /// <typeparam name="TTransition">Injected type for creating adaptive tansition</typeparam>
    /// <remarks>It uses the standard initialization, so you can use the CreateInitedInstance interface
    /// to create the initialization ready object
    /// </remarks>
    public class HttpTransferEngine<TTransition> : IInitializeStandardized<HttpTransferEngine<TTransition>>
        where TTransition : INetworkTransition<INetworkAsyncToken, AutoResetEvent>, new()
    {
        private INetworkTransitionScheduler _scheduler = null;
        private int _defaultTimeout = 5;

        /// <summary>
        /// Send special token to request waitting queue
        /// </summary>
        /// <param name="token"></param>
        public void SendToken(INetworkAsyncToken token)
        {
            _scheduler.AddToken(token);
        }

        /// <summary>
        /// Send special token to request waitting queue async
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SendTokenAsync(INetworkAsyncToken token)
        {
            await Task.Run(() =>
            {
                _scheduler.AddToken(token);
            });
        }

        /// <summary>
        /// Cancel special token's request
        /// </summary>
        /// <param name="token">Th</param>
        /// <remarks>Note: the canncel operation depends on status of this special
        /// token. If this token waitting in the request queue, it will be remove from the queue.
        /// If this token pendding in the http request process, it will be aborted.</remarks>
        public void CancelToken(INetworkAsyncToken token)
        {
            token.Cancel();
        }

        /// <summary>
        /// The only method to create a special token
        /// It is a factory method to create the token and you can customize it later
        /// </summary>
        /// <typeparam name="TResponse">Specify the response type</typeparam>
        /// <param name="handler">The response handler action</param>
        /// <param name="owner">Specify the owner of this token</param>
        /// <param name="request">Specify the request message of this token</param>
        /// <param name="priority">Specify the priority of this token</param>
        /// <param name="timeout">Specify the time you allow to waitting for response</param>
        /// <param name="progress">Specify the progress indicator</param>
        /// <returns>The token created</returns>
        /// <remarks>The handler and progress should not be null meanwhile.
        /// The handler and owner should be injected together
        /// The request message should not be null</remarks>
        public INetworkAsyncToken CreateToken<TResponse>(Action<INetworkAsyncToken> handler,
            object owner, HttpRequestMessage request,
            TransitionPriority priority = null,
            TimeSpan? timeout = null,
            IProgressIndicator<int> progress = null) where TResponse : INetworkRespMessage, new()
        {
            Guarder.NotNull(_scheduler, "You should create token after HttpTransferEngine's initialization!");
            Guarder.AtLeastOneNotNull(handler, progress, 
                "You should always inject handler or progress at least one reference here, for reporting the result!");
            Guarder.InjectTogether(handler, owner, "The owner of NetworkAsyncToken and handler should inject together!");
            Guarder.NotNull(request, "The request of NetworkAsyncToken should not be null!");

            timeout = timeout ?? TimeSpan.FromSeconds(_defaultTimeout);
            priority = priority ?? TransitionPriority.Default;

            INetworkAsyncToken token = new HttpAsyncToken<TResponse>(
                handler, owner, request, request.Method, _scheduler, priority, timeout, progress);

            return token;
        }

        /// <summary>
        /// The standarlized initializaton method
        /// </summary>
        /// <returns>The task can be awaited</returns>
        public async override Task Initialize()
        {
            await Task.Run(() =>
            {
                _scheduler = HttpTransitionScheduler<TTransition>.Default;

                // You can composit the configuration somewhere
                TransitionSchedulerConfig config = new TransitionSchedulerConfig() 
                { ChannelCount = 5, DefaultTimeout = 5 };

                _defaultTimeout = config.DefaultTimeout;
                _scheduler.Start(config);
            });
        }
    }
}
