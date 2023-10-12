using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCustomfieldsQuestion
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCustomfieldsQuestion()
    {
    }

    [XmlElement(ElementName = "customfields")]
    public string CustomFields { get; set; } = String.Empty;
}