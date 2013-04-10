using Renren.Components.IO.Compression.Gzip;
using Renren.Components.Network;
using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Renren.Components.RestApis
{
    public class RenrenRespMessage<TEntity, TRemote> : INetworkRespMessage
        where TRemote : IRemoteError
        where TEntity : class
    {
        public TRemote RemoteError
        {
            get;
            private set;
        }

        public TEntity Result
        {
            get;
            private set;
        }

        public RespStatus Status
        {
            get;
            private set;
        }

        public Exception LocalError
        {
            get;
            private set;
        }

        public string Prompts
        {
            get;
            private set;
        }

        public byte[] RawData
        {
            get;
            private set;
        }

        public void Init(byte[] rawData, object resp)
        {
            this.RawData = rawData;

            var prepared = prepare(this.RawData, resp);
            var error = (TRemote)JsonUtilityExt.DeserializeObj(
                new MemoryStream(prepared), typeof(TRemote));

            if (error != null && error.Hits())
            {
                this.RemoteError = error;
                this.Prompts = error.ToHumanString();
                this.Status = RespStatus.RemoteFailed;
            }
            else
            {
                TEntity obj = (TEntity)JsonUtilityExt.DeserializeObj(
                    new MemoryStream(prepared), typeof(TEntity));
                this.Result = obj;
                this.Prompts = Encoding.UTF8.GetString(prepared, 0, prepared.Length);
                Status = RespStatus.Succeed;
            }

            Debug.WriteLine(Prompts);
        }

        public void Init(Exception ex)
        {
            this.LocalError = ex;
            this.Prompts = ex.Message;
            this.Status = RespStatus.LocalFailed;
        }

        private byte[] prepare(byte[] original, object resp)
        {
            if (resp != null &&
                resp is HttpWebResponse &&
                Regex.IsMatch(((resp as HttpWebResponse).Headers["Content-Type"] ?? "")
                                     .ToLower(), "(application/json-gz)"))
            {
                {
                    return GZipHelper.Decompress(original);
                }
            }
            else
            {
                return original;
            }
        }
    }
}
