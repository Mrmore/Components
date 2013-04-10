using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// The formdata parameter definiton
    /// </summary>
    public class FormdataParam
    {
        public ICollection<FormdataItem> Itmes { get; set; }
        public string Boundary { get; set; }
    }

    /// <summary>
    /// Define one item of formdata parameter
    /// </summary>
    public class FormdataItem
    {
        /// <summary>
        /// The name of one item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The file name of upload file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// It dose not hand over the stream's lifecycle
        /// so you should dispose it outside
        /// </summary>
        public Stream Data { get; set; }

        /// <summary>
        /// The content type of upload file
        /// </summary>
        public string ContentType { get; set; }
    }

    /// <summary>
    /// The definition of http request message
    /// </summary>
    public class HttpRequestMessage
    {
        /// <summary>
        /// This default post request message provided to create a new 
        /// default post request message
        /// </summary>
        public static HttpRequestMessage DefaultPost
        {
            get
            {
                var message = new HttpRequestMessage();
                message.Method = HttpMethod.POST;
                message.ContentType = "text/plain;charset=utf-8";
                message.QueryPairs = new Dictionary<string, string>();
                message.Headers = new Dictionary<string, string>();
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
        }

        public static HttpRequestMessage DefaultGet
        {
            get
            {
                var message = new HttpRequestMessage();
                message.Method = HttpMethod.GET;

                return message;
            }
        }

        /// <summary>
        /// This default multi-part request message provided to create a new 
        /// default multi-part request message
        /// </summary>
        public static HttpRequestMessage DefaultMultiPart
        {
            get
            {
                var message = new HttpRequestMessage();
                message.Method = HttpMethod.POST_FORMDATA;
                message.QueryPairs = new Dictionary<string, string>();
                message.Headers = new Dictionary<string, string>();
                message.Formdata = new FormdataParam() 
                { 
                    Itmes = new List<FormdataItem>(), 
                    Boundary = DateTime.Now.Ticks.ToString("X")
                };
                message.ContentType = "multipart/form-data;charset=utf-8;boundary=" + message.Formdata.Boundary;
                message.PostZipFunc = (dict) => dict.ToString();

                return message;
            }
        }

        /// <summary>
        /// The internal http request message constructor
        /// </summary>
        internal HttpRequestMessage() { }

        /// <summary>
        /// The target url
        /// </summary>
        public Uri Target { get; set; }

        /// <summary>
        /// The query pairs using to contain the post data
        /// </summary>
        public IDictionary<string, string> QueryPairs { get; set; }

        /// <summary>
        /// The post data zip function used to zip the post data
        /// Note: the input dict is query pairs
        /// </summary>
        public Func<IDictionary<string, string>, string> PostZipFunc { get; set; }

        /// <summary>
        /// Getter and setter of formdata prameter
        /// </summary>
        public FormdataParam Formdata { get; set; }

        /// <summary>
        /// Getter and setter of http request header
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Getter and setter of the request content type of upload file
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Getter and setter of http request method
        /// </summary>
        public HttpMethod Method { get; set; }
    }
}
