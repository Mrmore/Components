using Renren.Components.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Renren.Components.Caching.Configuration
{
    public abstract class BaseConfigurationSection : IConfigurationSection
    {
        private string section;

        public string Section { get { return section; } }

        protected string name;

        public string Name { get { return name; } }

        public IConfigurationSection parentSection;

        public IConfigurationSection ParentSection { get { return parentSection; } }

        private XElement element;

        public XElement Element { get { return element; } }

        private IDictionary<string, string> sectionAttribute = new Dictionary<string,string>();

        public IDictionary<string, string> SectionAttribute { get { return sectionAttribute; } }

        private ICollection<IConfigurationSection> subSections = new Collection<IConfigurationSection>();

        public ICollection<IConfigurationSection> SubSections { get { return subSections; } }

        protected BaseConfigurationSection(XElement element, IConfigurationSection parentSection)
        {
            Guarder.NotNull(element,"XElement");
            this.element = element;
            this.parentSection = parentSection;
            ParseXElement();
        }

        public string GetAttributeValue(string attribute)
        {
            Guarder.NotNullOrEmpty(attribute, "attribute");
            if(sectionAttribute != null && sectionAttribute.ContainsKey(attribute))
                return sectionAttribute[attribute];
            return string.Empty;
        }

        public IConfigurationSection GetConfigurationSection(string subSection)
        {
            Guarder.NotNullOrEmpty(subSection, "subSection");
            if (!string.IsNullOrEmpty(Name) && Name.Equals(subSection))
                return this;
            if (SubSections.Count == 0)
                return null;
            else
            {
                IConfigurationSection section = null;
                foreach (var item in SubSections)
                {
                    section =  item.GetConfigurationSection(subSection);
                    if (section != null)
                        break;
                }
                return section;
            }
        }

        public abstract void ParseXElement();
    }
}
