using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntries
{
    [XmlElement(ElementName = "question_bank_entry")]
    public List<QuestionsXmlQuestionsCategoryQuestionBankEntry> QuestionBankEntries
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}