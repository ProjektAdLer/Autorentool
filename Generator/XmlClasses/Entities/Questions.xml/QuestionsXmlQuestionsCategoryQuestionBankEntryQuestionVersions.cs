using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions()
    {
    }

    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions(int id)
    {
        Id = id.ToString();
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; } = "0";

    [XmlElement(ElementName = "version")] public string Version { get; set; } = "1";

    [XmlElement(ElementName = "status")] public string Status { get; set; } = "ready";

    [XmlElement(ElementName = "questions")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestions Questions { get; set; } = new();
}