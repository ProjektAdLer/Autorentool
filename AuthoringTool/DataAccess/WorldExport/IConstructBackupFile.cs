using System.Xml;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace AuthoringTool.DataAccess.WorldExport;

public interface IConstructBackupFile
{
    void CreateXMLFiles();
    void OverwriteEncoding();
    void CreateBackupFile();
}