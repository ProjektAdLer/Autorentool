using System.Diagnostics;
using System.IO.Abstractions;
using System.Security.Cryptography;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.sections;

namespace AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;

/// <summary>
/// Creates all H5P files in the needed XML File.
/// </summary>
public class XmlH5PFactory
{
    private string hardcordedPath = Path.Join("C:", "Users", "biglerdd", "Desktop", "HS", "Hagel-Lernelemente", "Wortsuche Metriken.h5p");
    private string? h5pElementId;
    private string? h5pElementName;
    private string? currentTime;
    private List<FilesXmlFile>? filesXmlFilesList;
    private List<ActivitiesInforefXmlFile>? ActivitiesInforefXmlFileList;

    internal IFilesXmlFiles FilesXmlFiles { get; }
    internal IFilesXmlFile FilesXmlFileBlock1 { get; }
    internal IFilesXmlFile FilesXmlFileBlock2 { get; }
    internal IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    internal IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    internal IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    internal IActivitiesH5PActivityXmlActivity ActivitiesH5PActivityXmlActivity { get; }
    internal IActivitiesH5PActivityXmlH5PActivity ActivitiesH5PActivityXmlH5PActivity { get; }
    internal IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    internal IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    internal IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    internal IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock1 { get; }
    internal IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock2 { get; }
    internal IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    internal IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    internal IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }
    internal IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    internal ISectionsInforefXmlInforef SectionsInforefXmlInforef { get; }
    internal ISectionsSectionXmlSection SectionsSectionXmlSection { get; }
    internal IReadDSL? ReadDsl { get; }

    private IFileSystem _fileSystem;

    
    public XmlH5PFactory(ReadDSL? readDsl)
    {
        FilesXmlFileBlock1 = new FilesXmlFile();
        FilesXmlFileBlock2 = new FilesXmlFile();

        ActivitiesGradesXmlGradeItem = new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlGradeItems = new ActivitiesGradesXmlGradeItems();
        ActivitiesGradesXmlActivityGradebook = new ActivitiesGradesXmlActivityGradebook();

        ActivitiesH5PActivityXmlActivity = new ActivitiesH5PActivityXmlActivity();
        ActivitiesH5PActivityXmlH5PActivity = new ActivitiesH5PActivityXmlH5PActivity();

        ActivitiesRolesXmlRoles = new ActivitiesRolesXmlRoles();
        
        FilesXmlFiles = new FilesXmlFiles();

        ActivitiesModuleXmlModule = new ActivitiesModuleXmlModule();

        ActivitiesGradeHistoryXmlGradeHistory = new ActivitiesGradeHistoryXmlGradeHistory();

        ActivitiesInforefXmlFileBlock1 = new ActivitiesInforefXmlFile();
        ActivitiesInforefXmlFileBlock2 = new ActivitiesInforefXmlFile();
        ActivitiesInforefXmlFileref = new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlGradeItem = new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlGradeItemref = new ActivitiesInforefXmlGradeItemref();
        ActivitiesInforefXmlInforef = new ActivitiesInforefXmlInforef();

        SectionsInforefXmlInforef = new SectionsInforefXmlInforef();
        SectionsSectionXmlSection = new SectionsSectionXmlSection();

        ReadDsl = readDsl;
        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        _fileSystem = new FileSystem();
    }

    public XmlH5PFactory(IFileSystem fileSystem, IReadDSL? readDsl , IFilesXmlFiles filesXmlFiles, IFilesXmlFile filesXmlFile,
        IActivitiesGradesXmlGradeItem gradesGradeItem, IActivitiesGradesXmlGradeItems gradesGradeItems, 
        ActivitiesGradesXmlActivityGradebook gradebook, IActivitiesH5PActivityXmlActivity h5PActivityXmlActivity, 
        IActivitiesH5PActivityXmlH5PActivity h5PActivityXmlH5PActivity, IActivitiesRolesXmlRoles roles, IActivitiesModuleXmlModule module,
        IActivitiesGradeHistoryXmlGradeHistory gradeHistory, IActivitiesInforefXmlFile inforefXmlFile, 
        IActivitiesInforefXmlFileref inforefXmlFileref, IActivitiesInforefXmlGradeItem inforefXmlGradeItem, 
        IActivitiesInforefXmlGradeItemref inforefXmlGradeItemref, IActivitiesInforefXmlInforef inforefXmlInforef,
        ISectionsInforefXmlInforef sectionsInforefXmlInforef, ISectionsSectionXmlSection sectionsSectionXmlSection)
    {
        _fileSystem = fileSystem; 
        
        FilesXmlFiles = filesXmlFiles;
        FilesXmlFileBlock1 = filesXmlFile;
        FilesXmlFileBlock2 = filesXmlFile;

        ActivitiesGradesXmlGradeItem = gradesGradeItem;
        ActivitiesGradesXmlGradeItems = gradesGradeItems;
        ActivitiesGradesXmlActivityGradebook = gradebook;

        ActivitiesH5PActivityXmlActivity = h5PActivityXmlActivity;
        ActivitiesH5PActivityXmlH5PActivity = h5PActivityXmlH5PActivity;

        ActivitiesRolesXmlRoles = roles;

        ActivitiesModuleXmlModule = module;

        ActivitiesGradeHistoryXmlGradeHistory = gradeHistory;

        ActivitiesInforefXmlFileBlock1 = inforefXmlFile;
        ActivitiesInforefXmlFileBlock2 = inforefXmlFile;
        ActivitiesInforefXmlFileref = inforefXmlFileref;
        ActivitiesInforefXmlGradeItem = inforefXmlGradeItem;
        ActivitiesInforefXmlGradeItemref = inforefXmlGradeItemref;
        ActivitiesInforefXmlInforef = inforefXmlInforef;

        SectionsInforefXmlInforef = sectionsInforefXmlInforef;
        SectionsSectionXmlSection = sectionsSectionXmlSection;

        ReadDsl = readDsl;

        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }
    
    
    /// <summary>
    /// Create H5P structure in files.xml, folder activity and folder sections for every H5P element in the DSL Document
    /// </summary>
    public void CreateH5PFileFactory()
    {
        XmlFileFactory filefactory = new XmlFileFactory();

        if (ReadDsl != null)
        {   // Get all the H5P elements that are in the DSL Document
            List<LearningElementJson>? h5pElementsList = ReadDsl.GetH5PElementsList();
            filesXmlFilesList = new List<FilesXmlFile>();

            // For Each H5P element in the list 
            // (for files.xml) set the H5Pelement id, name, hashvalue, copy the File to the needed location in the backup structure
            // and all the other paramters in (H5PSetParametersFilesXml)
            // Create folder activities and folder Sections/section... 
            if (h5pElementsList != null)
                foreach (var h5pElement in h5pElementsList)
                {
                    h5pElementId = h5pElement.id.ToString();
                    if (h5pElement.identifier != null) h5pElementName = h5pElement.identifier.value;
                    filefactory.CalculateHashCheckSumAndFileSize(hardcordedPath);
                    filefactory.CreateFolderAndFiles(hardcordedPath, filefactory.fileCheckSum);
                    H5PSetParametersFilesXml(filefactory.fileCheckSum, filefactory.fileSize);
                    H5PSetParametersActivity();
                    H5PSetParametersSections();
                    
                    // These ints are needed for the activities/inforef.xml file. 
                    XmlEntityManager.IncreaseFileId();
                }
        }

        FilesXmlFiles.SetParameters(filesXmlFilesList);
        FilesXmlFiles.Serialize();
    }

    /// <summary>
    /// Setting Parameters for h5p element in files.xml, 
    /// </summary>
    /// <param name="hashCheckSum"></param> SHA1 Hash value for the file
    /// <param name="filesize"></param> Byte Filesize for the file
    public void H5PSetParametersFilesXml(string? hashCheckSum, string? filesize)
    {
        if (filesXmlFilesList != null)
        {
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].SetParameters(hashCheckSum, h5pElementId, "mod_h5pactivity",
                "package",
                "0", h5pElementName, filesize, "application/zip.h5p", "/", currentTime,
                currentTime, "$@NULL@$", "0", XmlEntityManager.GetFileIdBlock1().ToString());
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].SetParameters(hashCheckSum, h5pElementId, "mod_h5pactivity",
                "package",
                "0", h5pElementName, filesize, "application/zip.h5p", "/", currentTime,
                currentTime, "$@NULL@$", "0", XmlEntityManager.GetFileIdBlock2().ToString());
        }
        
    }

    /// <summary>
    /// Create Folder Activity and the needed Activity Files
    /// </summary>
    public void H5PSetParametersActivity()
    {
        CreateActivityFolder(h5pElementId);
        
        //file activities/h5p.../grades.xml
        ActivitiesGradesXmlGradeItem.SetParameters(h5pElementId, h5pElementName,
            "mod", "h5pactivity", "1", "0", "$@NULL@$", "$@NULL@$",
            "$@NULL@$", "1", "100.00000", "0.00000", "$@NULL@$", "$@NULL@$",
            "0.00000", "1.00000", "0.00000", "0.00000", "1.00000",
            "0", "2", "0", "$@NULL@$", "0", "0", "0", "0",
            currentTime, currentTime, "", h5pElementId);
        ActivitiesGradesXmlGradeItems.SetParameters(ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem);
        ActivitiesGradesXmlActivityGradebook.SetParameterts(ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems,
            "");
        ActivitiesGradesXmlActivityGradebook.Serialize(h5pElementId);
        
        //file activities/h5p.../h5pactivity.xml
        ActivitiesH5PActivityXmlH5PActivity.SetParameterts(h5pElementName,
            currentTime, currentTime, "", "1", "100",
            "15", "1", "1", "1", "", h5pElementId);
        ActivitiesH5PActivityXmlActivity.SetParameterts(ActivitiesH5PActivityXmlH5PActivity as ActivitiesH5PActivityXmlH5PActivity, 
            h5pElementId, h5pElementId, "h5pactivity", h5pElementId);
        ActivitiesH5PActivityXmlActivity.Serialize(h5pElementId);
        
        //file activities/h5p.../roles.xml
        ActivitiesRolesXmlRoles.SetParameterts("", "");
        ActivitiesRolesXmlRoles.Serialize(h5pElementId);
        
        //file activities/h5p.../module.xml
        ActivitiesModuleXmlModule.SetParameterts("h5pactivity", h5pElementId, h5pElementId,
            "", currentTime, "0", "0", "1",
            "1", "1", "0", "1",
            "1", "$@NULL@$", "0",
            "0", "$@NULL@$", "0", "", 
            h5pElementId, "2021051700");
        ActivitiesModuleXmlModule.Serialize(h5pElementId);
        
        //file activities/h5p.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.SetParameterts("");
        ActivitiesGradeHistoryXmlGradeHistory.Serialize(h5pElementId);
        
        //file activities/h5p.../inforef.xml
        ActivitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();
        if (ActivitiesInforefXmlFileList != null)
        {
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock1().ToString());
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock2().ToString());

            ActivitiesInforefXmlFileref.SetParameterts(ActivitiesInforefXmlFileList);
        }

        ActivitiesInforefXmlGradeItem.SetParameters("1");
        ActivitiesInforefXmlGradeItemref.SetParameters(ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ?? throw new InvalidOperationException());
        
        ActivitiesInforefXmlInforef.SetParameters(ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref, 
            ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref);
        ActivitiesInforefXmlInforef.Serialize(h5pElementId);
    }

    /// <summary>
    /// Create Folder section/ in the folder sections. And both files inforef.xml and section.xml
    /// </summary>
    public void H5PSetParametersSections()
    {
        CreateSectionsFolder(h5pElementId);
        
        //file sections/section.../inforef.xml
        SectionsInforefXmlInforef.SetParameters();
        SectionsInforefXmlInforef.Serialize(h5pElementId);
        
        //file sections/section.../section.xml
        SectionsSectionXmlSection.SetParameters(h5pElementId, "$@NULL@$",
            "$@NULL@$", "0", "$@NULL@$", "1", 
            "$@NULL@$", currentTime, h5pElementId);
        SectionsSectionXmlSection.Serialize(h5pElementId);
    }

    /// <summary>
    /// Creates a h5p folder in the activity folder. Each activity needs an folder.
    /// </summary>
    /// <param name="moduleId"></param>
    public void CreateActivityFolder(string? moduleId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "h5pactivity_"+moduleId));
    }

    /// <summary>
    /// Creates section folders in the sections folder. For every sectionId.
    /// </summary>
    /// <param name="sectionId"></param>
    public void CreateSectionsFolder(string? sectionId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_"+sectionId));
    }
}