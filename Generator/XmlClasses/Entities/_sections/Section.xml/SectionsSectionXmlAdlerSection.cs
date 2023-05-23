using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._sections.Section.xml;

public class SectionsSectionXmlAdlerSection : ISectionsSectionXmlAdlerSection
{
    public SectionsSectionXmlAdlerSection()
    {
        RequiredPointsToComplete = "0";
        Uuid = "";
    }
    
    [XmlElement(ElementName = "required_points_to_complete")]
    public string RequiredPointsToComplete { get; set; }
    
    [XmlElement(ElementName = "uuid")]
    public string Uuid { get; set; }
}