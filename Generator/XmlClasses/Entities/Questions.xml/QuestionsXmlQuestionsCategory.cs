using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategory
{
    public QuestionsXmlQuestionsCategory()
    {
        Name = "";
        Id = "0";
    }

    public QuestionsXmlQuestionsCategory(int id, string name)
    {
        Id = id.ToString();
        Name = name;
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "name")] public string Name { get; set; }

    [XmlElement(ElementName = "contextid")]
    public string ContextId { get; set; } = "0";

    [XmlElement(ElementName = "contextlevel")]
    public string ContextLevel { get; set; } = "0";

    [XmlElement(ElementName = "contextinstanceid")]
    public string ContextInstanceId { get; set; } = "0";

    [XmlElement(ElementName = "info")] public string Info { get; set; } = "";

    [XmlElement(ElementName = "infoformat")]
    public string InfoFormat { get; set; } = "0";

    [XmlElement(ElementName = "stamp")] public string Stamp { get; set; } = "";

    [XmlElement(ElementName = "parent")] public string Parent { get; set; } = "0";

    [XmlElement(ElementName = "sortorder")]
    public string SortOrder { get; set; } = "0";

    [XmlElement(ElementName = "idnumber")] public string IdNumber { get; set; } = "$@NULL@$";

    [XmlElement(ElementName = "question_bank_entries")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntries QuestionBankEntries { get; set; } = new();
}