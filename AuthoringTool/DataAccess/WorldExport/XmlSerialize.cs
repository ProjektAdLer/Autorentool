using System.IO;
using System.IO.Abstractions;
using System.Xml;
using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.WorldExport;

public class XmlSerialize
{
    private IFileSystem _fileSystem;
    public XmlSerialize()
    {
        _fileSystem = XmlSerializeFileSystemProvider.FileSystem;
    }
    
    public void Serialize(object xml,string xmlname)
    {
        var curWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        var path_1 = Path.Join(curWorkDir, "XMLFilesForExport");
        var path_2 = Path.Join(path_1, xmlname);
        
        var settings = new XmlWriterSettings
        {
            Encoding = new UpperCaseUTF8Encoding(), // Moodle needs Encoding in Uppercase!
            NewLineHandling = System.Xml.NewLineHandling.Replace,
            NewLineOnAttributes = true,
            Indent = true // Generate new lines for each element
        };
        
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        XmlSerializer x = new XmlSerializer(xml.GetType());
        using var stream = _fileSystem.File.OpenWrite(path_2);
        using (var xmlWriter = XmlWriter.Create(stream, settings))
        {
            x.Serialize(xmlWriter, xml, ns);
        }
    }
    
}