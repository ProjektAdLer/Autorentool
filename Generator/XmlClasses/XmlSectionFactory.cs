//First Attempts to add a Section into the Moodle Backup Structure

/*using AuthoringTool.DataAccess.XmlClasses.sections;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlSectionFactory
{

    internal ISectionsSectionXmlSection SectionsSectionXmlSection;
    public XmlSectionFactory()
    {
        SectionsSectionXmlSection = new SectionsSectionXmlSection();
    }

    public XmlSectionFactory(ISectionsSectionXmlSection sectionsSectionXmlSection)
    {
        SectionsSectionXmlSection = sectionsSectionXmlSection;
    }

    public void CreateXmlSectionFactory()
    {
        CreateSectionSectionXml();
    }

    public void CreateSectionSectionXml()
    {
        //write section.xml file
        SectionsSectionXmlSection.SetParameters("160","1");
        
        SectionsSectionXmlSection.Serialize();
    }
}*/