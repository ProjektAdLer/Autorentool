using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.MoodleBackup.xml;

[XmlRoot(ElementName="contents")]
public class MoodleBackupXmlContents : IMoodleBackupXmlContents {

    public MoodleBackupXmlContents()
    {
        Activities = new MoodleBackupXmlActivities();
        Sections = new MoodleBackupXmlSections();
        Course = new MoodleBackupXmlCourse();
    }

    [XmlElement(ElementName="activities")]
    public MoodleBackupXmlActivities Activities { get; set; }
    [XmlElement(ElementName="sections")]
    public MoodleBackupXmlSections Sections { get; set; }
    [XmlElement(ElementName="course")]
    public MoodleBackupXmlCourse Course { get; set; }
}