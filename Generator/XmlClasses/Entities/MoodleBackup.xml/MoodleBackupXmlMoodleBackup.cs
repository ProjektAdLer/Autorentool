using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities.MoodleBackup.xml;

[XmlRoot(ElementName = "moodle_backup")]
public class MoodleBackupXmlMoodleBackup : IMoodleBackupXmlMoodleBackup
{
    public MoodleBackupXmlMoodleBackup()
    {
        Information = new MoodleBackupXmlInformation();
    }

    public MoodleBackupXmlMoodleBackup(int contextId)
    {
        Information = new MoodleBackupXmlInformation(contextId);
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "moodle_backup.xml");
    }

    [XmlElement(ElementName = "information")]
    public MoodleBackupXmlInformation Information { get; set; }
}