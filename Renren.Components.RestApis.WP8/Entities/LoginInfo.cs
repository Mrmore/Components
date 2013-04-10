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
    public class LoginInfo : BindableBase
    {
        [DataMember]
        private string session_key;
        public string SessionKey
        {
            get
            {
                return session_key;
            }
            set
            {
                this.SetProperty(ref session_key, value, "SessionKey");
            }
        }

        [DataMember]
        private string ticket;
        public string Ticket
        {
            get
            {
                return ticket;
            }
            set
            {
                ticket = value;
                this.SetProperty(ref ticket, value, "Ticket");
            }
        }

        [DataMember]
        private int uid;
        public int Uid
        {
            get
            {
                return uid;
            }
            set
            {
                this.SetProperty(ref uid, value, "Uid");
            }
        }

        [DataMember]
        private string secret_key;
        public string SecretKey
        {
            get
            {
                return secret_key;
            }
            set
            {
                this.SetProperty(ref secret_key, value, "SecretKey");
            }
        }

        [DataMember]
        private string user_name;
        public string UserName
        {
            get
            {
                return user_name;
            }
            set
            {
                this.SetProperty(ref user_name, value, "UserName");
            }
        }

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

        [DataMember]
        private string now;
        public string Now
        {
            get
            {
                return now;
            }
            set
            {
                this.SetProperty(ref now, value, "Now");
            }
        }

        [DataMember]
        private int is_online;
        public int IsOnline
        {
            get
            {
                return is_online;
            }
            set
            {
                this.SetProperty(ref is_online, value, "IsOnline");
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
