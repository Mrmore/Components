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
    public class Blog : BindableBase
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
        private string cate;
        public string Category
        {
            get
            {
                return cate;
            }
            set
            {
                this.SetProperty(ref cate, value, "Category");
            }
        }

        [DataMember]
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                this.SetProperty(ref title, value, "Title");
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
        private int view_count;
        public int ViewCount
        {
            get
            {
                return view_count;
            }
            set
            {
                this.SetProperty(ref view_count, value, "ViewCount");
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

        /// <summary>
        /// 权限范围，有4个int值: 99(所有人),4(密码保护) ,1(好友), -1(仅自己可见)。
        /// </summary>
        [DataMember]
        private int visible;
        public int Visible
        {
            get
            {
                return visible;
            }
            set
            {
                this.SetProperty(ref visible, value, "Visible");
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
        private string share_count;
        public string ShareCount
        {
            get
            {
                return share_count;
            }
            set
            {
                this.SetProperty(ref share_count, value, "ShareCount");
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
