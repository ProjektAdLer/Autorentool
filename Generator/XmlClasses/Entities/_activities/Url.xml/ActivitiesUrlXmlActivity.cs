using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._activities.Url.xml;

[XmlRoot(ElementName = "activity")]
public class ActivitiesUrlXmlActivity : IActivitiesUrlXmlActivity
{
    public ActivitiesUrlXmlActivity()
    {
        Url = new ActivitiesUrlXmlUrl();
        Id = "";
        Moduleid = "";
        Modulename = "url";
        Contextid = "";
    }
    
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "url.xml"));
    }

    [XmlElement(ElementName="url")]
    public ActivitiesUrlXmlUrl Url { get; set; }
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
    [XmlAttribute(AttributeName="moduleid")]
    public string Moduleid { get; set; }
    [XmlAttribute(AttributeName="modulename")]
    public string Modulename { get; set; }
    [XmlAttribute(AttributeName="contextid")]
    public string Contextid { get; set; }
}