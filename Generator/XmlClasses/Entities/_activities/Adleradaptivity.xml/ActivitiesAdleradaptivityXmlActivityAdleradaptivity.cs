using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivity
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivity()
    {
        Id = "0";
        Name = "";
    }

    public ActivitiesAdleradaptivityXmlActivityAdleradaptivity(string id, string name)
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

    [XmlElement("tasks")] public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTasks Tasks { get; set; } = new();

    [XmlElement("attempts")]
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityAttempts Attempts { get; set; } = new();
}