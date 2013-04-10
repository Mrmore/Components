using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Renren.Components.Caching.Configuration
{
    public class CacheManagerSection : BaseConfigurationSection
    {
        private readonly string cacheManagerElement = "add";

        public CacheManagerSection(XElement element,IConfigurationSection parentSection)
            : base(element, parentSection)
        {
        }

        public override void ParseXElement()
        {
            var managers = Element.Elements(cacheManagerElement);
            foreach (var item in managers)
            {
                IConfigurationSection section = new CacheManagerItemSection(item, this);
                this.SubSections.Add(section);
            }
        }
    }
}
