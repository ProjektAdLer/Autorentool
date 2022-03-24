using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="settings")]
public partial class MoodleBackupXmlSettings {

    public MoodleBackupXmlSettings()
    {
        Setting = new List<MoodleBackupXmlSetting>();
    }
    
    [XmlElement(ElementName="setting")]
    public List<MoodleBackupXmlSetting> Setting;
}