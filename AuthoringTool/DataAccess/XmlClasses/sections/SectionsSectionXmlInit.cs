using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.sections;

public class SectionsSectionXmlInit : IXMLInit
{
    public void XmlInit()
    {
            var currTime = DateTimeOffset.Now.ToUnixTimeSeconds(); 
            var sectionsSection = new SectionsSectionXmlSection();
            sectionsSection.Id = "160";
            sectionsSection.Number = "1";
            sectionsSection.Timemodified = currTime.ToString();
            
            var xml = new XmlSer();
            xml.serialize(sectionsSection, "sections/section_160/section.xml");

    }
}
    