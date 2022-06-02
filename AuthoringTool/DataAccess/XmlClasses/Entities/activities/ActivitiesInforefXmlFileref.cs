using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="fileref")]
public class ActivitiesInforefXmlFileref : IActivitiesInforefXmlFileref{
    
    public void SetParameters(List<ActivitiesInforefXmlFile>? file)
    {
        File = file;
    }
    
    [XmlElement(ElementName="file")]
    public List<ActivitiesInforefXmlFile>? File { get; set; }
}