using System.Xml;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace AuthoringTool.DataAccess.WorldExport;

public interface IBackupFileGenerator
{
    void CreateBackupFolders();
    void WriteXMLFiles();
    void WriteBackupFile();
}