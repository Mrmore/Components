using Renren.Components.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Tester.Win8.CacheTest
{
    public abstract class ICacheTask
    {
        private Task task;
        private bool bStop = false;
        protected ICacheManager manager;

        protected ICacheTask(ICacheManager manager)
        {
            this.manager = manager;
        }
        public void Start(int delay)
        {
            if (task == null)
            {
                bStop = false;
                task = Task.Factory.StartNew(async () =>
                    {
                        while (!bStop)
                        {
                            await CacheAction();
                            await Task.Delay(delay);
                        }
                    });
            }
        }

        public void Stop()
        {
            bStop = true;
            task = null;
        }

        public abstract Task CacheAction();
    }
}
