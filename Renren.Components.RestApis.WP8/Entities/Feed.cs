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
    public class FeedEntity : BindableBase
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
        private long source_id;
        public long SourceId
        {
            get
            {
                return source_id;
            }
            set
            {
                this.SetProperty(ref source_id, value, "SourceId");
            }
        }

        [DataMember]
        private int type;
        public int Type
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
        private string str_time;
        public string StrTime
        {
            get
            {
                return str_time;
            }
            set
            {
                this.SetProperty(ref str_time, value, "StrTime");
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
        private string prefix;
        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                this.SetProperty(ref prefix, value, "Prefix");
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
        private int origin_type;
        public int OriginType
        {
            get
            {
                return origin_type;
            }
            set
            {
                this.SetProperty(ref origin_type, value, "OriginType");
            }
        }

        [DataMember]
        private string origin_title;
        public string OriginTitle
        {
            get
            {
                return origin_title;
            }
            set
            {
                this.SetProperty(ref origin_title, value, "OriginTitle");
            }
        }

        [DataMember]
        private string origin_img;
        public string OriginImg
        {
            get
            {
                return origin_img;
            }
            set
            {
                this.SetProperty(ref origin_img, value, "OriginImg");
            }
        }

        [DataMember]
        private int origin_page_id;
        public int OriginPageId
        {
            get
            {
                return origin_page_id;
            }
            set
            {
                this.SetProperty(ref origin_page_id, value, "OriginPageId");
            }
        }

        [DataMember]
        private string origin_url;
        public string OriginUrl
        {
            get
            {
                return origin_url;
            }
            set
            {
                this.SetProperty(ref origin_url, value, "OriginUrl");
            }
        }

        [DataMember]
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                this.SetProperty(ref description, value, "Description");
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
        private ObservableCollection<Comment> comment_list = new ObservableCollection<Comment>();
        public ObservableCollection<Comment> CommentList
        {
            get
            {
                return comment_list;
            }
            set
            {
                this.SetProperty(ref comment_list, value, "CommentList");
            }
        }

        [DataMember]
        private ObservableCollection<Attachment> attachement_list = new ObservableCollection<Attachment>();
        public ObservableCollection<Attachment> AttachementList
        {
            get
            {
                return attachement_list;
            }
            set
            {
                this.SetProperty(ref attachement_list, value, "AttachementList");
            }
        }

        [DataMember]
        private ForwardStatus status_forward = new ForwardStatus();
        public ForwardStatus StatusForward
        {
            get
            {
                return status_forward;
            }
            set
            {
                this.SetProperty(ref status_forward, value, "StatusForward");
            }
        }

        [DataMember]
        private Share share = new Share();
        public Share Share
        {
            get
            {
                return share;
            }
            set
            {
                this.SetProperty(ref share, value, "Share");
            }
        }

        [DataMember]
        private FeedPlace place = new FeedPlace();
        public FeedPlace Place
        {
            get
            {
                return place;
            }
            set
            {
                this.SetProperty(ref place, value, "Place");
            }
        }

        [DataMember]
        private FeedLike like = new FeedLike();
        public FeedLike Like
        {
            get
            {
                return like;
            }
            set
            {
                this.SetProperty(ref like, value, "Like");
            }
        }

        [DataMember]
        private UserUrls user_urls;
        public UserUrls UserUrls
        {
            get
            {
                return user_urls;
            }
            set
            {
                this.SetProperty(ref user_urls, value, "UserUrls");
            }
        }

        [DataMember]
        private Voice voice;
        public Voice Voice
        {
            get
            {
                return voice;
            }
            set
            {
                this.SetProperty(ref voice, value, "Voice");
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
