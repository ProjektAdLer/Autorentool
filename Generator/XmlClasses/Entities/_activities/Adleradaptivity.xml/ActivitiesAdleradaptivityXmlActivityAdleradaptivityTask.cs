using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public class ActivitiesAdleradaptivityXmlActivityAdleradaptivityTask
{
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTask()
    {
        Id = "0";
        Title = "";
        Uuid = "";
        RequiredDifficulty = "0";
    }

    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTask(int id, string title, string uuid,
        string requiredDifficulty)
    {
        Id = id.ToString();
        Title = title;
        Uuid = uuid;
        RequiredDifficulty = requiredDifficulty;
    }

    [XmlAttribute("id")] public string Id { get; set; }

    [XmlElement("title")] public string Title { get; set; }

    [XmlElement("uuid")] public string Uuid { get; set; }

    [XmlElement("required_difficulty")] public string RequiredDifficulty { get; set; }

    [XmlElement("questions")]
    public ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestions Questions { get; set; } = new();
}