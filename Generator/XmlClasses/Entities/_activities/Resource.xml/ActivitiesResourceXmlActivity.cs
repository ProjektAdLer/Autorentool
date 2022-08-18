using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._activities.Resource.xml;


[XmlRoot(ElementName="activity")]
public class ActivitiesResourceXmlActivity : IActivitiesResourceXmlActivity{

    public ActivitiesResourceXmlActivity()
    {
        Resource = new ActivitiesResourceXmlResource();
        Id = "";
        ModuleId = "";
        ModuleName = "";
        ContextId = "";
    }
    
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "resource.xml"));
    }

    [XmlElement(ElementName="resource")]
    public ActivitiesResourceXmlResource Resource { get; set; }
        
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
        
    [XmlAttribute(AttributeName="moduleid")]
    public string ModuleId { get; set; }
        
    [XmlAttribute(AttributeName="modulename")]
    public string ModuleName { get; set; }
        
    [XmlAttribute(AttributeName="contextid")]
    public string ContextId { get; set; }
    
}
