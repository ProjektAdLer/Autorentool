namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlActivity
{
    void SetParameters(string? moduleid, string? sectionid, string? modulename, string? title, string? directory);
}