using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;

public class ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestions
{
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestions()
    {
    }

    [XmlElement("question")]
    public List<ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestion> Questions { get; set; } = new();
}