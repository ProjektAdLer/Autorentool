using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersion
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersion()
    {
    }


    [XmlElement(ElementName = "question_versions")]
    public List<QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions> QuestionVersions { get; set; } = new();
}