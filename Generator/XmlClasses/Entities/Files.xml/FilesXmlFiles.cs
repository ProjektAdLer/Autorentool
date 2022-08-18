using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities.Files.xml;


[XmlRoot(ElementName = "files")]
public class FilesXmlFiles : IFilesXmlFiles
{

    public FilesXmlFiles()
    {
        File = new List<FilesXmlFile>();
    }
    
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "files.xml");
    }
    
    [XmlElement(ElementName="file")]
    public List<FilesXmlFile> File { get; set; }
    
}
