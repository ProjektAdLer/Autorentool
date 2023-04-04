using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._sections.Section.xml;

public class SectionsSectionXmlPluginLocalAdlerSection : ISectionsSectionXmlPluginLocalAdlerSection
{
    public SectionsSectionXmlPluginLocalAdlerSection()
    {
        AdlerSection = new SectionsSectionXmlAdlerSection();
    }

    [XmlElement(ElementName = "adler_section")]
    public SectionsSectionXmlAdlerSection? AdlerSection { get; set; }
}