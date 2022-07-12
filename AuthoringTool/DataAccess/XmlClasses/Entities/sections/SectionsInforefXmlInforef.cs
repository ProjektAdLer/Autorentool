using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.sections;


[XmlRoot(ElementName="inforef")]
public class SectionsInforefXmlInforef : ISectionsInforefXmlInforef{

    public void SetParameters()
    {
        
    }
    
    public void Serialize(string?  name, string? sectionId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("sections", "section_"+sectionId, "inforef.xml"));
    }
}