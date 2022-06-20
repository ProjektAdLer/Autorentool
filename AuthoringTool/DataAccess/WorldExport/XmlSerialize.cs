using System.IO.Abstractions;
using System.Xml;
using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.WorldExport;

/// <summary>
/// Serialize XML classes to xml.files
/// </summary>
public class XmlSerialize
{
    private IFileSystem _fileSystem;
    
    //The Constructor is needed for testing. It is initialized as a new Filesystem() doing nothing ordinary.
    //But it can be changed with the static property FileSystem to make it possible to test the Serialize Method.
    public XmlSerialize()
    {
        _fileSystem = XmlSerializeFileSystemProvider.FileSystem;
    }
    
    /// <summary>
    /// It takes an Xml-objekt and writes it to the desired location. The Startlocation is defined, it is possible to
    /// navigate to folders located deeper in the structure. The Method uses the UpperCase Class and deletes the Xml-Namespace
    /// as it is accepted by the moodle restore process. 
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="xmlname"></param>
    public void Serialize(object xml,string xmlname)
    {
        var curWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        var path = Path.Join(curWorkDir, "XMLFilesForExport", xmlname);
        
        var settings = new XmlWriterSettings
        {
            Encoding = new UpperCaseUTF8Encoding(), // Moodle needs Encoding in Uppercase!
            NewLineHandling = NewLineHandling.Replace,
            NewLineOnAttributes = true,
            Indent = true // Generate new lines for each element
        };
        
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        XmlSerializer x = new XmlSerializer(xml.GetType());
        using var stream = _fileSystem.File.OpenWrite(path);
        using (var xmlWriter = XmlWriter.Create(stream, settings))
        {
            x.Serialize(xmlWriter, xml, ns);
        }
    }
    
}