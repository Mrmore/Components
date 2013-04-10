using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Caching.Configuration
{
    public interface IConfigurationSetting
    {
        string ConfigName { get; }

        IDictionary<Type, object> InitArgs { get; set; }

        string InstanceName { get; }

        void AddArg(Type classType, object arg);
    }
}
