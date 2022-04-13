namespace AuthoringTool.DataAccess.WorldExport;

public interface IBackupFileGenerator
{
    /// <summary>
    /// Creates all Folders needed for the Backup File
    /// </summary>
    void CreateBackupFolders();
    void WriteXmlFiles();
    void WriteBackupFile();
}