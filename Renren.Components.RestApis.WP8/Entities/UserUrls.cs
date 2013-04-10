using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.RestApis.Entities
{
    [DataContract]
    public class UserUrls : BindableBase
    {
        /// <summary>
        /// 50*50
        /// </summary>
        [DataMember]
        private string tiny_url;
        public string TinyUrl
        {
            get
            {
                return tiny_url;
            }
            set
            {
                this.SetProperty(ref tiny_url, value, "TinyUrl");
            }
        }

        /// <summary>
        /// 100
        /// </summary>
        [DataMember]
        private string head_url;
        public string HeadUrl
        {
            get
            {
                return head_url;
            }
            set
            {
                this.SetProperty(ref head_url, value, "HeadUrl");
            }
        }

        /// <summary>
        /// 200
        /// </summary>
        [DataMember]
        private string main_url;
        public string MainUrl
        {
            get
            {
                return main_url;
            }
            set
            {
                this.SetProperty(ref main_url, value, "MainUrl");
            }
        }

        /// <summary>
        /// 400*400
        /// </summary>
        [DataMember]
        private string flex_url;
        public string FlexUrl
        {
            get
            {
                return flex_url;
            }
            set
            {
                this.SetProperty(ref flex_url, value, "FlexUrl");
            }
        }

        /// <summary>
        /// 720
        /// </summary>
        [DataMember]
        private string large_url;
        public string LargeUrl
        {
            get
            {
                return large_url;
            }
            set
            {
                this.SetProperty(ref large_url, value, "LargeUrl");
            }
        }

        [DataMember]
        private object extensions;
        public object Extensions
        {
            get
            {
                return extensions;
            }
            set
            {
                this.SetProperty(ref extensions, value, "Extensions");
            }
        }
    }
}
