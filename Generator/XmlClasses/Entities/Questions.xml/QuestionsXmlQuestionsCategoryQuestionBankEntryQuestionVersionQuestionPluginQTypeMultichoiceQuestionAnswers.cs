using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswers
{
    public QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswers()
    {
    }

    [XmlElement(ElementName = "answer")]
    public List<
            QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswer>
        Answer
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}