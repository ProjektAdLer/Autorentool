using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;


[XmlRoot(ElementName="fileref")]
public class ActivitiesInforefXmlFileref : IActivitiesInforefXmlFileref{

    public ActivitiesInforefXmlFileref()
    {
        File = new List<ActivitiesInforefXmlFile>();
    }

    
    [XmlElement(ElementName="file")]
    public List<ActivitiesInforefXmlFile> File { get; set; }
}