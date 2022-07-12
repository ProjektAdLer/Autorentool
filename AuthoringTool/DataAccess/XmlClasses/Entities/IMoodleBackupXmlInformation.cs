namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlInformation
{
    void SetParameters(string? name, string? moodleVersion, string? moodleRelease, string? backupVersion,
        string? backupRelease, string? backupDate, string? mnetRemoteusers, string? includeFiles,
        string? includeFileReferencesToExternalContent, string? originalWwwroot, string? originalSiteIdentifierHash,
        string? originalCourseId, string? originalCourseFormat, string? originalCourseFullname,
        string? originalCourseShortname, string? originalCourseStartdate, string? originalCourseEnddate,
        string? originalCourseContextid, string? originalSystemContextid, MoodleBackupXmlDetails? details,
        MoodleBackupXmlContents? contents, MoodleBackupXmlSettings? settings);
}