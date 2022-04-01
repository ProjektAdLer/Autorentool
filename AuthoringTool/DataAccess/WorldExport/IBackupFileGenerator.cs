using System.Xml;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace AuthoringTool.DataAccess.WorldExport;

public interface IBackupFileGenerator
{
    /// <summary>
    /// Creates all Folders needed for the Backup File
    /// </summary>
    void CreateBackupFolders();
    void WriteXMLFiles();
    void WriteBackupFile();
}