//First Attempts to add a Section into the Moodle Backup Structure

using System.IO.Abstractions;
using Generator.ATF;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;

namespace Generator.XmlClasses;

public class XmlSectionFactory : IXmlSectionFactory
{
    private IFileSystem _fileSystem;
    public string CurrentTime;
    public List<ILearningSpaceJson> LearningSpaceJsons;
    public IReadAtf ReadAtf;
    public ISectionsInforefXmlInforef SectionsInforefXmlInforef;

    public ISectionsSectionXmlSection SectionsSectionXmlSection;

    public XmlSectionFactory(IReadAtf readAtf, IFileSystem? fileSystem = null,
        ISectionsSectionXmlSection? section = null, ISectionsInforefXmlInforef? inforef = null)
    {
        ReadAtf = readAtf;
        _fileSystem = fileSystem ?? new FileSystem();
        SectionsSectionXmlSection = section ?? new SectionsSectionXmlSection();
        SectionsInforefXmlInforef = inforef ?? new SectionsInforefXmlInforef();
        LearningSpaceJsons = new List<ILearningSpaceJson>();
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public void CreateSectionFactory()
    {
        LearningSpaceJsons = ReadAtf.GetSpaceList();

        AddSectionForWorldAttributes();

        AddSectionsForLearningSpaces(LearningSpaceJsons);

        if (ReadAtf.GetBaseLearningElementsList().Count > 0) AddSectionForBaseLearningElements();
    }

    private void AddSectionForWorldAttributes()
    {
        CreateSectionsFolder("0");
        CreateSectionInforefXml("0");
        CreateSectionSectionXml("0", "", "", new List<int?>(),
            "", null, -1);
    }

    private void AddSectionsForLearningSpaces(List<ILearningSpaceJson> spaces)
    {
        foreach (var space in spaces)
        {
            CreateSectionsFolder(space.SpaceId.ToString());
            CreateSectionInforefXml(space.SpaceId.ToString());
            CreateSectionSectionXml(space.SpaceId.ToString(), space.SpaceName, space.SpaceDescription,
                space.SpaceSlotContents, space.SpaceUUID, space.RequiredSpacesToEnter, space.RequiredPointsToComplete);
        }
    }

    private void AddSectionForBaseLearningElements()
    {
        var baseLearningElements = ReadAtf.GetBaseLearningElementsList();
        var baseLearningElementIds = baseLearningElements.Select(baseLearningElement => baseLearningElement.ElementId)
            .Select(dummy => (int?)dummy).ToList();

        var sectionId = (LearningSpaceJsons.Count + 1).ToString();

        //Add a section for BaseLearningElements
        CreateSectionsFolder(sectionId);
        CreateSectionInforefXml(sectionId);
        CreateSectionSectionXml(sectionId, "Hinweise auf externe Lerninhalte",
            "", baseLearningElementIds, "", null, -1);
    }

    private void CreateSectionInforefXml(string sectionId)
    {
        SectionsInforefXmlInforef.Serialize("", sectionId);
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
        else
        {
            SectionsSectionXmlSection.AvailabilityJson = "$@NULL@$";
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