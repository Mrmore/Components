using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
    [DataContract]
    public class CacheConfigSetting : BindableBase
    {
        [DataMember]
        private string key;
        public string Key
        {
            get { return key; }
            set { this.SetProperty(ref key, value, "Key"); }
        }

        [DataMember]
        private int timerTicks;
        public int TimerTicks
        {
            get { return timerTicks; }
            set { this.SetProperty(ref timerTicks, value, "TimerTicks"); }
        }

        [DataMember]
        private int maxItemNumber;
        public int MaxItemNumber
        {
            get { return maxItemNumber; }
            set { this.SetProperty(ref maxItemNumber, value, "MaxItemNumber"); }
        }

        [DataMember]
        private int removeNumber;
        public int RemoveNumber
        {
            get { return removeNumber; }
            set { this.SetProperty(ref removeNumber, value, "RemoveNumber"); }
        }

        [DataMember]
        private StoreType cacheStoreType;
        public StoreType CacheStoreType
        {
            get { return cacheStoreType; }
            set { this.SetProperty(ref cacheStoreType, value, "CacheStoreType"); }
        }

        public static CacheConfigSetting CreatSetting(string key = null)
        {
            key = key ?? "DefaultCacheManageKey";
            CacheConfigSetting setting = new CacheConfigSetting(key,200,200,100,StoreType.File);
            return setting;
            
        }

        private CacheConfigSetting(string key, int timerTicks, int maxItemNumber, int removeNumber, StoreType cacheStoreType)
        {
            this.key = key;
            this.timerTicks = timerTicks;
            this.maxItemNumber = maxItemNumber;
            this.removeNumber = removeNumber;
            this.cacheStoreType = cacheStoreType;
        }
    }

    public enum StoreType
    {
        UnKnown = 0,
        File,
        DataBase     
    }
}
