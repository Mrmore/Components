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
    public class FeedList : BindableBase
    {
        [DataMember]
        private ObservableCollection<FeedEntity> feed_list = new ObservableCollection<FeedEntity>();
        public ObservableCollection<FeedEntity> Feeds
        {
            get
            {
                return feed_list;
            }
            set
            {
                this.SetProperty(ref feed_list, value, "Feeds");
            }
        }

        [DataMember]
        private int has_more;
        public int HasMore
        {
            get
            {
                return has_more;
            }
            set
            {
                this.SetProperty(ref has_more, value, "HasMore");
            }
        }

        [DataMember]
        private int is_mini_feed;
        public int IsMiniFeed
        {
            get
            {
                return is_mini_feed;
            }
            set
            {
                this.SetProperty(ref is_mini_feed, value, "IsMiniFeed");
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
