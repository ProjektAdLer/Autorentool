using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="activity")]
public class ActivitiesH5PActivityXmlActivity : IActivitiesH5PActivityXmlActivity{
    
    
    public void SetParameterts(ActivitiesH5PActivityXmlH5PActivity? h5Pactivity, string? id, string? moduleid, string? modulename, string? contextid)
    {
        H5pactivity = h5Pactivity;
        Id = id;
        Moduleid = moduleid;
        Modulename = modulename;
        Contextid = contextid;
    }
    
    public void Serialize(string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", "h5pactivity_"+moduleId, "h5pactivity.xml"));
    }
    
    [XmlElement(ElementName="h5pactivity")]
    public ActivitiesH5PActivityXmlH5PActivity? H5pactivity { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
    
    [XmlAttribute(AttributeName="moduleid")]
    public string? Moduleid { get; set; }
    
    [XmlAttribute(AttributeName="modulename")]
    public string? Modulename { get; set; }
    
    [XmlAttribute(AttributeName="contextid")]
    public string? Contextid { get; set; }
    
}