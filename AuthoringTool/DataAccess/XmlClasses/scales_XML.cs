using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses

{
    [XmlRoot(ElementName="scales_definition")]
    public class ScalesXmlScalesDefinition {
        
    }

    public class ScalesXmlInit
    {
        public ScalesXmlScalesDefinition Init()
        {
            var scale = new ScalesXmlScalesDefinition();
            
            var xml = new XmlSer();
            xml.serialize(scale, "scales.xml");
            
            return scale;
        }
    }

}