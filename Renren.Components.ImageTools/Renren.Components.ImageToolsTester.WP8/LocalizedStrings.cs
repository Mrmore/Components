﻿using Renren.Components.ImageToolsTester.WP8.Resources;

namespace Renren.Components.ImageToolsTester.WP8
{
    /// <summary>
    /// 提供对字符串资源的访问权。
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}