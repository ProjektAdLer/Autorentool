using System.Diagnostics.CodeAnalysis;
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
        Incorrectfeedback = "";
        Partiallycorrectfeedback = "";
    }

    public
        QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionMultichoice(
            int id, int single, string incorrectfeedback)
    {
        Id = id.ToString();
        Single = single.ToString();
        Incorrectfeedback = incorrectfeedback;
        Partiallycorrectfeedback = incorrectfeedback;
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "layout")]
    public string Layout
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "single")] public string Single { get; set; }

    [XmlElement(ElementName = "shuffleanswers")]
    public string Shuffleanswers
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "correctfeedback")]
    public string Correctfeedback
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "Diese Antwort ist korrekt.";

    [XmlElement(ElementName = "correctfeedbackformat")]
    public string Correctfeedbackformat
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "partiallycorrectfeedback")]
    public string Partiallycorrectfeedback { get; set; }

    [XmlElement(ElementName = "partiallycorrectfeedbackformat")]
    public string Partiallycorrectfeedbackformat
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "incorrectfeedback")]
    public string Incorrectfeedback { get; set; }

    [XmlElement(ElementName = "incorrectfeedbackformat")]
    public string Incorrectfeedbackformat
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "answernumbering")]
    public string Answernumbering
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "abc";

    [XmlElement(ElementName = "shownumcorrect")]
    public string Shownumcorrect
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "showstandardinstruction")]
    public string Showstandardinstruction
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";
}