namespace AuthoringTool.DataAccess.WorldExport;

public interface IBackupFileGenerator
{
    /// <summary>
    /// Creates all Folders needed for the Backup File
    /// </summary>
    void CreateBackupFolders();
    
    /// <summary>
    /// Creates all Xml Files at the right location.
    /// </summary>
    void WriteXmlFiles();
    
    /// <summary>
    /// Locates all Folders and Xml Files, packs it into a tar.gzip file and renames it to .mbz (moodle backup zip) 
    /// </summary>
    void WriteBackupFile();
}