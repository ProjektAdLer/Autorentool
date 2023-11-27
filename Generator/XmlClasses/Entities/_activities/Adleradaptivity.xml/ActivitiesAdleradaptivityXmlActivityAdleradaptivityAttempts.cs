using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivityAttempts
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityAttempts()
    {
    }

    [XmlElement("attempt")]
    public List<ActivitiesAdleradaptivityXmlActivityAdleradaptivityAttempt> Attempts { get; set; } = new();
}