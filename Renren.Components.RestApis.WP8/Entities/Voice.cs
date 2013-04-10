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
    public class Voice : BindableBase
    {
        [DataMember]
        private long voice_id;
        public long VoiceId
        {
            get
            {
                return voice_id;
            }
            set
            {
                this.SetProperty(ref voice_id, value, "VoiceId");
            }
        }

        [DataMember]
        private string voice_url;
        public string VoiceUrl
        {
            get
            {
                return voice_url;
            }
            set
            {
                this.SetProperty(ref voice_url, value, "VoiceUrl");
            }
        }

        [DataMember]
        private long voice_length;
        public long VoiceLength
        {
            get
            {
                return voice_length;
            }
            set
            {
                this.SetProperty(ref voice_length, value, "VoiceLength");
            }
        }

        [DataMember]
        private long voice_size;
        public long VoiceSize
        {
            get
            {
                return voice_size;
            }
            set
            {
                this.SetProperty(ref voice_size, value, "VoiceSize");
            }
        }

        [DataMember]
        private long voice_rate;
        public long VoiceRate
        {
            get
            {
                return voice_rate;
            }
            set
            {
                this.SetProperty(ref voice_rate, value, "VoiceRate");
            }
        }

        [DataMember]
        private long voice_count;
        public long VoiceCount
        {
            get
            {
                return voice_count;
            }
            set
            {
                this.SetProperty(ref voice_count, value, "VoiceCount");
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
