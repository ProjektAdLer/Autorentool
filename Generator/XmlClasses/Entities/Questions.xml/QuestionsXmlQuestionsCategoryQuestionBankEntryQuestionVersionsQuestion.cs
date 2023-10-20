using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion()
    {
        Id = "0";
        QuestionText = "";
    }

    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion(int id, string questionText)
    {
        Id = id.ToString();
        QuestionText = questionText;
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "parent")] public string Parent { get; set; } = "0";

    [XmlElement(ElementName = "name")] public string Name { get; set; } = "";

    [XmlElement(ElementName = "questiontext")]
    public string QuestionText { get; set; }

    [XmlElement(ElementName = "questiontextformat")]
    public string QuestionTextFormat { get; set; } = "1";

    [XmlElement(ElementName = "generalfeedback")]
    public string GeneralFeedback { get; set; } = "";

    [XmlElement(ElementName = "generalfeedbackformat")]
    public string GeneralFeedbackFormat { get; set; } = "1";

    [XmlElement(ElementName = "defaultmark")]
    public string DefaultMark { get; set; } = "1.0000000";

    [XmlElement(ElementName = "penalty")] public string Penalty { get; set; } = "0.3333333";

    [XmlElement(ElementName = "qtype")] public string QType { get; set; } = "multichoice";

    [XmlElement(ElementName = "length")] public string Length { get; set; } = "1";

    [XmlElement(ElementName = "stamp")]
    public string Stamp { get; set; } = new(Enumerable
        .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 10)
        .Select(s => s[new Random().Next(s.Length)]).ToArray());

    [XmlElement(ElementName = "timecreated")]
    public string TimeCreated { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

    [XmlElement(ElementName = "timemodified")]
    public string TimeModified { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

    [XmlElement(ElementName = "createdby")]
    public string CreatedBy { get; set; } = "0";

    [XmlElement(ElementName = "modifiedby")]
    public string ModifiedBy { get; set; } = "0";

    [XmlElement(ElementName = "plugin_qtype_multichoice_question")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestion
        PluginQTypeMultichoiceQuestion { get; set; } = new();

    [XmlElement(ElementName = "plugin_qbank_comment_question")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCommentQuestion
        PluginQbankCommentQuestion { get; set; } = new();

    [XmlElement(ElementName = "plugin_qbank_customfields_question")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCustomfieldsQuestion
        PluginQbankCustomfieldsQuestion { get; set; } = new();

    [XmlElement(ElementName = "question_hints")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionQuestionHints QuestionHints
    {
        get;
        set;
    } = new();

    [XmlElement(ElementName = "tags")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionTags Tags { get; set; } = new();
}