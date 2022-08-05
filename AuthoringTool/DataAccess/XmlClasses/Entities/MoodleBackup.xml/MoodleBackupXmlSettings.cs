using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[XmlRoot(ElementName="settings")]
public class MoodleBackupXmlSettings : IMoodleBackupXmlSettings{

    public MoodleBackupXmlSettings()
    {
        Setting = new List<MoodleBackupXmlSetting>();
    }
    
    
    [XmlElement(ElementName="setting")]
    public List<MoodleBackupXmlSetting> Setting { get; set; }
}