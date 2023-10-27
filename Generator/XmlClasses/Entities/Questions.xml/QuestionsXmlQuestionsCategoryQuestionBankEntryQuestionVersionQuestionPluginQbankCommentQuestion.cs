using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCommentQuestion
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCommentQuestion()
    {
    }

    [XmlElement(ElementName = "comments")] public string Comments { get; set; } = String.Empty;
}