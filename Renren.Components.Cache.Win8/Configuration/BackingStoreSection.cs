using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Renren.Components.Caching.Configuration
{
    public class BackingStoreSection : BaseConfigurationSection
    {
        private readonly string cacheManagerElement = "add";

        public BackingStoreSection(XElement element, IConfigurationSection parentSection)
            : base(element, parentSection)
        {
        }

        public override void ParseXElement()
        {
            var managers = Element.Elements(cacheManagerElement);
            foreach (var item in managers)
            {
                IConfigurationSection section = new BackingStoreItemSection(item, this);
                this.SubSections.Add(section);
            }
        }
    }
}
