using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="moodle_backup")]
public partial class MoodleBackupXmlMoodleBackup {
    [XmlElement(ElementName="information")]
    public MoodleBackupXmlInformation Information { get; set; }
}