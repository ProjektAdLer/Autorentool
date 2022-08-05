using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Outcomes.xml;

[XmlRoot(ElementName="outcomes_definition")]
public class OutcomesXmlOutcomesDefinition : IOutcomesXmlOutcomesDefinition
{
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "outcomes.xml");
    }
}