using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.sections;


[XmlRoot(ElementName="inforef")]
public class SectionsInforefXmlInforef : ISectionsInforefXmlInforef{

    public void SetParameters()
    {
        
    }
    
    public void Serialize(string? sectionId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "sections/section_"+sectionId+"/inforef.xml");
    }
}