using System.Xml.Serialization;

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
            return file;
        }
        
    }

}