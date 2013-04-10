using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Renren.Components.Caching.Configuration
{
    public class BackingStoreItemSection : BaseConfigurationSection
    {
        private readonly string nameAttribute = "name";
        private readonly string typeAttribute = "type";
        public BackingStoreItemSection(XElement element, IConfigurationSection parentSection)
            : base(element, parentSection)
        {
        }

        public override void ParseXElement()
        {
            Guarder.NotNull(Element.Attribute(typeAttribute), typeAttribute);

            this.SectionAttribute.Add(typeAttribute, Element.Attribute(typeAttribute).Value);
            if (Element.Attribute(nameAttribute) != null)
                this.name = Element.Attribute(nameAttribute).Value;
        }
    }
}
