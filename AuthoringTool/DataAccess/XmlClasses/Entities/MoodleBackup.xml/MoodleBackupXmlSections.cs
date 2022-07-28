using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="sections")]
public partial class MoodleBackupXmlSections : IMoodleBackupXmlSections{

    public MoodleBackupXmlSections()
    {
        Section = new List<MoodleBackupXmlSection>();
    }
    
    
    [XmlElement(ElementName="section")]
    public List<MoodleBackupXmlSection> Section { get; set; }
}