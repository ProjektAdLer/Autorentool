using System;
using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.sections;

[XmlRoot(ElementName="section")]
public partial class SectionsSectionXmlSection : ISectionsSectionXmlSection{


    public void SetParameters(string id, string number)
    {
        var currTime = DateTimeOffset.Now.ToUnixTimeSeconds(); 
        Id = id;
        Number = number;
        Timemodified = currTime.ToString();
    }
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "sections/section_160/section.xml");

    }
        
    [XmlElement(ElementName="number")]
    public string Number = "";
        
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
    public string Timemodified = "";
        
    [XmlAttribute(AttributeName="id")]
    public string Id = "";
}