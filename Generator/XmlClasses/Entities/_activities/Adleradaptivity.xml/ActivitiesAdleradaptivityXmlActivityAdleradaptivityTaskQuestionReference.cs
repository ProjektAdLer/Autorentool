using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestionReference
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestionReference()
    {
        Id = "0";
        QuestionBankEntryId = "0";
    }

    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestionReference(int id)
    {
        Id = id.ToString();
        QuestionBankEntryId = id.ToString();
    }

    [XmlAttribute("id")] public string Id { get; set; }

    [XmlElement("usingcontextid")] public string UsingContextId { get; set; } = "0";

    [XmlElement("component")] public string Component { get; set; } = "mod_adleradaptivity";

    [XmlElement("questionarea")] public string QuestionArea { get; set; } = "question";

    [XmlElement("questionbankentryid")] public string QuestionBankEntryId { get; set; }

    [XmlElement("version")] public string Version { get; set; } = "1";
}