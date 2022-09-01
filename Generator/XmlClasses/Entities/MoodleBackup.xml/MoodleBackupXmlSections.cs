using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.MoodleBackup.xml;

[XmlRoot(ElementName="sections")]
public class MoodleBackupXmlSections : IMoodleBackupXmlSections{

    public MoodleBackupXmlSections()
    {
        Section = new List<MoodleBackupXmlSection>();
    }
    
    
    [XmlElement(ElementName="section")]
    public List<MoodleBackupXmlSection> Section { get; set; }
}