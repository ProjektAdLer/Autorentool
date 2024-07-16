using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntry
{
    [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntry()
    {
        Id = "0";
        IdNumber = "";
    }

    public QuestionsXmlQuestionsCategoryQuestionBankEntry(int id, string idNumber)
    {
        IdNumber = idNumber;
        Id = id.ToString();
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "questioncategoryid")]
    public string QuestionCategoryId
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "4";

    [XmlElement(ElementName = "idnumber")] public string IdNumber { get; set; }

    [XmlElement(ElementName = "ownerid")]
    public string OwnerId
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = "0";

    [XmlElement(ElementName = "question_version")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersion QuestionVersion
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}