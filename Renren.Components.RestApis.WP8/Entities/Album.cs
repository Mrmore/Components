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
    public class Album : BindableBase
    {
        [DataMember]
        private int id;
        public int Id
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
        private string img;
        public string Imgage
        {
            get
            {
                return img;
            }
            set
            {
                this.SetProperty(ref img, value, "Imgage");
            }
        }

        [DataMember]
        private string large_img;
        public string LargeImage
        {
            get
            {
                return large_img;
            }
            set
            {
                this.SetProperty(ref large_img, value, "LargeImage");
            }
        }

        [DataMember]
        private string main_img;
        public string MainImage
        {
            get
            {
                return main_img;
            }
            set
            {
                this.SetProperty(ref main_img, value, "MainImage");
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
        private long create_time;
        public long CreateTime
        {
            get
            {
                return create_time;
            }
            set
            {
                this.SetProperty(ref create_time, value, "CreateTime");
            }
        }

        [DataMember]
        private long upload_time;
        public long UploadTime
        {
            get
            {
                return upload_time;
            }
            set
            {
                this.SetProperty(ref upload_time, value, "UploadTime");
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
        private string location;
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                this.SetProperty(ref location, value, "Location");
            }
        }

        [DataMember]
        private int size;
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                this.SetProperty(ref size, value, "Size");
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
        private int has_password;
        public int HasPassword
        {
            get
            {
                return has_password;
            }
            set
            {
                this.SetProperty(ref has_password, value, "HasPassword");
            }
        }

        [DataMember]
        private int album_type;
        public int AlbumType
        {
            get
            {
                return album_type;
            }
            set
            {
                this.SetProperty(ref album_type, value, "AlbumType");
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
