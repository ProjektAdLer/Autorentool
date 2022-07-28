using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="file")]
public class ActivitiesInforefXmlFile : IActivitiesInforefXmlFile{

    public ActivitiesInforefXmlFile()
    {
        Id = "";
    }
    
    
    [XmlElement(ElementName="id")]
    public string Id { get; set; }
}
