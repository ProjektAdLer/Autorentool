using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions
{
    [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions()
    {
    }

    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions(int id)
    {
        Id = id.ToString();
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; } = "0";

    [XmlElement(ElementName = "version")]
    public string Version
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "1";

    [XmlElement(ElementName = "status")]
    public string Status
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "ready";

    [XmlElement(ElementName = "questions")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestions Questions
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}