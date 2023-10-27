using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivityTasks
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTasks()
    {
    }

    [XmlElement("task")]
    public List<ActivitiesAdleradaptivityXmlActivityAdleradaptivityTask> Tasks { get; set; } = new();
}