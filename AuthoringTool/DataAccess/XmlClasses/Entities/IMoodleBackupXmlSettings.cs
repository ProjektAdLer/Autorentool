namespace AuthoringTool.DataAccess.XmlClasses;

public interface IMoodleBackupXmlSettings
{
    void SetParameters();

    void FillSettings(MoodleBackupXmlSetting moodleBackupXmlSetting);
}