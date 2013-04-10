using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// One of implmementation of Http request token
    /// It contains:
    /// The handle action of response,
    /// The request message
    /// The response message
    /// </summary>
    /// <typeparam name="TResponse">
    /// The actual type of Http response expected</typeparam>
    /// <remarks>It should be created using http engine facade</remarks>
    public class HttpAsyncToken<TResponse> : IHttpAsyncToken
        where TResponse : INetworkRespMessage, new()
    {
        private static long _baseId = 1;
        private string _id = string.Empty;

        /// <summary>
        /// Internal constructor of Http token
        /// </summary>
        /// <param name="handler">The handle action at reponse return</param>
        /// <param name="owner">The owner of this token. 
        /// Note: it's used to build weak reference</param>
        /// <param name="request">The request message</param>
        /// <param name="method">The used http method</param>
        /// <param name="scheduler">The reference of secheduler</param>
        /// <param name="priority">The priority of this token</param>
        /// <param name="timeout">The time out for this token's request</param>
        /// <param name="progress">The progress indicator of this request</param>
        internal HttpAsyncToken(Action<INetworkAsyncToken> handler, 
            object owner, HttpRequestMessage request, HttpMethod method,
            INetworkTransitionScheduler scheduler,
            TransitionPriority priority,
            TimeSpan? timeout,
            IProgressIndicator<int> progress = null)
        {
            this.Handler = new WeakAction<INetworkAsyncToken>(owner, handler);
            this.Request = request;
            this.Progress = progress;
            this._id = Interlocked.Increment(ref _baseId).ToString();
            this.Scheduler = scheduler;
            this.NetType = NetworkType.Http;
            this.Priority = priority;
            this.ExpTimeout = timeout;
            this.Method = method;
        }

        /// <summary>
        /// This method is responsible for cancelling current token's request
        /// </summary>
        public void Cancel()
        {
            this.Scheduler.Cancel(this);
        }

        /// <summary>
        /// Set the byte array of raw data
        /// </summary>
        /// <param name="rawData">The byte array raw data</param>
        /// <param name="resp">The reponse taking the raw data</param>
        public void SetRawData(byte[] rawData, object resp = null)
        {

            TResponse respone = (TResponse)Activator.CreateInstance(typeof(TResponse));
            respone.Init(rawData, resp);
            this.Response = respone;
        }

        /// <summary>
        /// Set the Exception error met in request
        /// process
        /// </summary>
        /// <param name="ex">The exception error</param>
        public void SetException(Exception ex)
        {
            TResponse respone = (TResponse)Activator.CreateInstance(typeof(TResponse));
            respone.Init(ex);
            this.Response = respone;
        }

        /// <summary>
        /// Set token's status
        /// </summary>
        /// <param name="status">The status</param>
        public void SetStatus(NetworkStatus status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Getter and seter of http request's method
        /// </summary>
        public HttpMethod Method
        {
            get;
            set;
        }

        /// <summary>
        /// Getter and setter of http request message
        /// </summary>
        public HttpRequestMessage Request
        {
            get;
            private set;
        }

        /// <summary>
        /// Getter and setter of request progress indicator
        /// </summary>
        public IProgressIndicator<int> Progress
        {
            get;
            set;
        }

        /// <summary>
        /// Getter of the token's unique id
        /// It will generate during construct process
        /// </summary>
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Getter and setter of time out for
        /// this token's request
        /// </summary>
        public TimeSpan? ExpTimeout
        {
            get;
            set;
        }
        
        /// <summary>
        /// Getter and setter of http request message
        /// </summary>
        public INetworkRespMessage Response
        {
            get;
            private set;
        }

        /// <summary>
        /// Getter and setter of reponse handler action
        /// Note: it's a weakaction so you should set the correct owner
        /// </summary>
        public WeakAction<INetworkAsyncToken> Handler
        {
            get;
            private set;
        }

        /// <summary>
        ///  Getter and setter of an arbitrary object value that can be used to store
        ///  custom information about this object.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Getter and setter of this token's priority
        /// </summary>
        private TransitionPriority _curPriority = TransitionPriority.Default;
        public TransitionPriority Priority
        {
            get { return _curPriority; }
            set
            {
                if (!_curPriority.Equals(value))
                {
                    this.Scheduler.Update(this, value);
                }
            }
        }

        /// <summary>
        /// Getter and setter of this token's status
        /// </summary>
        public NetworkStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Getter and setter of the netwrok type used to transfer
        /// </summary>
        public NetworkType NetType
        {
            get;
            private set;
        }

        /// <summary>
        /// Getter and setter of the scheduler reference
        /// </summary>
        public INetworkTransitionScheduler Scheduler
        {
            get;
            private set;
        }
    }
}
