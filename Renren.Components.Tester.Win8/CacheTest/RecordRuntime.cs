using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Tester.Win8.CacheTest
{
    public class RecordRuntime
    {
        private long preTicks;
        private Func<int, int> debugAction;

        public RecordRuntime()
        {
            preTicks = DateTime.Now.Ticks;
        }
        public RecordRuntime(Func<int, int> action)
        {
            preTicks = DateTime.Now.Ticks;
            debugAction = action;
        }

        public int ComputeExeTime()
        {
            long now = DateTime.Now.Ticks;
            int ExeTime = TimeSpan.FromTicks(now - preTicks).Milliseconds;
            preTicks = now;
            if (debugAction != null)
                debugAction(ExeTime);
            return ExeTime;
        }
    }
}
