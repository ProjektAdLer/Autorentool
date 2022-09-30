using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Url.xml;

[XmlRoot(ElementName = "url")]
public class ActivitiesUrlXmlUrl : IActivitiesUrlXmlUrl
{
    public ActivitiesUrlXmlUrl()
    {
        Name = "";
        Intro = "";
        Introformat = "1";
        Externalurl = "";
        Display = "1";
        Displayoptions = "a:1:{s:10:\"printintro\";i:1;}";
        Parameters = "a:0:{}";
        Timemodified = "";
        Id = "";
    }

    [XmlElement(ElementName="name")]
    public string Name { get; set; }
    [XmlElement(ElementName="intro")]
    public string Intro { get; set; }
    [XmlElement(ElementName="introformat")]
    public string Introformat { get; set; }
    [XmlElement(ElementName="externalurl")]
    public string Externalurl { get; set; }
    [XmlElement(ElementName="display")]
    public string Display { get; set; }
    [XmlElement(ElementName="displayoptions")]
    public string Displayoptions { get; set; }
    [XmlElement(ElementName="parameters")]
    public string Parameters { get; set; }
    [XmlElement(ElementName="timemodified")]
    public string Timemodified { get; set; }
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
}