using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.RestApis.Entities
{
    [DataContract]
    public class Attachment : BindableBase
    {
        [DataMember]
        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                this.SetProperty(ref url, value, "Url");
            }
        }

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
        private string type;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                this.SetProperty(ref type, value, "Type");
            }
        }

        [DataMember]
        private string src;
        public string Src
        {
            get
            {
                return src;
            }
            set
            {
                this.SetProperty(ref src, value, "Src");
            }
        }

        [DataMember]
        private long media_id;
        public long MediaId
        {
            get
            {
                return media_id;
            }
            set
            {
                this.SetProperty(ref media_id, value, "MediaId");
            }
        }

        [DataMember]
        private int owner_id;
        public int OwnerId
        {
            get
            {
                return owner_id;
            }
            set
            {
                this.SetProperty(ref owner_id, value, "OwnerId");
            }
        }

        [DataMember]
        private string digest;
        public string Digest
        {
            get
            {
                return digest;
            }
            set
            {
                this.SetProperty(ref digest, value, "Digest");
            }
        }

        [DataMember]
        private int comment_count;
        public int CommentCount
        {
            get
            {
                return comment_count;
            }
            set
            {
                this.SetProperty(ref comment_count, value, "CommentCount");
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
