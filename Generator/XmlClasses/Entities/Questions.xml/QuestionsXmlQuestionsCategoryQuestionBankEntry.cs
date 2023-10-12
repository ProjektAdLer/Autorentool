using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntry
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntry()
    {
        Id = "0";
        IdNumber = "";
    }

    public QuestionsXmlQuestionsCategoryQuestionBankEntry(int id, Guid idNumber)
    {
        IdNumber = idNumber.ToString();
        Id = id.ToString();
    }

    [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

    [XmlElement(ElementName = "questioncategoryid")]
    public string QuestionCategoryId { get; set; } = "4";

    [XmlElement(ElementName = "idnumber")] public string IdNumber { get; set; }

    [XmlElement(ElementName = "ownerid")] public string OwnerId { get; set; } = "0";

    [XmlElement(ElementName = "question_version")]
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersion QuestionVersion { get; set; } = new();
}