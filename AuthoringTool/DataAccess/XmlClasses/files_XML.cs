using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses

{
    [XmlRoot(ElementName="files")]
    public class FilesXmlFiles
    {
        
    }

    public class FilesXmlInit
    {
        public FilesXmlFiles Init()
        {
            var file = new FilesXmlFiles();
            
            var xml = new XmlSer();
            xml.serialize(file, "files.xml");
            
            return file;
        }
        
    }

}