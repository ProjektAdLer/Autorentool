namespace AuthoringTool.DataAccess.XmlClasses;

public interface IMoodleBackupXmlSetting
{
    void SetParametersShort(string name, string value);

    void SetParametersFull(string name, string value, string level, string section);
}