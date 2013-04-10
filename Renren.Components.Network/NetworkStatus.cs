using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renren.Components.Network
{
    /// <summary>
    /// The token's network operation status definition
    /// It's charge of indicating operation status to a specified token
    /// </summary>
    public enum NetworkStatus
    {
        /// <summary>
        /// Indicate the specified token is waitting to schedule
        /// </summary>
        Waitting,

        /// <summary>
        /// Indicate the specified token has been send to network channel
        /// </summary>
        Pendding,

        /// <summary>
        /// Indicate the specified token has completed the network operation
        /// </summary>
        Completed,

        /// <summary>
        /// Indicate the specified token network operation failed
        /// </summary>
        Failed,

        /// <summary>
        /// Indicate the specified token has been cancelled
        /// </summary>
        Canceled,

        /// <summary>
        /// Indicate the specified token operation time out
        /// </summary>
        Timeout,

        /// <summary>
        /// Should never meet this status
        /// </summary>
        Unkown
    }
}
