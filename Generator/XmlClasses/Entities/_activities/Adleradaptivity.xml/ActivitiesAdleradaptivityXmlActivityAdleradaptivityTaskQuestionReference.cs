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

    [XmlAttribute("usingcontextid")] public string UsingContextId { get; set; } = "0";

    [XmlAttribute("component")] public string Component { get; set; } = "mod_adleradaptivity";

    [XmlAttribute("questionarea")] public string QuestionArea { get; set; } = "question";

    [XmlAttribute("questionbankentryid")] public string QuestionBankEntryId { get; set; }

    [XmlAttribute("version")] public string Version { get; set; } = "1";
}