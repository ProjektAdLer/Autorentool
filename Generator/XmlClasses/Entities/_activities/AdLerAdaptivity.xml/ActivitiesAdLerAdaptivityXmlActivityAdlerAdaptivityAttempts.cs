using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;

public class ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityAttempts
{
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityAttempts()
    {
    }

    [XmlElement("attempt")]
    public List<ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityAttempt> Attempts { get; set; } = new();
}