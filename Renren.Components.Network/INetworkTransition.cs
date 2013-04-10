using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Network
{
    /// <summary>
    /// Specify the network transition's intefaces
    /// It's not the final profile for network transition, since
    /// it's only used by <see cref="INetworkTransitionScheduler"/>
    /// It's not generic network transition profile
    /// </summary>
    /// <typeparam name="T">The token's type</typeparam>
    /// <typeparam name="R">The sync up factor</typeparam>
    /// <remarks>The generic types T and R, stand for token type and sync up root type
    /// and the token type should always be derived from <see cref="INetworkAsyncToken"/></remarks>
    public interface INetworkTransition<T, R> where T : INetworkAsyncToken
    {
        /// <summary>
        /// Send specified token to network channel and do some sync up issue
        /// </summary>
        /// <param name="token"></param>
        /// <param name="factor"></param>
        void Send(T token, R factor);

        /// <summary>
        /// Canel the specified token and do some sync up issue
        /// </summary>
        /// <param name="token"></param>
        /// <param name="factor"></param>
        void Cancel(T token, R factor);
    }
}
