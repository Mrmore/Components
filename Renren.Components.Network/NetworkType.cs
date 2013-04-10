using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renren.Components.Network
{
    /// <summary>
    /// Define which network protocal are using
    /// </summary>
    public enum NetworkType
    {
        /// <summary>
        /// Specify the http protocal network are using
        /// </summary>
        Http,

        /// <summary>
        /// Specify the low level socket are using
        /// </summary>
        Socket
    }
}
