using Renren.Components.RestApis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.RestApis
{
    public static class SocialSDKContext
    {
        static public LoginInfo LoginContext = null;
        public static string ScreenResolution = null;
        public static string UDID = "dummy device id";

        public const string ApiKey = "";
        public const string SecretKey = "";
        public const string ApiVersion = "1.0";

        public const string RestApiBaseAddress = "http://api.m.renren.com/api/";
        //public static string RestApiBaseAddress = "http://mc2.test.renren.com/api/";

        public const string PublishDate = "20121123";
        public const string OS = "Windows 8";
        public const string AppID = "208652";
        public const string AppName = "Renren Windows 8";

    }
}
