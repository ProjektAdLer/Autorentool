using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities.Scales.xml;

[XmlRoot(ElementName="scales_definition")]
public class ScalesXmlScalesDefinition : IScalesXmlScalesDefinition {
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "scales.xml");
    }
    
}