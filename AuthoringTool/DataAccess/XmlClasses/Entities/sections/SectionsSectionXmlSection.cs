using System;
using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.sections;

[XmlRoot(ElementName="section")]
public partial class SectionsSectionXmlSection : ISectionsSectionXmlSection{
    
    public void SetParameters(string? number, string name, string summary, string summaryformat, string sequence, 
        string visible, string availabilityjson, string? timemodified, string? id)
    {
        Number = number;
        Name = name;
        Summary = summary;
        Summaryformat = summaryformat;
        Sequence = sequence;
        Visible = visible;
        Availabilityjson = availabilityjson;
        Timemodified = timemodified;
        Id = id;
    }
    
    
    public void Serialize(string? sectionId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("sections", "section_"+sectionId, "section.xml"));

    }
        
    [XmlElement(ElementName="number")]
    public string? Number = "";
        
    [XmlElement(ElementName="name")]
    public string Name = "$@NULL@$";

    [XmlElement(ElementName = "summary")] public string Summary = "";

    [XmlElement(ElementName = "summaryformat")]
    public string Summaryformat = "1";
        
    [XmlElement(ElementName="sequence")]
    public string Sequence = "";
        
    [XmlElement(ElementName="visible")]
    public string Visible = "1";

    [XmlElement(ElementName = "availabilityjson")]
    public string Availabilityjson = "$@NULL@$";

    [XmlElement(ElementName = "timemodified")]
    public string? Timemodified = "";
        
    [XmlAttribute(AttributeName="id")]
    public string? Id = "";
}