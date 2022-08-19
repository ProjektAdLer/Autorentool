//First Attempts to add a Section into the Moodle Backup Structure

/*using AuthoringToolLib.DataAccess.XmlClasses.sections;

namespace AuthoringToolLib.DataAccess.XmlClasses;

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