using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

public class OutcomesXmlInit : IXMLInit
{
    public void XmlInit() 
    {
            var outcome = new OutcomesXmlOutcomesDefinition();
            
            var xml = new XmlSer();
            xml.serialize(outcome, "outcomes.xml");

    }
}
    