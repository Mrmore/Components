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
    public class PhotoList : BindableBase
    {
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
        private ObservableCollection<Photo> photo_list = new ObservableCollection<Photo>();
        public ObservableCollection<Photo> Photos
        {
            get
            {
                return photo_list;
            }
            set
            {
                this.SetProperty(ref photo_list, value, "Photos");
            }
        }

        [DataMember]
        private string album_name;
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
        private int album_id;
        public int AlbumId
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
