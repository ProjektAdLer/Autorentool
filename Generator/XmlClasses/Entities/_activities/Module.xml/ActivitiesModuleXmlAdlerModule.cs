using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Module.xml;

public class ActivitiesModuleXmlAdlerModule : IActivitiesModuleXmlAdlerModule
{
    public ActivitiesModuleXmlAdlerModule()
    {
        ScoreMax = "0";
        Uuid = "";
        TimeCreated = "0";
        TimeModified = "0";
    }
    
    [XmlElement(ElementName = "score_max")]
    public string ScoreMax { get; set; }
    
    [XmlElement(ElementName = "uuid")]
    public string Uuid { get; set; }
    
    [XmlElement(ElementName = "timecreated")]
    public string TimeCreated { get; set; }
    
    [XmlElement(ElementName = "timemodified")]
    public string TimeModified { get; set; }
}