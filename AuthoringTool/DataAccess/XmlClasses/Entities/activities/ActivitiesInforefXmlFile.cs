using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="file")]
public class ActivitiesInforefXmlFile : IActivitiesInforefXmlFile{
    
    public void SetParameters(string? id)
    {
        Id = id;
    }
    
    [XmlElement(ElementName="id")]
    public string? Id { get; set; }
}
