using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="activity")]
public class MoodleBackupXmlActivity : IMoodleBackupXmlActivity{
    
    public void SetParameters(string? moduleid, string? sectionid, string? modulename, string? title, string? directory)
    {
        Moduleid = moduleid;
        Sectionid = sectionid;
        Modulename = modulename;
        Title = title;
        Directory = directory;
    }

    
    [XmlElement(ElementName="moduleid")]
    public string? Moduleid { get; set; }
    
    [XmlElement(ElementName="sectionid")]
    public string? Sectionid { get; set; }
    
    [XmlElement(ElementName="modulename")]
    public string? Modulename { get; set; }
    
    [XmlElement(ElementName="title")]
    public string? Title { get; set; }
    
    [XmlElement(ElementName="directory")]
    public string? Directory { get; set; }
}