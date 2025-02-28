using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestions
{
    [XmlElement(ElementName = "question")]
    public List<QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion> Question
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}