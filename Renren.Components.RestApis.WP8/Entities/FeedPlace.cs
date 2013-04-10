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
    public class FeedPlace : BindableBase
    {
        [DataMember]
        private long id;
        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                this.SetProperty(ref id, value, "Id");
            }
        }

        [DataMember]
        private string pid;
        public string Pid
        {
            get
            {
                return pid;
            }
            set
            {
                this.SetProperty(ref pid, value, "Pid");
            }
        }

        [DataMember]
        private string pname;
        public string PlaceName
        {
            get
            {
                return pname;
            }
            set
            {
                this.SetProperty(ref pname, value, "PlaceName");
            }
        }

        [DataMember]
        private string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                this.SetProperty(ref address, value, "Address");
            }
        }

        [DataMember]
        private string comment;
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                this.SetProperty(ref comment, value, "Comment");
            }
        }

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
        private long longitude;
        public long Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                this.SetProperty(ref longitude, value, "longitude");
            }
        }

        [DataMember]
        private long latitude;
        public long Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                this.SetProperty(ref latitude, value, "Latitude");
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
