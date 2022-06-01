namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlContents
{
    void SetParameters(MoodleBackupXmlActivities? moodleBackupXmlActivities,
        MoodleBackupXmlSections? moodleBackupSections, MoodleBackupXmlCourse? moodleBackupCourse);
}