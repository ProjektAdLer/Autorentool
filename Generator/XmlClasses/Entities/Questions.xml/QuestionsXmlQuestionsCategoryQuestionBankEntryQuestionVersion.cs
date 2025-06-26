using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersion
{
    [XmlElement(ElementName = "question_versions")]
    public List<QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions> QuestionVersions
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = new();
}