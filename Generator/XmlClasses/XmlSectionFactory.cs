//First Attempts to add a Section into the Moodle Backup Structure

using Generator.DSL;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;

namespace Generator.XmlClasses;

public class XmlSectionFactory
{

    public ISectionsSectionXmlSection SectionsSectionXmlSection;
    public ISectionsInforefXmlInforef SectionsInforefXmlInforef;
    public IReadDsl ReadDsl;
    public string CurrentTime;
    
    public XmlSectionFactory(IReadDsl readDsl, ISectionsSectionXmlSection? section = null, ISectionsInforefXmlInforef? inforef = null)
    {
        ReadDsl = readDsl;
        SectionsSectionXmlSection = section ?? new SectionsSectionXmlSection();
        SectionsInforefXmlInforef = inforef ?? new SectionsInforefXmlInforef();
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public void CreateSectionFactory()
    {
        var learningSpaces = ReadDsl.GetLearningSpaceList();
        foreach (var space in learningSpaces)
        {
            CreateSectionInforefXml( space.SpaceId.ToString() );
            CreateSectionSectionXml( space.SpaceId.ToString(),  space.LearningSpaceName, "Summary will follow");
        }

    }
    
    public void CreateSectionInforefXml(string sectionid)
    {
        SectionsInforefXmlInforef.Serialize("", sectionid);
    }

    public void CreateSectionSectionXml(string sectionId, string sectionName, string? sectionSummary)
    {
        //write section.xml file
        SectionsSectionXmlSection.Id = sectionId;
        SectionsSectionXmlSection.Number = sectionId;
        SectionsSectionXmlSection.Name = sectionName;
        SectionsSectionXmlSection.Summary = sectionSummary ?? "";
        SectionsSectionXmlSection.Timemodified = CurrentTime;

        SectionsSectionXmlSection.Serialize("",sectionId);
    }
}