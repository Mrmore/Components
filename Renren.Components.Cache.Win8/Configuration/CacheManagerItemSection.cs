using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Renren.Components.Caching.Configuration
{
    public class CacheManagerItemSection : BaseConfigurationSection
    {
        private readonly string nameAttribute = "name";
        private readonly string typeAttribute = "type";
        private readonly string expirationPollFrequencyInSecondsAttribute = "expirationPollFrequencyInSeconds";
        private readonly string maximumElementsInCacheBeforeScavengingAttribute = "maximumElementsInCacheBeforeScavenging";
        private readonly string numberToRemoveWhenScavengingAttribute = "numberToRemoveWhenScavenging";
        private readonly string backingStoreFolderNameAttribute = "backingStoreFolderName";
        private readonly string backingStoresAttribute = "backingStores";

        public CacheManagerItemSection(XElement element, IConfigurationSection parentSection)
            : base(element, parentSection)
        {
        }

        public override void ParseXElement()
        {
            Guarder.NotNull(Element.Attribute(typeAttribute), typeAttribute);
            Guarder.NotNull(Element.Attribute(expirationPollFrequencyInSecondsAttribute), expirationPollFrequencyInSecondsAttribute);
            Guarder.NotNull(Element.Attribute(maximumElementsInCacheBeforeScavengingAttribute), maximumElementsInCacheBeforeScavengingAttribute);
            Guarder.NotNull(Element.Attribute(numberToRemoveWhenScavengingAttribute), numberToRemoveWhenScavengingAttribute);
            Guarder.NotNull(Element.Attribute(backingStoreFolderNameAttribute), backingStoreFolderNameAttribute);
            Guarder.NotNull(Element.Attribute(backingStoresAttribute), backingStoresAttribute);

            this.SectionAttribute.Add(typeAttribute, Element.Attribute(typeAttribute).Value);
            this.SectionAttribute.Add(expirationPollFrequencyInSecondsAttribute, Element.Attribute(expirationPollFrequencyInSecondsAttribute).Value);
            this.SectionAttribute.Add(maximumElementsInCacheBeforeScavengingAttribute, Element.Attribute(maximumElementsInCacheBeforeScavengingAttribute).Value);
            this.SectionAttribute.Add(numberToRemoveWhenScavengingAttribute, Element.Attribute(numberToRemoveWhenScavengingAttribute).Value);
            this.SectionAttribute.Add(backingStoreFolderNameAttribute, Element.Attribute(backingStoreFolderNameAttribute).Value);
            this.SectionAttribute.Add(backingStoresAttribute, Element.Attribute(backingStoresAttribute).Value);
            
            if (Element.Attribute(nameAttribute) != null)
                this.name = Element.Attribute(nameAttribute).Value;
        }
    }
}
