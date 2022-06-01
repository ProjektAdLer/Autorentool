using System.Xml.Serialization;
using AuthoringTool.DataAccess.XmlClasses.Entities;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="contents")]
public partial class MoodleBackupXmlContents : IMoodleBackupXmlContents {

   
    public void SetParameters(MoodleBackupXmlActivities? moodleBackupXmlActivities, 
        MoodleBackupXmlSections? moodleBackupSections,MoodleBackupXmlCourse? moodleBackupCourse)
    {
        Activities = moodleBackupXmlActivities;
        Sections = moodleBackupSections; 
        Course = moodleBackupCourse;
    }
    
    [XmlElement(ElementName="activities")]
    public MoodleBackupXmlActivities? Activities { get; set; }
    [XmlElement(ElementName="sections")]
    public MoodleBackupXmlSections? Sections { get; set; }
    [XmlElement(ElementName="course")]
    public MoodleBackupXmlCourse? Course { get; set; }
}