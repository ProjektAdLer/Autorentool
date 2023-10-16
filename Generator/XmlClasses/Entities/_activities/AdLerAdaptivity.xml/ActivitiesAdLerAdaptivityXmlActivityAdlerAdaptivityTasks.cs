using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;

public class ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTasks
{
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTasks()
    {
    }

    [XmlElement("task")]
    public List<ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask> Tasks { get; set; } = new();
}