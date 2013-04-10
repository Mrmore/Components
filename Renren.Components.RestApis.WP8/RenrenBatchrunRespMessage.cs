using Renren.Components.IO.Compression.Gzip;
using Renren.Components.Network;
using Renren.Components.Shared;
using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Renren.Components.RestApis
{
    public class RenrenBatchrunRespMessage<TRemote> : INetworkRespMessage
         where TRemote : IRemoteError
    {
        public TRemote RemoteError
        {
            get;
            private set;
        }

        public IDictionary<BatchRunBinder, object> Results
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

        string _regexPattern =
                @"\{(((""error_code"":\d+,""error_msg"":""(\S|\s)*""))|(""result"":\d+))\}";
        public void Init(byte[] rawData, object resp)
        {
            this.Results = new Dictionary<BatchRunBinder, object>();

            var prepared = prepare(this.RawData, resp);
            string result = Encoding.UTF8.GetString(prepared, 0, prepared.Length);
            this.Prompts = result;
            foreach (var item in Results.Keys)
            {
                string pattern = "\"" + item.Method + "\":" + _regexPattern;
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                var m = regex.Match(result);
                if (m.Success)
                {
                    int nIdx = m.Groups[0].Value.IndexOf(":");
                    string parse = m.Groups[0].Value.Substring(nIdx + 1, 
                        m.Groups[0].Value.Length - nIdx - 1);

                    var error = (TRemote)JsonUtilityExt.DeserializeObj(
                        new MemoryStream(Encoding.UTF8.GetBytes(parse)),
                        typeof(TRemote));

                    if (error != null && error.Hits())
                    {
                        this.Results.Add(item, error);
                    }
                    else
                    {
                        object obj = JsonUtilityExt.DeserializeObj(
                            new MemoryStream(Encoding.UTF8.GetBytes(parse)), item.RespType);
                        this.Results.Add(item, obj);
                    }
                }
            }

            this.Status = RespStatus.Succeed;
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
