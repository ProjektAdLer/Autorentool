using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;


namespace AuthoringTool.DataAccess.XmlClasses.Entities;


[XmlRoot(ElementName = "files")]
public partial class FilesXmlFiles : IFilesXmlFiles
{
    public void SetParameters(List<FilesXmlFile>? filelist)
    {
        File = filelist;
    }
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "files.xml");
    }
    
    [XmlElement(ElementName="file")]
    public List<FilesXmlFile>? File { get; set; }
    
}
