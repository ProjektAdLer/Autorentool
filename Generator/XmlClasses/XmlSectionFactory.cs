//First Attempts to add a Section into the Moodle Backup Structure

using System.IO.Abstractions;
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
    private IFileSystem _fileSystem;

    public XmlSectionFactory(IReadDsl readDsl, IFileSystem? fileSystem = null, ISectionsSectionXmlSection? section = null, ISectionsInforefXmlInforef? inforef = null)
    {
        ReadDsl = readDsl;
        _fileSystem = fileSystem ?? new FileSystem();
        SectionsSectionXmlSection = section ?? new SectionsSectionXmlSection();
        SectionsInforefXmlInforef = inforef ?? new SectionsInforefXmlInforef();
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public void CreateSectionFactory()
    {
        var learningSpaces = ReadDsl.GetLearningSpaceList();
        
        //Add A Section for every LearningSpace
        foreach (var space in learningSpaces)
        {
            CreateSectionsFolder(space.SpaceId.ToString());
            CreateSectionInforefXml( space.SpaceId.ToString() );
            CreateSectionSectionXml( space.SpaceId.ToString(),  space.LearningSpaceName, "Summary will follow");
        }

    }

    //Create a Section for every Element added to a LearningWorld and not to a LearningSpace
    /*public void CreateLearningWorldSection()
    {
        CreateSectionsFolder("0");
        CreateSectionInforefXml( "0" );
        CreateSectionSectionXml( "0",  "Freie Lernelemente", "Diese Lernelemente sind keinem Lernraum zugeordnet");
    }*/
    
    private void CreateSectionInforefXml(string sectionid)
    {
        SectionsInforefXmlInforef.Serialize("", sectionid);
    }

    private void CreateSectionSectionXml(string sectionId, string sectionName, string? sectionSummary)
    {
        //write section.xml file
        SectionsSectionXmlSection.Id = sectionId;
        SectionsSectionXmlSection.Number = sectionId;
        SectionsSectionXmlSection.Name = sectionName;
        SectionsSectionXmlSection.Summary = sectionSummary ?? "";
        SectionsSectionXmlSection.Timemodified = CurrentTime;

        SectionsSectionXmlSection.Serialize("",sectionId);
    }
    
    /// <summary>
    /// Creates section folders in the sections folder. For every sectionId.
    /// </summary>
    /// <param name="sectionId"></param>
    public void CreateSectionsFolder(string sectionId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_"+sectionId));
    }
}