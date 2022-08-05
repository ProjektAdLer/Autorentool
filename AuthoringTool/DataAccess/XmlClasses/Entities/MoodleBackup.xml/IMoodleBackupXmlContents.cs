namespace AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

public interface IMoodleBackupXmlContents
{
    
    MoodleBackupXmlActivities Activities { get; set; }
    MoodleBackupXmlSections Sections { get; set; }
    MoodleBackupXmlCourse Course { get; set; }
}