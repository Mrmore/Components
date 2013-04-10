using Renren.Components.RestApis.Entities;
using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Tester.Win8.CacheTest
{
    public interface IDataCreator
    {
        ICollection<object> CreateDate();
    }

    public class FeedNewsCreator : IDataCreator
    {
        private ICollection<object> result;
        public ICollection<object> CreateDate()
        {
            SemaphoreSlim sema = new SemaphoreSlim(0);

            var token = RestApis.Feed.Get<FeedList>((t) =>
                {
                    if (t.Status == Network.NetworkStatus.Completed)
                    {
                        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(t.Response.Prompts));
                        FeedList feeds = (FeedList)stream.DeserializeJsonStreamAs(typeof(FeedList));
                        result = new Collection<object>();
                        foreach (var item in feeds.Feeds)   
                        {
                            result.Add(item);    
                        }                        
                    }
                    else
                        result = null;
                    sema.Release();
                }, this);
            
            sema.Wait();
            return result;
        }
    }
}
