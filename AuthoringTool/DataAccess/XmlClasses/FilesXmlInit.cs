using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

public class FilesXmlInit : IXMLInit
{ 
    public void XmlInit()
        {
            var file = new FilesXmlFiles();
            
            var xml = new XmlSer();
            xml.serialize(file, "files.xml");
            
        }
        
}