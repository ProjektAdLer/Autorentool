using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="contents")]
public partial class MoodleBackupXmlContents {

   
    public void SetParameters(MoodleBackupXmlSections moodleBackupSections,MoodleBackupXmlCourse moodleBackupCourse)
    {
        Sections = moodleBackupSections;
                Course = moodleBackupCourse;
    }
    
    [XmlElement(ElementName="sections")]
    public MoodleBackupXmlSections Sections;
    [XmlElement(ElementName="course")]
    public MoodleBackupXmlCourse Course;
}