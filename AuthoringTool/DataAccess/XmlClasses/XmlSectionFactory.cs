using AuthoringTool.DataAccess.XmlClasses.sections;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlSectionFactory
{
    public void writeSectionXml()
    {
        //write section.xml file
        var sectionSection = new SectionsSectionXmlSection("160","1");
        
        sectionSection.Serialize();
    }
}