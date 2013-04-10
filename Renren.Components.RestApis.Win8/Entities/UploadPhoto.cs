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
    public class UploadPhoto : BindableBase
    {
        [DataMember]
        private long photo_id;
        public long PhotoId
        {
            get
            {
                return photo_id;
            }
            set
            {
                this.SetProperty(ref photo_id, value, "PhotoId");
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
