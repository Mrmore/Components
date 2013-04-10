using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Network
{
    /// <summary>
    /// Specify the network async token's generic interfaces and properties
    /// It's an major facade for network client
    /// </summary>
    public interface INetworkAsyncToken
    {
        /// <summary>
        /// The action used to handler the token completed
        /// </summary>
        WeakAction<INetworkAsyncToken> Handler { get; }

        /// <summary>
        /// Getter and Setter the progress indicator
        /// </summary>
        IProgressIndicator<int> Progress { get; set; }

        /// <summary>
        /// Specify the time out used to request of this token
        /// </summary>
        TimeSpan? ExpTimeout { get; set; }

        /// <summary>
        ///  Getter and setter of an arbitrary object value that can be used to store
        ///  custom information about this object.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Getter and setter of this token's priority
        /// </summary>
        TransitionPriority Priority { get; set; }

        /// <summary>
        /// Getter of this token's status
        /// </summary>
        NetworkStatus Status { get; }

        /// <summary>
        /// Getter of the netwrok type used to transfer
        /// </summary>
        NetworkType NetType { get; }

        /// <summary>
        /// Getter of the scheduler reference
        /// </summary>
        INetworkTransitionScheduler Scheduler { get; }

        /// <summary>
        /// Getter of http request message
        /// </summary>
        INetworkRespMessage Response { get; }

        /// <summary>
        /// Getter of the token's unique id
        /// It will generate during construct process
        /// </summary>
        string Id { get; }

        /// <summary>
        /// This method is responsible for cancelling current token's request
        /// </summary>
        void Cancel();

        /// <summary>
        /// Set the byte array of raw data
        /// </summary>
        /// <param name="rawData">The byte array raw data</param>
        /// <param name="resp">The reponse taking the raw data</param>
        void SetRawData(byte[] rawData, object additional);

        /// <summary>
        /// Set the Exception error met in request
        /// process
        /// </summary>
        /// <param name="ex">The exception error</param>
        void SetException(Exception ex);

        /// <summary>
        /// Set token's status
        /// </summary>
        /// <param name="status">The status</param>
        void SetStatus(NetworkStatus status);
    }
}
