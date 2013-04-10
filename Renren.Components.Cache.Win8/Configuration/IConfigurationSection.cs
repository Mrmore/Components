using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Renren.Components.Caching.Configuration
{
    public interface IConfigurationSection
    {
        string Section { get; }

        string Name { get; }

        IConfigurationSection ParentSection { get; }

        XElement Element { get; }

        IDictionary<string, string> SectionAttribute { get; }

        ICollection<IConfigurationSection> SubSections { get; }

        void ParseXElement();

        string GetAttributeValue(string attribute);

        IConfigurationSection GetConfigurationSection(string subSection);
    }
}
