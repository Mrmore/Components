using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Network
{
    /// <summary>
    /// Transition secheduler configuration definition
    /// </summary>
    [DataContract]
    public class TransitionSchedulerConfig
    {
        /// <summary>
        /// Specify the channel count
        /// </summary>
        [DataMember]
        public int ChannelCount { get; set; }

        /// <summary>
        /// Specify the default time-out one token allow to wait in network operation
        /// Note: it will be effective till the time-out was not specified to a specified token
        /// </summary>
        [DataMember]
        public int DefaultTimeout { get; set; }
    }
}
