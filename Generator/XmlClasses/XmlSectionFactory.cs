//First Attempts to add a Section into the Moodle Backup Structure

using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;

namespace Generator.XmlClasses;

public class XmlSectionFactory : IXmlSectionFactory
{

    public ISectionsSectionXmlSection SectionsSectionXmlSection;
    public ISectionsInforefXmlInforef SectionsInforefXmlInforef;
    public IReadDsl ReadDsl;
    public string CurrentTime;
    private IFileSystem _fileSystem;
    public List<SpaceJson> SpaceJsons;

    public XmlSectionFactory(IReadDsl readDsl, IFileSystem? fileSystem = null, ISectionsSectionXmlSection? section = null, ISectionsInforefXmlInforef? inforef = null)
    {
        ReadDsl = readDsl;
        _fileSystem = fileSystem ?? new FileSystem();
        SectionsSectionXmlSection = section ?? new SectionsSectionXmlSection();
        SectionsInforefXmlInforef = inforef ?? new SectionsInforefXmlInforef();
        SpaceJsons = new List<SpaceJson>();
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public void CreateSectionFactory()
    {
        SpaceJsons = ReadDsl.GetSectionList();
        
        //Add A Section for every Space
        foreach (var space in SpaceJsons)
        {
            CreateSectionsFolder(space.SpaceId.ToString());
            CreateSectionInforefXml( space.SpaceId.ToString() );
            CreateSectionSectionXml( space.SpaceId.ToString(),  space.Identifier.Value, "");
        }

    }
    
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
    private void CreateSectionsFolder(string sectionId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_"+sectionId));
    }
}