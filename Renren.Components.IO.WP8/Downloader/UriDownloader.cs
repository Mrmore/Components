using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Renren.Components.IO.Compression;

#if NETFX_CORE
#else
using System.IO.IsolatedStorage;
#endif

namespace Renren.Components.IO.Downloader
{
    public static class UriDownloader
    {
#if NETFX_CORE
        public static async Task<byte[]> DownloadAsBytesFromUri(Uri uri)
        {
            return await Task.Run<byte[]>(async () =>
                {
                    var streamRef = RandomAccessStreamReference.CreateFromUri(uri);

                    byte[] bytes = new byte[1];
                    using (var stream = await streamRef.OpenReadAsync())
                    {
                        bytes = stream.AsStreamForRead().ToArray();
                    }

                    return bytes;
                });
        }
#else
        public static async Task<byte[]> DownloadAsBytesFromUri(Uri uri)
        {
            return await Task.Run<byte[]>(() =>
                {
                    AutoResetEvent syncRoot = new AutoResetEvent(false);
                    byte[] bytes = new byte[1];

                    var request = WebRequest.CreateHttp(uri);
                    request.Method = "GET";
                    request.BeginGetResponse(new AsyncCallback(
                        async (r) =>
                        {
                            var req = r.AsyncState as HttpWebRequest;
                            try
                            {
                                using (HttpWebResponse response = req.EndGetResponse(r) as HttpWebResponse)
                                {
                                    if (response != null && (int)response.StatusCode < 300 && (int)response.StatusCode >= 200)
                                    {
                                        using (Stream input = response.GetResponseStream())
                                        {
                                            using (MemoryStream output = new MemoryStream())
                                            {
                                                await input.CopyToAsync(output);
                                                output.Flush();
                                                bytes = output.ToArray();
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Uri download failed: " + ex.ToString());
                            }
                            finally
                            {
                                syncRoot.Set();
                            }
                        }
                        ), request);

                    syncRoot.WaitOne();
                    return bytes;
                });
        }
#endif

        public static async Task<Stream> DownloadAsStreamFromUri(Uri uri)
        {
            // Try to downlaod the total bytes from network
            var bytes = await DownloadAsBytesFromUri(uri);
            return new MemoryStream(bytes);
        }

#if NETFX_CORE
        public static async Task<IRandomAccessStream> DownloadAsRandomStreamFromUri(Uri uri)
        {
            return await Task.Run<IRandomAccessStream>(async () =>
            {
                // Try to downlaod the total bytes from network
                var bytes = await DownloadAsBytesFromUri(uri);

                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0));
                writer.WriteBytes(bytes);

                await writer.StoreAsync();
                await randomAccessStream.FlushAsync();
                return randomAccessStream;
            });
        }
#else
        public static async Task<Stream> DownloadAsIsoStreamFromUri(Uri uri)
        {
            return await Task.Run<Stream>(async () =>
            {
                // Try to downlaod the total bytes from network
                var bytes = await DownloadAsBytesFromUri(uri);

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // Create a directory at the root of the store.
                    if (!store.DirectoryExists("UriDownloadCacheTempFolder"))
                    {
                        store.CreateDirectory("UriDownloadCacheTempFolder");
                    }

                    var name = uri.AbsoluteUri.ToMD5String();
                    var extension = Path.GetExtension(uri.AbsoluteUri);
                    var path = @"UriDownloadCacheTempFolder\" + name + extension;

                    IsolatedStorageFileStream isoStream = store.OpenFile(path, FileMode.OpenOrCreate);
                    isoStream.Write(bytes, 0, (int)bytes.Length);
                    isoStream.Flush();

                    return isoStream;
                }
            });

        }
#endif

    }
}
