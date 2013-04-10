using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renren.Components.Tools;

namespace Renren.Components.Tester.Win8.CacheTest
{
    public static class CacheEquals<T>
    {
        public static bool CheckEquals(T obj1, T obj2)
        {
            if (obj1.GetType() != typeof(T) || obj2.GetType() != typeof(T))
                return false;

            string str1 = obj1.SerializeAsJsonString(typeof(T));
            string str2 = obj2.SerializeAsJsonString(typeof(T));
            if (!str1.Equals(str2))
                return false;
            else
                return true;
        }
    }
}
