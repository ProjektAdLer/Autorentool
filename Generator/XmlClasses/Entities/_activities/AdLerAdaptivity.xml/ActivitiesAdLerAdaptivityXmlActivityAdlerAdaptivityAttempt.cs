using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;

public class ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityAttempt
{
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityAttempt()
    {
    }

    [XmlAttribute("id")] public int Id { get; set; }

    [XmlAttribute("attempt_id")] public int AttemptId { get; set; }

    [XmlAttribute("user_id")] public int UserId { get; set; }
}