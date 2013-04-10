

using System;

namespace Renren.Components.Network
{
    /// <summary>
    /// Define the reponse message status
    /// </summary>
    public enum RespStatus
    {
        /// <summary>
        /// Indicate this is a succeed reponse
        /// </summary>
        Succeed,

        /// <summary>
        /// Indicate this is a remote error from server
        /// </summary>
        RemoteFailed,

        /// <summary>
        /// Indicate this is a local error during process
        /// </summary>
        LocalFailed,

        /// <summary>
        /// Should never meet this status
        /// </summary>
        Unkown
    }

    /// <summary>
    /// Specify the network message profile
    /// </summary>
    public interface INetworkRespMessage
    {
        /// <summary>
        /// Get the reponse's status
        /// </summary>
        RespStatus Status { get; }

        /// <summary>
        /// Get the local exception during request
        /// </summary>
        Exception LocalError { get; }

        /// <summary>
        /// The information string used to prompt the end-user
        /// </summary>
        string Prompts { get; }

        /// <summary>
        /// Get the raw byte array data
        /// </summary>
        byte[] RawData { get; }

        /// <summary>
        /// Initialize the response message with raw data
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="additonal"></param>
        void Init(byte[] raw, object additional);

        /// <summary>
        /// Initialize the reponse message with local exception
        /// </summary>
        /// <param name="ex"></param>
        void Init(Exception ex);
    }
}
