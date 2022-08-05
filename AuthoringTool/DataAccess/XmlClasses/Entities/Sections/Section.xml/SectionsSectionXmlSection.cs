using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Section.xml;

[XmlRoot(ElementName="section")]
public class SectionsSectionXmlSection : ISectionsSectionXmlSection{

    public SectionsSectionXmlSection()
    {
        Number = "";
        Name = "";
        Summary = "";
        SummaryFormat = "0";
        Sequence = "$@NULL@$";
        Visible = "1";
        AvailabilityJson = "$@NULL@$";
        Timemodified = "";
        Id = "";
    }
    
    
    public void Serialize(string? name, string? sectionId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("sections", "section_"+sectionId, "section.xml"));

    }
        
    [XmlElement(ElementName="number")]
    public string Number { get; set; }
        
    [XmlElement(ElementName="name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "summary")] 
    public string Summary { get; set; }

    [XmlElement(ElementName = "summaryformat")]
    public string SummaryFormat { get; set; }
        
    [XmlElement(ElementName="sequence")]
    public string Sequence { get; set; }
        
    [XmlElement(ElementName="visible")]
    public string Visible { get; set; }

    [XmlElement(ElementName = "availabilityjson")]
    public string AvailabilityJson { get; set; }

    [XmlElement(ElementName = "timemodified")]
    public string Timemodified { get; set; }
        
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
}