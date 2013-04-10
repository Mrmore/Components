
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Renren.Components.Network
{
    /// <summary>
    /// It's a default implementation of network response message
    /// </summary>
    public class NetworkRespMessage : INetworkRespMessage
	{
        /// <summary>
        /// The reponse status
        /// </summary>
        public RespStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Local exception during request sending
        /// </summary>
        public Exception LocalError
        {
            get;
            private set;
        }

        /// <summary>
        /// The status informaton used to prompt the end-user
        /// </summary>
        public string Prompts
        {
            get;
            private set;
        }

        /// <summary>
        /// The raw byte array data
        /// </summary>
        public byte[] RawData
        {
            get;
            private set;
        }

        /// <summary>
        /// Initialize the response message with raw data
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="additonal"></param>
        public void Init(byte[] raw, object additional)
        {
            this.RawData = raw;
            this.Prompts = Encoding.UTF8.GetString(raw, 0, raw.Length);
            this.Status = RespStatus.Succeed;
        }

        /// <summary>
        /// Initialize the reponse message with local exception
        /// </summary>
        /// <param name="ex"></param>
        public void Init(Exception ex)
        {
            this.LocalError = ex;
            this.Prompts = ex.Message;
            this.Status = RespStatus.LocalFailed;
        }
    }
}
