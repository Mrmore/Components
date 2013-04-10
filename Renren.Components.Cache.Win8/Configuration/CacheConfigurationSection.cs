using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Renren.Components.Caching.Configuration
{
    public class CacheConfigurationSection : BaseConfigurationSection
    {
        private readonly string cacheManagersAttribute = "cacheManagers";
        private readonly string cacheManagerElement = "cacheManagers";
        private readonly string backingStoreElement = "backingStores";
        public CacheConfigurationSection(XElement element)
            :base(element,null)
        {
        }

        public override void ParseXElement()
        {
            var cacheManagers = Element.Attribute(cacheManagersAttribute);
            if (cacheManagers == null)
                throw new ArgumentNullException("configuration " + cacheManagersAttribute);
            this.SectionAttribute.Add(cacheManagersAttribute, Element.Attribute(cacheManagersAttribute).Value);
            IConfigurationSection cacheManagerSection = new CacheManagerSection(Element.Element(cacheManagerElement), this);
            this.SubSections.Add(cacheManagerSection);
            IConfigurationSection backingStoreSection = new BackingStoreSection(Element.Element(backingStoreElement), this);
            this.SubSections.Add(backingStoreSection);
        }
    }
}
