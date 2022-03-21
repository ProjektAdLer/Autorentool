using System.Xml.Serialization;

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
            return outcome;
        }
    }

}