using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="resource")]
public class ActivitiesResourceXmlResource : IActivitiesResourceXmlResource{

    public ActivitiesResourceXmlResource()
    {
        Name = "";
        Intro = "";
        IntroFormat = "1";
        TobeMigrated = "0";
        Legacyfiles = "0";
        Legacyfileslast = "$@NULL@$";
        Display = "0";
        DisplayOptions = "";
        FilterFiles = "0";
        Revision = "0";
        Timemodified = "";
        Id = "";
    }
    
   
    [XmlElement(ElementName="name")]
    public string Name { get; set; }
    
    [XmlElement(ElementName="intro")]
    public string Intro { get; set; }
    
    [XmlElement(ElementName="introformat")]
    public string IntroFormat { get; set; }
    
    [XmlElement(ElementName="tobemigrated")]
    public string TobeMigrated { get; set; }
    
    [XmlElement(ElementName="legacyfiles")]
    public string Legacyfiles { get; set; }
    
    [XmlElement(ElementName="legacyfileslast")]
    public string Legacyfileslast { get; set; }
    
    [XmlElement(ElementName="display")]
    public string Display { get; set; }
    
    [XmlElement(ElementName="displayoptions")]
    public string DisplayOptions { get; set; }
    
    [XmlElement(ElementName="filterfiles")]
    public string FilterFiles { get; set; }
    
    [XmlElement(ElementName="revision")]
    public string Revision { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string Timemodified { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
    
}
