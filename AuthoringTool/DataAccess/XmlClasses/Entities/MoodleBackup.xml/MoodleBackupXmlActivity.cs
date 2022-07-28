using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="activity")]
public class MoodleBackupXmlActivity : IMoodleBackupXmlActivity{

    public MoodleBackupXmlActivity()
    {
        ModuleId = "";
        SectionId = "";
        ModuleName = "";
        Title = "";
        Directory = "";
    }

    [XmlElement(ElementName="moduleid")]
    public string ModuleId { get; set; }
    
    [XmlElement(ElementName="sectionid")]
    public string SectionId { get; set; }
    
    [XmlElement(ElementName="modulename")]
    public string ModuleName { get; set; }
    
    [XmlElement(ElementName="title")]
    public string Title { get; set; }
    
    [XmlElement(ElementName="directory")]
    public string Directory { get; set; }
}