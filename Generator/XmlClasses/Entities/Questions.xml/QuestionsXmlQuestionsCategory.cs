using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategory
{
    [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
    public QuestionsXmlQuestionsCategory()
    {
        Parent = "0";
        Name = "";
        Id = "0";
        ContextId = "0";
    }

    public QuestionsXmlQuestionsCategory(int id, string name, int contextId, int parent)
    {
        Id = id.ToString();
        Name = name;
        Parent = parent.ToString();
        ContextId = contextId.ToString();
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "name")] public string Name { get; set; }

    [XmlElement(ElementName = "contextid")]
    public string ContextId { get; set; }

    [XmlElement(ElementName = "contextlevel")]
    public string ContextLevel
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "50";

    [XmlElement(ElementName = "contextinstanceid")]
    public string ContextInstanceId
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "info")]
    public string Info
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "";

    [XmlElement(ElementName = "infoformat")]
    public string InfoFormat
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "stamp")]
    public string Stamp
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new(Enumerable
        .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 10)
        .Select(s => s[new Random().Next(s.Length)]).ToArray());

    [XmlElement(ElementName = "parent")] public string Parent { get; set; }

    [XmlElement(ElementName = "sortorder")]
    public string SortOrder
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "idnumber")]
    public string IdNumber
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "$@NULL@$";

    [XmlElement(ElementName = "question_bank_entries")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntries QuestionBankEntries
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}