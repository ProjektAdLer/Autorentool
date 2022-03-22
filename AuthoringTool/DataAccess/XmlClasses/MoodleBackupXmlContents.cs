using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="contents")]
public partial class MoodleBackupXmlContents {
    [XmlElement(ElementName="sections")]
    public MoodleBackupXmlSections Sections;
    [XmlElement(ElementName="course")]
    public MoodleBackupXmlCourse Course;
}