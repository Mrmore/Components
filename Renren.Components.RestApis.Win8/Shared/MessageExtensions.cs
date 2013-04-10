using Renren.Components.Network.Http;
using Renren.Components.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.RestApis.Shared
{
    public static class MessageExtensions
    {
        public static HttpRequestMessage ToRenrenStyle(this HttpRequestMessage message)
        {
            message.ContentType = "application/x-www-form-urlencoded";
            message.PostZipFunc = (dict) =>
            {
                StringBuilder parameters = new StringBuilder();
                foreach (var param in dict)
                {
                    parameters.Append(String.Format("{0}={1}&", param.Key, param.Value));
                }

                string result = parameters.ToString();
                return result.Substring(0, result.Length - 1);
            };

            return message;
        }

        public static string ToRenrenSig(this HttpRequestMessage request, string key)
        {
            StringBuilder sb = new StringBuilder();

            var queries = request.QueryPairs;
            var sorted = queries.OrderBy((t) => t.Key);

            foreach (var query in sorted)
            {
                sb.Append(string.Format("{0}={1}",
                    query.Key, query.Value.Length < 50 ?
                    query.Value : query.Value.Substring(0, 50)));
            }

            sb.Append(key);

            return sb.ToString().ToMD5String();
        }
    }
}
