using AuthoringTool.DataAccess.XmlClasses.sections;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlSectionFactory
{
    public XmlSectionFactory()
    {
        //write section.xml file
        var sectionSection = new SectionsSectionXmlSection();
        sectionSection.SetParameters("160","1");
        
        sectionSection.Serialize();
    }
}