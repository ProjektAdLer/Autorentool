//First Attempts to add a Section into the Moodle Backup Structure

using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;

namespace Generator.XmlClasses;

public class XmlSectionFactory : IXmlSectionFactory
{
    private IFileSystem _fileSystem;
    public string CurrentTime;
    public List<ILearningSpaceJson> LearningSpaceJsons;
    public IReadDsl ReadDsl;
    public ISectionsInforefXmlInforef SectionsInforefXmlInforef;

    public ISectionsSectionXmlSection SectionsSectionXmlSection;

    public XmlSectionFactory(IReadDsl readDsl, IFileSystem? fileSystem = null,
        ISectionsSectionXmlSection? section = null, ISectionsInforefXmlInforef? inforef = null)
    {
        ReadDsl = readDsl;
        _fileSystem = fileSystem ?? new FileSystem();
        SectionsSectionXmlSection = section ?? new SectionsSectionXmlSection();
        SectionsInforefXmlInforef = inforef ?? new SectionsInforefXmlInforef();
        LearningSpaceJsons = new List<ILearningSpaceJson>();
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public void CreateSectionFactory()
    {
        LearningSpaceJsons = ReadDsl.GetSectionList();

        //Add A Section for every LearningSpace
        foreach (var space in LearningSpaceJsons)
        {
            CreateSectionsFolder(space.SpaceId.ToString());
            CreateSectionInforefXml(space.SpaceId.ToString());
            CreateSectionSectionXml(space.SpaceId.ToString(), space.SpaceName, space.SpaceDescription,
                space.SpaceSlotContents, space.SpaceUUID, space.RequiredSpacesToEnter, space.RequiredPointsToComplete);
        }
    }

    private void CreateSectionInforefXml(string sectionid)
    {
        SectionsInforefXmlInforef.Serialize("", sectionid);
    }

    private void CreateSectionSectionXml(string sectionId, string sectionName, string? sectionSummary,
        List<int?> sectionSequence, string spaceUuid, string? requiredSpacesToEnter, int requiredPointsToComplete)
    {
        //write section.xml file
        SectionsSectionXmlSection.Id = sectionId;
        SectionsSectionXmlSection.Number = sectionId;
        SectionsSectionXmlSection.Name = sectionName;
        SectionsSectionXmlSection.Summary = sectionSummary ?? "";
        SectionsSectionXmlSection.Sequence = string.Join(",", sectionSequence.Where(i => i.HasValue));
        if (!string.IsNullOrEmpty(requiredSpacesToEnter))
        {
            SectionsSectionXmlSection.AvailabilityJson =
                "{\"op\":\"&\",\"c\":[{\"type\":\"adler\",\"condition\":\"" + requiredSpacesToEnter +
                "\"}],\"showc\":[true]}";
        }

        if (requiredPointsToComplete >= 0)
        {
            //AdlerSection can not be null at this point because it is set in the constructor
            SectionsSectionXmlSection.PluginLocalAdlerSection.AdlerSection!.RequiredPointsToComplete =
                requiredPointsToComplete.ToString();
            SectionsSectionXmlSection.PluginLocalAdlerSection.AdlerSection!.Uuid = spaceUuid;
        }
        else
        {
            SectionsSectionXmlSection.PluginLocalAdlerSection.AdlerSection = null;
        }

        SectionsSectionXmlSection.Timemodified = CurrentTime;

        SectionsSectionXmlSection.Serialize("", sectionId);

        SectionsSectionXmlSection.PluginLocalAdlerSection.AdlerSection = new SectionsSectionXmlAdlerSection();
    }

    /// <summary>
    /// Creates section folders in the sections folder. For every sectionId.
    /// </summary>
    /// <param name="sectionId"></param>
    private void CreateSectionsFolder(string sectionId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections",
            "section_" + sectionId));
    }
}