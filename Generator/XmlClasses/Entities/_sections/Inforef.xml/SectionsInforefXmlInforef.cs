using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._sections.Inforef.xml;


[XmlRoot(ElementName="inforef")]
public class SectionsInforefXmlInforef : ISectionsInforefXmlInforef{
    public void Serialize(string?  name, string? sectionId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("sections", "section_"+sectionId, "inforef.xml"));
    }
}