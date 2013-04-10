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
    public class Comment : BindableBase
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
        private int user_id;
        public int UserId
        {
            get
            {
                return user_id;
            }
            set
            {
                this.SetProperty(ref user_id, value, "UserId");
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
        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                this.SetProperty(ref content, value, "Content");
            }
        }

        [DataMember]
        private long time;
        public long Time
        {
            get
            {
                return time;
            }
            set
            {
                this.SetProperty(ref time, value, "Time");
            }
        }

        [DataMember]
        private string whisper;
        public string Whisper
        {
            get
            {
                return whisper;
            }
            set
            {
                this.SetProperty(ref whisper, value, "Whisper");
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

    [DataContract]
    public class CommentList : BindableBase
    {
        [DataMember]
        private ObservableCollection<Comment> comment_list = new ObservableCollection<Comment>();
        public ObservableCollection<Comment> Comments
        {
            get
            {
                return comment_list;
            }
            set
            {
                this.SetProperty(ref comment_list, value, "Comments");
            }
        }

        [DataMember]
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                this.SetProperty(ref count, value, "Count");
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
