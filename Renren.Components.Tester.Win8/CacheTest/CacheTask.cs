using Renren.Components.Caching;
using Renren.Components.RestApis.Entities;
using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Renren.Components.IO.Serialization;

namespace Renren.Components.Tester.Win8.CacheTest
{
    public class AddCacheTask : ICacheTask
    {
        public AddCacheTask(ICacheManager manager)
            : base(manager)
        { }

        public override async Task CacheAction()
        {
            CacheDepository<FeedEntity> dep = CacheDepositoryFactory.GetCacheDepository<FeedEntity>();
            KeyValuePair<string, ICollection<FeedEntity>> item = await dep.GetItemFromRawdepository();
            await dep.AddToSynDepository(item.Key,item.Value);
            RecordRuntime record = new RecordRuntime();
            await manager.Add<Collection<FeedEntity>>(item.Key, (Collection<FeedEntity>)item.Value);
            Debug.WriteLine("Add " + item.Key + " time(ms): " + record.ComputeExeTime().ToString());   
        }
    }

    public class RemoveCacheTask : ICacheTask
    {
        public RemoveCacheTask(ICacheManager manager)
            : base(manager)
        { }
        public override async Task CacheAction()
        {
            CacheDepository<FeedEntity> dep = CacheDepositoryFactory.GetCacheDepository<FeedEntity>();
            KeyValuePair<string, ICollection<FeedEntity>> item = await dep.GetItemFromSynDepository();
            if (item.Key != null)
            {
                await dep.RemoveFromSynDepository(item.Key);
                RecordRuntime record = new RecordRuntime();
                await manager.Remove(item.Key);
                Debug.WriteLine("Remove " + item.Key + " time(ms): " + record.ComputeExeTime().ToString());   
            }
        }
    }

    public class GetCacheTask : ICacheTask
    {
        public GetCacheTask(ICacheManager manager)
            : base(manager)
        { }
        public override async Task CacheAction()
        {
            CacheDepository<FeedEntity> dep = CacheDepositoryFactory.GetCacheDepository<FeedEntity>();
            KeyValuePair<string, ICollection<FeedEntity>> item = await dep.GetItemFromSynDepository();
            if (item.Key != null)
            {
                RecordRuntime record = new RecordRuntime();
                Collection<FeedEntity> feeds = await manager.GetData<Collection<FeedEntity>>(item.Key);
                if (feeds == default(Collection<FeedEntity>))
                {
                    Debug.WriteLine("Get " + item.Key + " failed " + "time(ms): " + record.ComputeExeTime().ToString()); 
                    return;
                }
                Debug.WriteLine("Get " + item.Key + " successed " + "time(ms): " + record.ComputeExeTime().ToString()); 
                if (!CacheEquals<Collection<FeedEntity>>.CheckEquals((Collection<FeedEntity>)(item.Value), feeds))
                    throw new Exception("Get Data Error");
                
            }
        }
    }

    public class FlushCacheTask : ICacheTask
    {
        public FlushCacheTask(ICacheManager manager)
            : base(manager)
        { }
        public override async Task CacheAction()
        {
            await CacheDepositoryFactory.GetCacheDepository<FeedEntity>().Flush();
            RecordRuntime record = new RecordRuntime();
            await manager.Flush();
            Debug.WriteLine("Flush" + record.ComputeExeTime().ToString()); 
        }
    }
}
