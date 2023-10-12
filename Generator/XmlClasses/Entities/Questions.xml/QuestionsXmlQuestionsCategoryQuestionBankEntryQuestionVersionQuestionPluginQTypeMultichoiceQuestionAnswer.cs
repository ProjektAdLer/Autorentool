using System.Globalization;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswer
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswer()
    {
        Id = "0";
        Answertext = "";
        Fraction = "0";
    }

    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswer(
        int id, string answertext, double fraction)
    {
        Id = id.ToString();
        Answertext = answertext;
        Fraction = fraction.ToString(CultureInfo.InvariantCulture);
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "answertext")]
    public string Answertext { get; set; }

    [XmlElement(ElementName = "answerformat")]
    public string Answerformat { get; set; } = "1";

    [XmlElement(ElementName = "fraction")] public string Fraction { get; set; }

    [XmlElement(ElementName = "feedback")] public string Feedback { get; set; } = "";

    [XmlElement(ElementName = "feedbackformat")]
    public string Feedbackformat { get; set; } = "1";
}