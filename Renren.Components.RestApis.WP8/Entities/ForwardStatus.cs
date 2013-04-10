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
    public class ForwardStatus : BindableBase
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
        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                this.SetProperty(ref status, value, "Status");
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
        private string owner_name;
        public string OwnerName
        {
            get
            {
                return owner_name;
            }
            set
            {
                this.SetProperty(ref owner_name, value, "OwnerName");
            }
        }

        [DataMember]
        //用户名 ： 状态内容 StatusEntity  user_name : content
        private string statusContentString;
        public string StatusContentString
        {
            get
            {
                return string.Format("{0} : {1}", owner_name, status);
            }
            set
            {
                this.SetProperty(ref statusContentString, value, "StatusContentString");
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
