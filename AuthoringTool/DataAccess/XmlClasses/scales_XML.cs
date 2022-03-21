using System.Xml.Serialization;

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
            return scale;
        }
    }

}