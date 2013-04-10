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
    public class Photo : BindableBase
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
        private long album_id;
        public long AlbumId
        {
            get
            {
                return album_id;
            }
            set
            {
                this.SetProperty(ref album_id, value, "AlbumId");
            }
        }

        [DataMember]
        private string album_name = string.Empty;
        public string AlbumName
        {
            get
            {
                return album_name;
            }
            set
            {
                this.SetProperty(ref album_name, value, "AlbumName");
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
        private string img_head;
        public string ImgHead
        {
            get
            {
                return img_head;
            }
            set
            {
                this.SetProperty(ref img_head, value, "ImgHead");
            }
        }

        [DataMember]
        private string img_large;
        public string ImgLarge
        {
            get
            {
                return img_large;
            }
            set
            {
                this.SetProperty(ref img_large, value, "ImgLarge");
            }
        }

        [DataMember]
        private string img_main;
        public string ImgMain
        {
            get
            {
                return img_main;
            }
            set
            {
                this.SetProperty(ref img_main, value, "ImgMain");
            }
        }

        [DataMember]
        private string caption;
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                this.SetProperty(ref caption, value, "Caption");
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
        private int img_large_width;
        public int ImgLargeWidth
        {
            get
            {
                return img_large_width;
            }
            set
            {
                this.SetProperty(ref img_large_width, value, "ImgLargeWidth");
            }
        }

        [DataMember]
        private int img_large_height;
        public int ImgLargeHeight
        {
            get
            {
                return img_large_height;
            }
            set
            {
                this.SetProperty(ref img_large_height, value, "ImgLargeHeight");
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

        private string user_head = String.Empty;
        public string UserHead
        {
            get
            {
                return user_head;
            }
            set
            {
                this.SetProperty(ref user_head, value, "UserHead");
            }
        }

        [DataMember]
        private FeedPlace lbs_data = new FeedPlace();
        public FeedPlace LBSData
        {
            get
            {
                return lbs_data;
            }
            set
            {
                this.SetProperty(ref lbs_data, value, "LBSData");
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
