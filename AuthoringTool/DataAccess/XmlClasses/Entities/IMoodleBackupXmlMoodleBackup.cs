namespace AuthoringTool.DataAccess.XmlClasses;

public interface IMoodleBackupXmlMoodleBackup : IXmlSerializable
{
    void SetParameters(MoodleBackupXmlInformation? moodleBackupInformation);
}