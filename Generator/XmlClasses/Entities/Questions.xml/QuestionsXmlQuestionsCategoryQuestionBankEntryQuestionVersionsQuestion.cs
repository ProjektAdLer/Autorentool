using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion
{
    [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion()
    {
        Id = "0";
        QuestionText = "";
        Name = "";
    }

    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion(int id, string questionText)
    {
        Id = id.ToString();
        QuestionText = questionText;
        // The name of the question is limited to 254 characters because of moodle database restrictions
        Name = questionText.Length >= 255 ? questionText[..255] : questionText;
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "parent")]
    public string Parent
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "name")] public string Name { get; set; }

    [XmlElement(ElementName = "questiontext")]
    public string QuestionText { get; set; }

    [XmlElement(ElementName = "questiontextformat")]
    public string QuestionTextFormat
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "generalfeedback")]
    public string GeneralFeedback
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "";

    [XmlElement(ElementName = "generalfeedbackformat")]
    public string GeneralFeedbackFormat
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "defaultmark")]
    public string DefaultMark
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1.0000000";

    [XmlElement(ElementName = "penalty")]
    public string Penalty
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0.3333333";

    [XmlElement(ElementName = "qtype")]
    public string QType
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "multichoice";

    [XmlElement(ElementName = "length")]
    public string Length
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "stamp")]
    public string Stamp
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new(Enumerable
        .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 10)
        .Select(s => s[new Random().Next(s.Length)]).ToArray());

    [XmlElement(ElementName = "timecreated")]
    public string TimeCreated
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

    [XmlElement(ElementName = "timemodified")]
    public string TimeModified
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

    [XmlElement(ElementName = "createdby")]
    public string CreatedBy
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "modifiedby")]
    public string ModifiedBy
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "plugin_qtype_multichoice_question")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestion
        PluginQTypeMultichoiceQuestion { get; set; } = new();

    [XmlElement(ElementName = "plugin_qbank_comment_question")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCommentQuestion
        PluginQbankCommentQuestion
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();

    [XmlElement(ElementName = "plugin_qbank_customfields_question")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCustomfieldsQuestion
        PluginQbankCustomfieldsQuestion
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();

    [XmlElement(ElementName = "question_hints")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionQuestionHints QuestionHints
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();

    [XmlElement(ElementName = "tags")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionTags Tags
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}