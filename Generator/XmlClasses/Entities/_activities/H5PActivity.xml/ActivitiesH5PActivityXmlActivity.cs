using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._activities.H5PActivity.xml;


[XmlRoot(ElementName="activity")]
public class ActivitiesH5PActivityXmlActivity : IActivitiesH5PActivityXmlActivity{

    public ActivitiesH5PActivityXmlActivity()
    {
        H5Pactivity = new ActivitiesH5PActivityXmlH5PActivity();
        Id = "";
        ModuleId = "";
        ModuleName = "";
        ContextId = "";
    }
    
   
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "h5pactivity.xml"));
    }
    
    [XmlElement(ElementName="h5pactivity")]
    public ActivitiesH5PActivityXmlH5PActivity H5Pactivity { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
    
    [XmlAttribute(AttributeName="moduleid")]
    public string ModuleId { get; set; }
    
    [XmlAttribute(AttributeName="modulename")]
    public string ModuleName { get; set; }
    
    [XmlAttribute(AttributeName="contextid")]
    public string ContextId { get; set; }
    
}