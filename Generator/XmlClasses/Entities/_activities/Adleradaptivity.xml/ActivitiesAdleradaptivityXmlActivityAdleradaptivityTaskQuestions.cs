using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestions
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestions()
    {
    }

    [XmlElement("question")]
    public List<ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestion> Questions { get; set; } = new();
}