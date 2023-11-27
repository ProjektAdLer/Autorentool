using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestion
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestion()
    {
        Id = "0";
        Difficulty = "0";
    }

    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestion(int id, int difficulty)
    {
        Id = id.ToString();
        Difficulty = difficulty.ToString();
        QuestionReference = new ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestionReference(id);
    }

    [XmlAttribute("id")] public string Id { get; set; }

    [XmlElement("difficulty")] public string Difficulty { get; set; }

    [XmlElement("question_reference")]
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestionReference QuestionReference { get; set; } =
        new();
}