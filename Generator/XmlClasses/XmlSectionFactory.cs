//First Attempts to add a Section into the Moodle Backup Structure

using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;

namespace Generator.XmlClasses;

public class XmlSectionFactory
{

    public ISectionsSectionXmlSection SectionsSectionXmlSection;
    public ISectionsInforefXmlInforef SectionsInforefXmlInforef;
    public string CurrentTime;
    
    public XmlSectionFactory(ISectionsSectionXmlSection? section = null, ISectionsInforefXmlInforef? inforef = null)
    {
        SectionsSectionXmlSection = section ?? new SectionsSectionXmlSection();
        SectionsInforefXmlInforef = inforef ?? new SectionsInforefXmlInforef();
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public void CreateSectionFactory(string sectionId, string sectionName, string? sectionSummary)
    {
        CreateSectionInforefXml();
        CreateSectionSectionXml( sectionId,  sectionName, sectionSummary);
    }
    
    public void CreateSectionInforefXml()
    {
        SectionsInforefXmlInforef.Serialize("","");
    }

    public void CreateSectionSectionXml(string sectionId, string sectionName, string? sectionSummary)
    {
        //write section.xml file
        SectionsSectionXmlSection.Id = sectionId;
        SectionsSectionXmlSection.Number = sectionId;
        SectionsSectionXmlSection.Name = sectionName;
        SectionsSectionXmlSection.Summary = sectionSummary ?? "";
        SectionsSectionXmlSection.Timemodified = CurrentTime;

        SectionsSectionXmlSection.Serialize("","");
    }
}