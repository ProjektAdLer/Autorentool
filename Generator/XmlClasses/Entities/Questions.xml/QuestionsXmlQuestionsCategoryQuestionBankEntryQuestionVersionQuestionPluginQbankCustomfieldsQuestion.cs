using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Questions.xml;

public class QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQbankCustomfieldsQuestion
{
    [XmlElement(ElementName = "customfields")]
    public string CustomFields
    {
        get;
        [ExcludeFromCodeCoverage(Justification = "Used by serialization")]
        set;
    } = String.Empty;
}