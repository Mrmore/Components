using System;
using System.Net;
using Windows.ApplicationModel;

namespace Renren.Components.Shared
{
    public partial class Constants
    {
        public static string ApiKey = "3ee4de2e054e4767a846b7a8c248da6b";
        public static string SecretKey = "ed3b38a8eb7445b9866a489d82e6b250";
        public static string ApiVersion = "1.0";

        public static string RestApiBaseAddress = "http://api.m.renren.com/api/";
        //public static string RestApiBaseAddress = "http://mc2.test.renren.com/api/";

        public static string UDID = "dummy device id";
        public static Uri ShareSongRequestUri = new Uri("http://api.m.renren.com/api/radio/shareSong", UriKind.Absolute);
        public static string PublishDate = "20121123";
        public static string OS = "Windows 8";
        public static string AppID = "208652";
        public static string AppName = "Renren Windows 8 HD";
        public static string ScreenResolution = null;
    }
}
