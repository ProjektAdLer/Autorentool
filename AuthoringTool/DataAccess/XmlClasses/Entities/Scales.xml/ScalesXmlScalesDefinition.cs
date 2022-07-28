using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="scales_definition")]
public partial class ScalesXmlScalesDefinition : IScalesXmlScalesDefinition {

    public ScalesXmlScalesDefinition()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "scales.xml");
    }
    
}