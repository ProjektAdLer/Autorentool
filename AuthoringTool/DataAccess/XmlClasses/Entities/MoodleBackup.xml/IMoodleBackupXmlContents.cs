namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlContents
{
    
    MoodleBackupXmlActivities Activities { get; set; }
    MoodleBackupXmlSections Sections { get; set; }
    MoodleBackupXmlCourse Course { get; set; }
}