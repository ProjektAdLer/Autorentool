using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="resource")]
public class ActivitiesResourceXmlResource : IActivitiesResourceXmlResource{
    
    public void SetParameters(string? name, string? intro, string? introformat, string? tobemigrated, string? legacyfiles, 
        string? legacyfileslast, string? display, string? displayoptions, string? filterfiles, string? revision, 
        string? timemodified, string? id)
    {
        Name = name;
        Intro = intro;
        Introformat = introformat;
        Tobemigrated = tobemigrated;
        Legacyfiles = legacyfiles;
        Legacyfileslast = legacyfileslast;
        Display = display;
        Displayoptions = displayoptions;
        Filterfiles = filterfiles;
        Revision = revision;
        Timemodified = timemodified;
        Id = id;
    }

    [XmlElement(ElementName="name")]
    public string? Name { get; set; }
    
    [XmlElement(ElementName="intro")]
    public string? Intro { get; set; }
    
    [XmlElement(ElementName="introformat")]
    public string? Introformat { get; set; }
    
    [XmlElement(ElementName="tobemigrated")]
    public string? Tobemigrated { get; set; }
    
    [XmlElement(ElementName="legacyfiles")]
    public string? Legacyfiles { get; set; }
    
    [XmlElement(ElementName="legacyfileslast")]
    public string? Legacyfileslast { get; set; }
    
    [XmlElement(ElementName="display")]
    public string? Display { get; set; }
    
    [XmlElement(ElementName="displayoptions")]
    public string? Displayoptions { get; set; }
    
    [XmlElement(ElementName="filterfiles")]
    public string? Filterfiles { get; set; }
    
    [XmlElement(ElementName="revision")]
    public string? Revision { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string? Timemodified { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
    
}
