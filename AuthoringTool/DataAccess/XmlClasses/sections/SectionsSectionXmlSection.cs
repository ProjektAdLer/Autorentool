using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.sections;

[XmlRoot(ElementName="section")]
public partial class SectionsSectionXmlSection {
        
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