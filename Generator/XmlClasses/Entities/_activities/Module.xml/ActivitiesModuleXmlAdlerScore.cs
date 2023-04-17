using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Module.xml;

public class ActivitiesModuleXmlAdlerScore : IActivitiesModuleXmlAdlerScore
{
    public ActivitiesModuleXmlAdlerScore()
    {
        ScoreMax = "0";
        Timecreated = "0";
        TimeModified = "0";
    }
    
    [XmlElement(ElementName = "score_max")]
    public string ScoreMax { get; set; }
    
    [XmlElement(ElementName = "timecreated")]
    public string Timecreated { get; set; }
    
    [XmlElement(ElementName = "timemodified")]
    public string TimeModified { get; set; }
}