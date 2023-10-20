using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategory
{
    public QuestionsXmlQuestionsCategory()
    {
        Name = "";
        Id = "0";
        ContextId = "0";
    }

    public QuestionsXmlQuestionsCategory(int id, string name, int contextId)
    {
        Id = id.ToString();
        Name = name;
        ContextId = contextId.ToString();
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "name")] public string Name { get; set; }

    [XmlElement(ElementName = "contextid")]
    public string ContextId { get; set; }

    [XmlElement(ElementName = "contextlevel")]
    public string ContextLevel { get; set; } = "50";

    [XmlElement(ElementName = "contextinstanceid")]
    public string ContextInstanceId { get; set; } = "1";

    [XmlElement(ElementName = "info")] public string Info { get; set; } = "";

    [XmlElement(ElementName = "infoformat")]
    public string InfoFormat { get; set; } = "0";

    [XmlElement(ElementName = "stamp")]
    public string Stamp { get; set; } = new(Enumerable
        .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 10)
        .Select(s => s[new Random().Next(s.Length)]).ToArray());

    [XmlElement(ElementName = "parent")] public string Parent { get; set; } = "3";

    [XmlElement(ElementName = "sortorder")]
    public string SortOrder { get; set; } = "0";

    [XmlElement(ElementName = "idnumber")] public string IdNumber { get; set; } = "$@NULL@$";

    [XmlElement(ElementName = "question_bank_entries")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntries QuestionBankEntries { get; set; } = new();
}