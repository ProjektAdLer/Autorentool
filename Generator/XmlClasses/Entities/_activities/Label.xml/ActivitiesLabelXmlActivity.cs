using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._activities.Label.xml;

[XmlRoot(ElementName = "activity")] 
public class ActivitiesLabelXmlActivity : IActivitiesLabelXmlActivity
{
    public ActivitiesLabelXmlActivity()
    {
        Label = new ActivitiesLabelXmlLabel();
        Id = "";
        ModuleId = "";
        ModuleName = "label";
        ContextId = "";
    }
    
    public void Serialize(string activityName, string moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this,Path.Join("activities", activityName + "_" + moduleId, "label.xml"));
    }

    [XmlElement(ElementName = "label")] 
    public ActivitiesLabelXmlLabel Label { get; set; }
    
    [XmlAttribute(AttributeName = "id")] 
    public string Id { get; set; }

    [XmlAttribute(AttributeName = "moduleid")]
    public string ModuleId { get; set; }

    [XmlAttribute(AttributeName = "modulename")]
    public string ModuleName { get; set; }

    [XmlAttribute(AttributeName = "contextid")]
    public string ContextId { get; set; }
}