using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Renren.Components.Tools;
using System.Diagnostics;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// One default implementation of http network transition
    /// It's in charge of sending and reponsing of overall http requst
    /// </summary>
    public class HttpTransition : INetworkTransition<INetworkAsyncToken, AutoResetEvent>
    {
        private IHttpAsyncToken _curToken = null;
        private HttpWebRequest _curRqeust = null;
        private AutoResetEvent _curFactor = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpTransition() { }

        /// <summary>
        /// Send specified token to network channel and wait for completing
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="factor">The wait factor</param>
        /// <remarks>Note: you should always pass a wait factor for sync up the operation</remarks>
        public void Send(INetworkAsyncToken token, AutoResetEvent factor)
        {
            try
            {
                this._curToken = (IHttpAsyncToken)token;
                this._curFactor = factor;
                this._curToken.SetStatus(NetworkStatus.Pendding);
                this._curProgress = 10;

                if (_curToken.Method == HttpMethod.GET)
                    startGet();
                else if (_curToken.Method == HttpMethod.POST)
                    startPost();
                else if (_curToken.Method == HttpMethod.POST_FORMDATA)
                    startMultiPart();
                else
                {
                    throw new NotImplementedException();
                }

                if (token.ExpTimeout != null)
                {
                    try
                    {
                        BackgroundTimer timer = new BackgroundTimer();
                        timer.Interval = (TimeSpan)token.ExpTimeout;
                        timer.Tick += (sender, e) =>
                        {
                            timer.Stop();
                            lock (this)
                            {
                                if (token.Status == NetworkStatus.Pendding)
                                {
                                    feedbackResult(new TimeoutException(), NetworkStatus.Timeout);
                                }
                            }
                        };
                    }
                    catch (Exception)
                    { }
                }
            }
            catch (Exception ex)
            {
                feedbackResult(ex, NetworkStatus.Failed);
            }

        }

        /// <summary>
        /// Start handle the muli-part post operation
        /// </summary>
        private void startMultiPart()
        {
            try
            {
                _curRqeust = WebRequest.CreateHttp(_curToken.Request.Target);
                _curRqeust.Method = "POST";
                _curRqeust.ContentType = _curToken.Request.ContentType;
                var boundary = _curToken.Request.Formdata.Boundary;

                _curRqeust.BeginGetRequestStream((IAsyncResult result) =>
                {
                    byte[] beginBoundary = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                    byte[] endBoundary = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

                    string paraTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    string fileTemplate = "Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    string fileTemplateWithoutContentType = "Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n\r\n";

                    using (Stream stream = _curRqeust.EndGetRequestStream(result))
                    {
                        foreach (var param in _curToken.Request.QueryPairs)
                        {
                            string value = param.Value ?? string.Empty;
                            byte[] bpara = Encoding.UTF8.GetBytes(String.Format(paraTemplate, param.Key, value));
                            stream.Write(beginBoundary, 0, beginBoundary.Length);
                            stream.Write(bpara, 0, bpara.Length);
                        }

                        stream.Write(beginBoundary, 0, beginBoundary.Length);

                        foreach (var item in _curToken.Request.Formdata.Itmes)
                        {
                            byte[] bfile = null;
                            var targetFormat = (item.ContentType == null) ? fileTemplateWithoutContentType : fileTemplate;
                            if (item.ContentType == null)
                            {
                                bfile = Encoding.UTF8.GetBytes(String.Format(targetFormat, item.Name, item.FileName));
                            }
                            else
                            {
                                bfile = Encoding.UTF8.GetBytes(String.Format(targetFormat, item.Name, item.FileName, item.ContentType));
                            }
                            stream.Write(bfile, 0, bfile.Length);

                            item.Data.CopyTo(stream);
                            stream.Flush();
                            //byte[] content = await ApiHelper.GetFileContent(item.File);
                            //stream.Write(content, 0, content.Length);

                            _curProgress = (_curProgress + 10) < 90 ? (_curProgress + 10) : 90;
                            reportProgress(_curProgress, "One file has been uploaded");
                        }
                        stream.Write(endBoundary, 0, endBoundary.Length);

                        stream.Flush();
                    }

                    _curRqeust.BeginGetResponse(new AsyncCallback(doResponse), _curRqeust);
                }, _curRqeust);
            }
            catch (Exception ex)
            {
                feedbackResult(ex, NetworkStatus.Failed);
            }

        }

        /// <summary>
        /// Start generic post http request
        /// </summary>
        private void startPost()
        {
            try
            {
                _curRqeust = WebRequest.CreateHttp(_curToken.Request.Target);
                _curRqeust.Method = "POST";
                _curRqeust.ContentType = _curToken.Request.ContentType;

                foreach (var header in _curToken.Request.Headers)
                {
                    _curRqeust.Headers[header.Key] = header.Value;
                }

                _curRqeust.BeginGetRequestStream(new AsyncCallback(doRequest), _curRqeust);

                _curProgress = (_curProgress + 10) < 90 ? (_curProgress + 10) : 90;
                reportProgress(_curProgress, "The request has been sent");
            }
            catch (Exception ex)
            {
                feedbackResult(ex, NetworkStatus.Failed);
            }

        }

        /// <summary>
        /// Start the generic get http request
        /// </summary>
        private void startGet()
        {
            try
            {
                _curRqeust = WebRequest.CreateHttp(_curToken.Request.Target);
                _curRqeust.Method = "GET";
                _curRqeust.BeginGetResponse(new AsyncCallback(doResponse), _curRqeust);

                _curProgress = (_curProgress + 10) < 90 ? (_curProgress + 10) : 90;
                reportProgress(_curProgress, "The request has been sent");
            }
            catch (Exception ex)
            {
                feedbackResult(ex, NetworkStatus.Failed);
            }
        }

        /// <summary>
        /// Handle overall http web request of post functionality
        /// </summary>
        /// <param name="asyncResult">The http request object</param>
        /// <remarks>It assumes 10% work load</remarks>
        private void doRequest(IAsyncResult asyncResult)
        {
            try
            {
                HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
                using (Stream stream = request.EndGetRequestStream(asyncResult))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(_curToken.Request.PostZipFunc(_curToken.Request.QueryPairs));
                        writer.Flush();
                    }
                }

                request.BeginGetResponse(new AsyncCallback(doResponse), request);

                _curProgress = (_curProgress + 10) < 90 ? (_curProgress + 10) : 90;
                reportProgress(_curProgress, "The http request complete and wait for response");
            }
            catch (Exception ex)
            {
                feedbackResult(ex, NetworkStatus.Failed);
            }

        }

        private int _curProgress = 0;
        /// <summary>
        /// Handle overall http web response of post functionality
        /// </summary>
        /// <param name="asyncResult">The http request object</param>
        /// <remarks>It assumes 80% work load</remarks>
        private void doResponse(IAsyncResult asyncResult)
        {
            try
            {
                HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
                using (HttpWebResponse response = request.EndGetResponse(asyncResult) as HttpWebResponse)
                {
                    if (response != null && (int)response.StatusCode < 300 && (int)response.StatusCode >= 200)
                    {
                        byte[] rawData = null;
                        const int chunk = 1024;

                        using (Stream input = response.GetResponseStream())
                        {
                            using (MemoryStream output = new MemoryStream())
                            {
                                byte[] buffer = new byte[chunk];
                                int bytesRead = 0;
                                long totalBytes = 0;
                                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                                {

                                    totalBytes += bytesRead;
                                    output.Write(buffer, 0, bytesRead);

                                    _curProgress = (_curProgress + 10) < 90 ? (_curProgress + 10) : 90;
                                    reportProgress(_curProgress, "The http request is reading response");
                                }

                                output.Flush();
                                rawData = output.ToArray();
                            }
                        }

                        feedbackResult(rawData, NetworkStatus.Completed, response);
                    }
                    else
                    {
                        feedbackResult(new InvalidOperationException(response.StatusDescription),
                            NetworkStatus.Failed);
                    }
                }
            }
            catch (Exception ex)
            {
                feedbackResult(ex, NetworkStatus.Failed);
            }
        }

        /// <summary>
        /// Report current progress status
        /// </summary>
        /// <param name="progress">The progress value</param>
        /// <param name="description">The progress description</param>
        private void reportProgress(int progress, string description)
        {
            if (_curToken.Progress != null)
            {
                _curToken.Progress.Report(_curToken, progress, description);
            }
        }

        /// <summary>
        /// Feed back the request result,
        /// which would be generic result or some generic netork error
        /// </summary>
        /// <param name="result">The result object</param>
        /// <param name="status">The network status should be</param>
        /// <param name="resp">The reponse message from network platform</param>
        private void feedbackResult(object result, NetworkStatus status, HttpWebResponse resp = null)
        {
            try
            {
                if (_curRqeust != null && status != NetworkStatus.Completed)
                {
                    _curRqeust.Abort();
                }

                if (result is Exception)
                {
                    _curToken.SetException(result as Exception);
                }
                else if (result is byte[])
                {
                    _curToken.SetRawData(result as byte[], resp);
                }

                _curToken.SetStatus(status);


                reportProgress(100, "The overall request and response has been completed");
                _curToken.Handler.Execute(_curToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("#Transition Error =>" + ex.ToString());
            }
            finally
            {
                _curFactor.Set();
            }
        }

        /// <summary>
        /// Cancel current operation of request
        /// </summary>
        /// <param name="token">The token need to canceled</param>
        /// <param name="factor">The sync up factor</param>
        public void Cancel(INetworkAsyncToken token, AutoResetEvent factor)
        {
            lock (this)
            {
                if (_curToken != null && 
                    _curToken.Equals(token) &&
                    _curToken.Status == NetworkStatus.Pendding &&
                    object.ReferenceEquals(factor, _curFactor))
                {
                    feedbackResult(new OperationCanceledException(), NetworkStatus.Canceled);
                }
            }
        }
    }
}
