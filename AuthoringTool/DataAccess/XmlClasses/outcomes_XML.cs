using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses

{
    [XmlRoot(ElementName="outcomes_definition")]
    public class OutcomesXmlOutcomesDefinition {
        
    }

    public class OutcomesXmlInit
    {
        public OutcomesXmlOutcomesDefinition Init()
        {
            var outcome = new OutcomesXmlOutcomesDefinition();
            
            var xml = new XmlSer();
            xml.serialize(outcome, "outcomes.xml");
            
            return outcome;
        }
    }

}