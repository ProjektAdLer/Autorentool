﻿//First Attempts to add a Section into the Moodle Backup Structure

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
    public List<LearningSpaceJson> LearningSpaceJsons;

    public XmlSectionFactory(IReadDsl readDsl, IFileSystem? fileSystem = null, ISectionsSectionXmlSection? section = null, ISectionsInforefXmlInforef? inforef = null)
    {
        ReadDsl = readDsl;
        _fileSystem = fileSystem ?? new FileSystem();
        SectionsSectionXmlSection = section ?? new SectionsSectionXmlSection();
        SectionsInforefXmlInforef = inforef ?? new SectionsInforefXmlInforef();
        LearningSpaceJsons = new List<LearningSpaceJson>();
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public void CreateSectionFactory()
    {
        LearningSpaceJsons = ReadDsl.GetSectionList();
        
        //Add A Section for every LearningSpace
        foreach (var space in LearningSpaceJsons)
        {
            CreateSectionsFolder(space.SpaceId.ToString());
            CreateSectionInforefXml( space.SpaceId.ToString() );
            CreateSectionSectionXml( space.SpaceId.ToString(),  space.SpaceName, space.SpaceDescription, space.SpaceContents, space.SpaceId);
        }

    }
    
    private void CreateSectionInforefXml(string sectionid)
    {
        SectionsInforefXmlInforef.Serialize("", sectionid);
    }

    private void CreateSectionSectionXml(string sectionId, string sectionName, string? sectionSummary, List<int> sectionSequence, int spaceId)
    {
        //write section.xml file
        SectionsSectionXmlSection.Id = sectionId;
        SectionsSectionXmlSection.Number = sectionId;
        SectionsSectionXmlSection.Name = sectionName;
        SectionsSectionXmlSection.Summary = sectionSummary ?? "";
        SectionsSectionXmlSection.Sequence = string.Join(",", sectionSequence);
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