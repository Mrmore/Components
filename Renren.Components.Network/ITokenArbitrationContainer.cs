using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renren.Components.Network
{
    /// <summary>
    /// Define the profile interfaces of arbitration container of tokens
    /// Note: the token type should always be derived from INetworkAsyncToken
    /// </summary>
    /// <typeparam name="T">The specified token type</typeparam>
    /// <remarks>Note: it's in charge of deciding which token should be taken next</remarks>
    public interface ITokenArbitrationContainer<T>
        where T : INetworkAsyncToken
    {
        /// <summary>
        /// Add a specified token to container
        /// </summary>
        /// <param name="token"></param>
        void AddToken(T token);

        /// <summary>
        /// Get the token should be taken next
        /// This method would be customized with different strategy
        /// </summary>
        /// <returns></returns>
        T Next();

        /// <summary>
        /// Clear overall tokens of container
        /// </summary>
        void Clear();

        /// <summary>
        /// Update the priority of specified token to a newer priority
        /// </summary>
        /// <param name="token">The specified token</param>
        /// <param name="newPriority">The newer priority</param>
        void Update(T token, TransitionPriority newPriority);

        /// <summary>
        /// Remove the specified token from container
        /// </summary>
        /// <param name="token">The specified token</param>
        void Remove(T token);

        /// <summary>
        /// Retrun a value to indicate whether the specified token exists 
        /// in this container
        /// </summary>
        /// <param name="token">The specified token</param>
        /// <returns>Ture: exsits, False: not exsits</returns>
        bool Contains(T token);
    }
}
