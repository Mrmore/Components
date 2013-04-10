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
    public class FeedLike : BindableBase
    {
        [DataMember]
        private string gid;
        public string Gid
        {
            get
            {
                return gid;
            }
            set
            {
                this.SetProperty(ref gid, value, "Gid");
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
