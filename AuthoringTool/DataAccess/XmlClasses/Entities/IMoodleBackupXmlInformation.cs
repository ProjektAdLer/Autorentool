namespace AuthoringTool.DataAccess.XmlClasses;

public interface IMoodleBackupXmlInformation
{
    void SetParameters(string name, string originalCourseId,
        string originalCourseFormat, string originalCourseFullname, string originalCourseShortname,
        string originalCourseContextid, string originalSystemContextid, string originalCourseStartdate, 
        string originalCourseEnddate, MoodleBackupXmlDetails? backupDetails, MoodleBackupXmlContents? backupContents, 
        MoodleBackupXmlSettings? backupSettings);
}