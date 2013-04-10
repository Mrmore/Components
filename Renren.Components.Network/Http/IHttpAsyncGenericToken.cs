using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// Specify a http aysnc token's properties
    /// </summary>
    public interface IHttpAsyncToken : INetworkAsyncToken
    {
        /// <summary>
        /// The http request method
        /// </summary>
        HttpMethod Method { get; set; }

        /// <summary>
        /// The http request message
        /// </summary>
        HttpRequestMessage Request { get; }
    }
}
