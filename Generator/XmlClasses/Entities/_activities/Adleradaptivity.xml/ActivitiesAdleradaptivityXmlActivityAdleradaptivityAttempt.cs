using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivityAttempt
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityAttempt()
    {
    }

    [XmlAttribute("id")] public int Id { get; set; }

    [XmlAttribute("attempt_id")] public int AttemptId { get; set; }

    [XmlAttribute("user_id")] public int UserId { get; set; }
}