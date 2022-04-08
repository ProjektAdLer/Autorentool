using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;


[XmlRoot(ElementName = "files")]
public partial class FilesXmlFiles : IFilesXmlFiles
{
    public void SetParameters()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "files.xml");
    }
    
}
