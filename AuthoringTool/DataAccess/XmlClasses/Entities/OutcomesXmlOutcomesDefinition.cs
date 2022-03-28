using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="outcomes_definition")]
public partial class OutcomesXmlOutcomesDefinition : IXmlSerializable {


    public void SetParameters()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "outcomes.xml");
    }
}