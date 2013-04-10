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
    public class RenrenRemoteError : BindableBase, IRemoteError
    {
        [DataMember]
        private int? error_code = 0;
        public int? ErrorCode
        {
            get
            {
                return error_code;
            }
            set
            {
                this.SetProperty(ref error_code, value, "ErrorCode");
            }
        }

        [DataMember]
        private string error_msg = string.Empty;
        public string ErrorMessage
        {
            get
            {
                return error_msg;
            }
            set
            {
                this.SetProperty(ref error_msg, value, "ErrorMessage");
            }
        }

        public bool Hits()
        {
            return (!string.IsNullOrEmpty(error_msg) && error_code != null);
        }

        public string ToHumanString()
        {
            return error_msg ?? "";
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
