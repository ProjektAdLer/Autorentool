using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;

public class ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestion
{
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestion()
    {
        Id = "0";
        Difficulty = "0";
    }

    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestion(int id, int difficulty)
    {
        Id = id.ToString();
        Difficulty = difficulty.ToString();
        QuestionReference = new ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestionReference(id);
    }

    [XmlAttribute("id")] public string Id { get; set; }

    [XmlElement("difficulty")] public string Difficulty { get; set; }

    [XmlElement("question_reference")]
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestionReference QuestionReference { get; set; } =
        new();
}