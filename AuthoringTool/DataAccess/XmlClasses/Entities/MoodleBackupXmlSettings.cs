using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="settings")]
public partial class MoodleBackupXmlSettings : IMoodleBackupXmlSettings{

    
    public void SetParameters(List<MoodleBackupXmlSetting?>? moodleBackupXmlSetting)
    {
        Setting = moodleBackupXmlSetting;
    }
    
    
    [XmlElement(ElementName="setting")]
    public List<MoodleBackupXmlSetting?>? Setting;
}