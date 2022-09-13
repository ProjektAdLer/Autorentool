using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Label.xml;

[XmlRoot(ElementName="label")]
public class ActivitiesLabelXmlLabel : IActivitiesLabelXmlLabel
{
    public ActivitiesLabelXmlLabel()
    {
        Name = "";
        Intro = "";
        Introformat = "1";
        Timemodified = "";
        Id = "";
    }

    [XmlElement(ElementName = "name")] 
    public string Name { get; set; }
    
    [XmlElement(ElementName = "intro")] 
    public string Intro { get; set; }

    [XmlElement(ElementName = "introformat")]
    public string Introformat { get; set; }

    [XmlElement(ElementName = "timemodified")]
    public string Timemodified { get; set; }

    [XmlAttribute(AttributeName = "id")] 
    public string Id { get; set; }
}