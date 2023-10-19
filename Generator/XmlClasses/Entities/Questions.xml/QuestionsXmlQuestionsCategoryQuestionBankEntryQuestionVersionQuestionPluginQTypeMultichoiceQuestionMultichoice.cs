using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class
    QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionMultichoice
{
    public
        QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionMultichoice()
    {
        Id = "0";
        Single = "1";
    }

    public
        QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionMultichoice(
            int id, int single)
    {
        Id = id.ToString();
        Single = single.ToString();
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "layout")] public string Layout { get; set; } = "0";

    [XmlElement(ElementName = "single")] public string Single { get; set; }

    [XmlElement(ElementName = "shuffleanswers")]
    public string Shuffleanswers { get; set; } = "0";

    [XmlElement(ElementName = "correctfeedback")]
    public string Correctfeedback { get; set; } = "";

    [XmlElement(ElementName = "correctfeedbackformat")]
    public string Correctfeedbackformat { get; set; } = "1";

    [XmlElement(ElementName = "partiallycorrectfeedback")]
    public string Partiallycorrectfeedback { get; set; } = "";

    [XmlElement(ElementName = "partiallycorrectfeedbackformat")]
    public string Partiallycorrectfeedbackformat { get; set; } = "1";

    [XmlElement(ElementName = "incorrectfeedback")]
    public string Incorrectfeedback { get; set; } = "";

    [XmlElement(ElementName = "incorrectfeedbackformat")]
    public string Incorrectfeedbackformat { get; set; } = "1";

    [XmlElement(ElementName = "answernumbering")]
    public string Answernumbering { get; set; } = "abc";

    [XmlElement(ElementName = "shownumcorrect")]
    public string Shownumcorrect { get; set; } = "1";

    [XmlElement(ElementName = "showstandardinstruction")]
    public string Showstandardinstruction { get; set; } = "0";
}