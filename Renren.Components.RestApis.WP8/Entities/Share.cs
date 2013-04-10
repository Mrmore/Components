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
    public class Share : BindableBase
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
        private long share_id;
        public long ShareId
        {
            get
            {
                return share_id;
            }
            set
            {
                this.SetProperty(ref share_id, value, "ShareId");
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
        private int source_owner_id;
        public int SourceOwnerId
        {
            get
            {
                return source_owner_id;
            }
            set
            {
                this.SetProperty(ref source_owner_id, value, "SourceOwnerId");
            }
        }

        [DataMember]
        private string source_owner_name;
        public string SourceOwnerName
        {
            get
            {
                return source_owner_name;
            }
            set
            {
                this.SetProperty(ref source_owner_name, value, "SourceOwnerName");
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
        private string photo;
        public string Photo
        {
            get
            {
                return photo;
            }
            set
            {
                this.SetProperty(ref photo, value, "Photo");
            }
        }

        [DataMember]
        private string photo_main;
        public string PhotoMain
        {
            get
            {
                return photo_main;
            }
            set
            {
                this.SetProperty(ref photo_main, value, "PhotoMain");
            }
        }

        [DataMember]
        private string photo_large;
        public string PhotoLarge
        {
            get
            {
                return photo_large;
            }
            set
            {
                this.SetProperty(ref photo_large, value, "PhotoLarge");
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
        private string video_url;
        public string VideoUrl
        {
            get
            {
                return video_url;
            }
            set
            {
                this.SetProperty(ref video_url, value, "VideoUrl");
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
        private int share_count;
        public int ShareCount
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
        private string forward_comment;
        public string ForwardComment
        {
            get
            {
                return forward_comment;
            }
            set
            {
                this.SetProperty(ref forward_comment, value, "ForwardComment");
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
        private int video_support;
        public int VideoSupport
        {
            get
            {
                return video_support;
            }
            set
            {
                this.SetProperty(ref video_support, value, "VideoSupport");
            }
        }

        [DataMember]
        private Blog blog_info;
        public Blog BlogInfo
        {
            get
            {
                return blog_info;
            }
            set
            {
                this.SetProperty(ref blog_info, value, "BlogInfo");
            }
        }

        [DataMember]
        private PhotoList photo_info;
        public PhotoList PhotoInfo
        {
            get
            {
                return photo_info;
            }
            set
            {
                this.SetProperty(ref photo_info, value, "PhotoInfo");
            }
        }

        [DataMember]
        private AlbumList album_info;
        public AlbumList Album_Info
        {
            get
            {
                return album_info;
            }
            set
            {
                this.SetProperty(ref album_info, value, "Album_Info");
            }
        }
        /// <summary>
        /// 分享转发链meta {"fpath":[{"uid":237371900,"uname":"康明明"},{"uid":352596716,"uname":"张凯彦"}]} 
        /// </summary>
        [DataMember]
        private ObservableCollection<FPath> forward_meta = new ObservableCollection<FPath>();
        public ObservableCollection<FPath> ForwardMeta
        {
            get
            {
                return forward_meta;
            }
            set
            {
                this.SetProperty(ref forward_meta, value, "FPath");
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
