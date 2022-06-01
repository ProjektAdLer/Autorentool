namespace AuthoringTool.DataAccess.XmlClasses;

public interface IMoodleBackupXmlDetail
{
    void SetParameters(string? type, string? format, string? interactive, string? mode,
        string? execution, string? executiontime, string? backupId);
}