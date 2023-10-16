using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;

public class ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask
{
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask()
    {
        Id = "0";
        Title = "";
        Uuid = "";
        Optional = "0";
        RequiredDifficulty = "0";
    }

    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask(int id, string title, string uuid, int optional,
        string requiredDifficulty)
    {
        Id = id.ToString();
        Title = title;
        Uuid = uuid;
        Optional = optional.ToString();
        RequiredDifficulty = requiredDifficulty;
    }

    [XmlAttribute("id")] public string Id { get; set; }

    [XmlElement("title")] public string Title { get; set; }

    [XmlElement("uuid")] public string Uuid { get; set; }

    [XmlElement("optional")] public string Optional { get; set; }

    [XmlElement("required_difficulty")] public string RequiredDifficulty { get; set; }

    [XmlElement("questions")]
    public ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestions Questions { get; set; } = new();
}