using Renren.Components.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Tester.Win8.CacheTest
{
    public class CacheDepositoryFactory
    {
        private static readonly AsyncLock depLock = new AsyncLock();

        public static CacheDepository<T> GetCacheDepository<T>()
        {
            return CacheDepository<T>.Defalut;
        }
    }
}
