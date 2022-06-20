using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="activity")]
public class ActivitiesResourceXmlActivity : IActivitiesResourceXmlActivity{
    
    public void SetParameters(ActivitiesResourceXmlResource? resource, string? id, string? moduleid, string? modulename, 
        string? contextid)
    {
        Resource = resource;
        Id = id;
        Moduleid = moduleid;
        Modulename = modulename;
        Contextid = contextid;
    }
    
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "resource.xml"));
    }

    [XmlElement(ElementName="resource")]
    public ActivitiesResourceXmlResource? Resource { get; set; }
        
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
        
    [XmlAttribute(AttributeName="moduleid")]
    public string? Moduleid { get; set; }
        
    [XmlAttribute(AttributeName="modulename")]
    public string? Modulename { get; set; }
        
    [XmlAttribute(AttributeName="contextid")]
    public string? Contextid { get; set; }
    
}
