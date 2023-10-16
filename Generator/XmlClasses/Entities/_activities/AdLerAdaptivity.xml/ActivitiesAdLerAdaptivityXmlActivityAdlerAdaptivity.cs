using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;

public class ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivity
{
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivity()
    {
        Id = "0";
        Name = "";
    }

    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivity(string id, string name)
    {
        Id = id;
        Name = name;
    }

    [XmlAttribute("id")] public string Id { get; set; }

    [XmlElement("name")] public string Name { get; set; }

    [XmlElement("intro")] public string Intro { get; set; } = "";

    [XmlElement("introformat")] public string IntroFormat { get; set; } = "1";

    [XmlElement("timemodified")]
    public string TimeModified { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

    [XmlElement("tasks")] public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTasks Tasks { get; set; } = new();

    [XmlElement("attempts")]
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityAttempts Attempts { get; set; } = new();
}