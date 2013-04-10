using Renren.Components.IO.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Renren.Components.IO
{
    public static class UriExtensions
    {
        public static async Task<byte[]> DownloadAsBytes(this Uri uri)
        {
            return await UriDownloader.DownloadAsBytesFromUri(uri);
        }

        public static async Task<Stream> DownloadAsStream(this Uri uri)
        {
            return await UriDownloader.DownloadAsStreamFromUri(uri);
        }

        public static async Task<Stream> AsStream(this Uri uri)
        {
            var streamRef = RandomAccessStreamReference.CreateFromUri(uri);

            var content = await streamRef.OpenReadAsync();
            return content.AsStream();
        }

#if NETFX_CORE
        public static async Task<IRandomAccessStream> DownloadAsRandomStream(this Uri uri)
        {
            return await UriDownloader.DownloadAsRandomStreamFromUri(uri);
        }
#else
        public static async Task<Stream> DownloadAsIsoStream(this Uri uri)
        {
            return await UriDownloader.DownloadAsIsoStreamFromUri(uri);
        }
#endif
    }
}
