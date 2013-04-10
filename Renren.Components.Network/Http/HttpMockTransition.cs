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
    /// It's a mock implementation of http transition
    /// </summary>
    public class HttpMockTransition : INetworkTransition<INetworkAsyncToken, EventWaitHandle>
    {
        EventWaitHandle _factor = null;
        INetworkAsyncToken _curToken = null;
        HttpWebRequest _curRqeust = null;

        public HttpMockTransition() { }

        public void Send(INetworkAsyncToken token, EventWaitHandle factor)
        {
            this._factor = factor;
            this._curToken = token;
            // Transfer token and return mocked data
        }

        private void requestCompleted()
        {
            this._factor.Set();
        }


        public void Cancel(INetworkAsyncToken token, EventWaitHandle factor)
        {
            if (_curToken != null && _curToken.Equals(token))
            {
                _curRqeust.Abort();
                factor.Set();
            }
        }
    }
}
