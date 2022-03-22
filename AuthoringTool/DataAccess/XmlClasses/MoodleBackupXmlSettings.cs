using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="settings")]
public partial class MoodleBackupXmlSettings {
    [XmlElement(ElementName="setting")]
    public List<MoodleBackupXmlSetting> Setting;
}