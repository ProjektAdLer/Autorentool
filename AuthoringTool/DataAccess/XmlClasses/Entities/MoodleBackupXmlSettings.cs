using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="settings")]
public partial class MoodleBackupXmlSettings : IMoodleBackupXmlSettings{

    
    public void SetParameters()
    {
        Setting = new List<MoodleBackupXmlSetting?>();
    }

    public void FillSettings(MoodleBackupXmlSetting? moodleBackupXmlSetting)
    {
        Setting.Add(moodleBackupXmlSetting);
    }
    
    [XmlElement(ElementName="setting")]
    public List<MoodleBackupXmlSetting?> Setting;
}