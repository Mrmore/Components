using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Network
{
    /// <summary>
    /// Define the network transition scheduler profile interfaces
    /// It's in charge of scheduling overall token pendding, sending, and cancelling operation
    /// </summary>
    public interface INetworkTransitionScheduler
    {
        /// <summary>
        /// Try to start the scheduler to work with specified configuration 
        /// </summary>
        /// <param name="config">The specified configuration</param>
        /// <remarks>You can stop with Stop method after start it</remarks>
        void Start(TransitionSchedulerConfig config);

        /// <summary>
        /// Try to stop overall operation of this scheduler
        /// </summary>
        /// <remarks>You can restart it with Start method after stop it</remarks>
        void Stop();

        /// <summary>
        /// Add specified new token to scheduler and wait to handle
        /// </summary>
        /// <param name="token"></param>
        void AddToken(INetworkAsyncToken token);

        /// <summary>
        /// Update the specified token's priority to a newer one
        /// </summary>
        /// <param name="older">The specified token</param>
        /// <param name="newPriority">A newer priority</param>
        void Update(INetworkAsyncToken older, TransitionPriority newPriority);

        /// <summary>
        /// Try to cancel the specified token asynchronously
        /// </summary>
        /// <param name="token">The specified token</param>
        /// <returns>The value indicate whether token has been cancelled</returns>
        Task<bool> TryCancelAsync(INetworkAsyncToken token);

        /// <summary>
        /// Try to cancel the specified token
        /// </summary>
        /// <param name="token">The specified token</param>
        void Cancel(INetworkAsyncToken token);

        /// <summary>
        /// Return a value to indicate whether the secheduler is running
        /// </summary>
        bool IsRunning { get; }
    }
}
