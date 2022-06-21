namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlMoodleBackup : IXmlSerializable
{
    void SetParameters(MoodleBackupXmlInformation? moodleBackupInformation);
}