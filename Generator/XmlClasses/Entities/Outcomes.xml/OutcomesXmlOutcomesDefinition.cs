using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities.Outcomes.xml;

[XmlRoot(ElementName="outcomes_definition")]
public class OutcomesXmlOutcomesDefinition : IOutcomesXmlOutcomesDefinition
{
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "outcomes.xml");
    }
}