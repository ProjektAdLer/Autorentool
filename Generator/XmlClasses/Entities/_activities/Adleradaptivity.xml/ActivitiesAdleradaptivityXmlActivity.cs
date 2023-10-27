using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

[XmlRoot(ElementName = "activity")]
public class ActivitiesAdleradaptivityXmlActivity : IActivitiesAdleradaptivityXmlActivity
{
    public ActivitiesAdleradaptivityXmlActivity()
    {
        Id = "0";
        ModuleId = "0";
    }

    public ActivitiesAdleradaptivityXmlActivity(string id)
    {
        Id = id;
        ModuleId = id;
    }

    [XmlAttribute("id")] public string Id { get; set; }

    [XmlAttribute("moduleid")] public string ModuleId { get; set; }

    [XmlAttribute("modulename")] public string ModuleName { get; set; } = "adleradaptivity";

    [XmlAttribute("contextid")] public string ContextId { get; set; } = "0";

    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "adleradaptivity.xml"));
    }

    [XmlElement("adleradaptivity")]
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivity Adleradaptivity { get; set; } = new();
}