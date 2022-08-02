using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Scales.xml;

[XmlRoot(ElementName="scales_definition")]
public class ScalesXmlScalesDefinition : IScalesXmlScalesDefinition {
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "scales.xml");
    }
    
}