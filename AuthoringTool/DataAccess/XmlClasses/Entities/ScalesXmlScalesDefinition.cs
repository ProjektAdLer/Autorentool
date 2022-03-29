using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="scales_definition")]
public partial class ScalesXmlScalesDefinition : IXmlSerializable {

    
    public void SetParameters()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "scales.xml");
    }
    
}