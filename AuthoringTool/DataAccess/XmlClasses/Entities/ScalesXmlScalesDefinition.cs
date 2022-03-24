using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="scales_definition")]
public partial class ScalesXmlScalesDefinition : IXmlSerializable {

    public ScalesXmlScalesDefinition()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "scales.xml");
    }
    
}