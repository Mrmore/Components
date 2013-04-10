using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.Caching.Configuration
{
    public class CacheConfigurationSetting : IConfigurationSetting
    {
        private string configName;

        public string ConfigName
        {
            get { return configName; }
        }

        public IDictionary<Type, object> InitArgs
        {
            get;
            set;
        }

        private string instanceName;

        public string InstanceName
        {
            get { return instanceName; }
        }

        public void AddArg(Type classType, object arg)
        {
            if (InitArgs == null)
                InitArgs = new Dictionary<Type, object>();
            InitArgs.Add(classType, arg);
        }

        public CacheConfigurationSetting(string configName, string instanceName = null, IDictionary<Type, object> arg = null)
        {
            Guarder.NotNullOrEmpty(configName, "configuration Name");
            this.configName = configName;
            this.instanceName = instanceName;
            this.InitArgs = arg;
        }
    }
}
