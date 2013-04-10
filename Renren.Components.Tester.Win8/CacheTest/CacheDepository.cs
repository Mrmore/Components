using Renren.Components.Async;
using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Tester.Win8.CacheTest
{
    public class CacheDepository<T>
    {
        private ICollection<T> rawDepository;
        private IDictionary<string, ICollection<T>> synDepository = new Dictionary<string, ICollection<T>>();
        private readonly AsyncReaderWriterLock synDepLock = new AsyncReaderWriterLock();
        private Func<ICollection<T>,ICollection<T>, string> keyGenerator;
        private static CacheDepository<T> defaultInstance;
        private static readonly object defaultLock = new object();

        public static CacheDepository<T> Defalut  
        {
            get
            {
                lock (defaultLock)
                {
                    if (defaultInstance == null)
                    {
                        defaultInstance = new CacheDepository<T>(new FeedNewsCreator(),
                            (rc, c) =>
                            {
                                T first = c.First();
                                T last = c.Last();
                                int start = -1;
                                int end = -1;
                                int index = 0;
                                foreach (T item in rc)
                                {
                                    if (first.Equals(item))
                                        start = index++;
                                    if (last.Equals(item))
                                        end = index++;
                                    if (start != -1 && end != -1)
                                        break;
                                }
                                return c.Count.ToString() + start.ToString() + end.ToString();
                            });
                    }
                }
                return defaultInstance;
            }
        }

        public CacheDepository(IDataCreator creator, Func<ICollection<T>,ICollection<T>, string> keyGenerator)
        {
            this.keyGenerator = keyGenerator;
            var col = creator.CreateDate();
            rawDepository = new Collection<T>();
            foreach (var item in col)
            {
                rawDepository.Add((T)item);
            }
        }

        public async Task<KeyValuePair<string,ICollection<T>>> GetItemFromRawdepository()
        {
            int start = RandomKey(0, rawDepository.Count());
            await Task.Delay(5);
            int end = RandomKey(0, rawDepository.Count());
            if (start > end)
            {
                int temp = start;
                start = end;
                end = temp;
            }

            ICollection<T> result = new Collection<T>();
            if (start == end)
            {
                result.Add(rawDepository.ElementAt(start));
            }
            else
            {
                int index = 0;
                foreach (var item in rawDepository)
                {
                    if (index >= start && index < end)
                        result.Add(item);
                    index++;
                }
            }
            return new KeyValuePair<string, ICollection<T>>(result.Count.ToString() + start.ToString() + end.ToString(),
                result);
        }

        public async Task<KeyValuePair<string,ICollection<T>>> GetItemFromSynDepository()
        {
            KeyValuePair<string,ICollection<T>> collection;
            using (var releaser = await synDepLock.ReaderLockAsync())
            {
                if (synDepository.Count() > 0)
                {
                    string key = synDepository.Keys.ElementAt(RandomKey(0, synDepository.Count()));
                    collection = new KeyValuePair<string, ICollection<T>>(key, synDepository[key]);
                }
            }
            return collection;
        }

        public async Task AddToSynDepository(string key,ICollection<T> item)
        {
            using (var releaser = await synDepLock.WriterLockAsync())
            {
                //string key = keyGenerator(rawDepository,item);
                if (!synDepository.ContainsKey(key))
                {
                    synDepository.Add(key, item);
                }
            }
        }

        public async Task RemoveFromSynDepository(string key)
        {
            Guarder.NotNullOrEmpty(key, "collection item");
            using (var releaser = await synDepLock.WriterLockAsync())
            {
                //string key = keyGenerator(rawDepository,item);
                if (synDepository.ContainsKey(key))
                {
                    synDepository.Remove(key);
                }
            }
        }

        public async Task Flush()
        {
            using (var releaser = await synDepLock.WriterLockAsync())
            {
                synDepository.Clear();
            }
        }

        private int RandomKey(int min , int max)
        {
            Random ra = new Random(DateTime.Now.Millisecond);
            return ra.Next(min, max);
        }
    }
}
