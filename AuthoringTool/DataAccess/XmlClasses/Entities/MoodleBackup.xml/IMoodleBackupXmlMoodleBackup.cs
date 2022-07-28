namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlMoodleBackup : IXmlSerializable
{
    MoodleBackupXmlInformation Information { get; set; }
}