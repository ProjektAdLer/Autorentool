using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Inforef.xml;


[XmlRoot(ElementName="file")]
public class ActivitiesInforefXmlFile : IActivitiesInforefXmlFile{

    public ActivitiesInforefXmlFile()
    {
        Id = "";
    }
    
    
    [XmlElement(ElementName="id")]
    public string Id { get; set; }
}
