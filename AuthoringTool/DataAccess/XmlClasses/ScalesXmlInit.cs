using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

public class ScalesXmlInit : IXMLInit
{
    public void XmlInit()
    {
        var scale = new ScalesXmlScalesDefinition();

        var xml = new XmlSer();
        xml.serialize(scale, "scales.xml");
        
    }
}