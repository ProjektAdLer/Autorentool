using System.IO.Abstractions;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.Entities.sections;

namespace AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;

/// <summary>
/// Creates all H5P files in the needed XML File.
/// </summary>
public class XmlH5PFactory
{
    private string? currWorkDir;
    private string hardcodedPath = "XMLFilesForExport";
    private string? h5pElementId;
    private string? h5pElementName;
    private string? currentTime;
    private List<FilesXmlFile>? filesXmlFilesList;
    private List<ActivitiesInforefXmlFile>? ActivitiesInforefXmlFileList;

    internal XmlFileManager _fileManager;
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
    

    public XmlH5PFactory(IReadDSL readDsl, XmlFileManager xmlFileManager, IFileSystem? fileSystem = null,
        IFilesXmlFiles? filesXmlFiles = null,
        IFilesXmlFile? filesXmlFile = null, IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null, ActivitiesGradesXmlActivityGradebook? gradebook = null,
        IActivitiesH5PActivityXmlActivity? h5PActivityXmlActivity = null,
        IActivitiesH5PActivityXmlH5PActivity? h5PActivityXmlH5PActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null, IActivitiesInforefXmlFile? inforefXmlFile = null,
        IActivitiesInforefXmlFileref? inforefXmlFileref = null,
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null,
        IActivitiesInforefXmlGradeItemref? inforefXmlGradeItemref = null,
        IActivitiesInforefXmlInforef? inforefXmlInforef = null,
        ISectionsInforefXmlInforef? sectionsInforefXmlInforef = null,
        ISectionsSectionXmlSection? sectionsSectionXmlSection = null)
    {
        _fileSystem = fileSystem?? new FileSystem();
        _fileManager = xmlFileManager;
        
        FilesXmlFileBlock1 = filesXmlFile?? new FilesXmlFile();
        FilesXmlFileBlock2 = filesXmlFile?? new FilesXmlFile();
        
        FilesXmlFiles = filesXmlFiles?? new FilesXmlFiles();

        ActivitiesGradesXmlGradeItem = gradesGradeItem?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlGradeItems = gradesGradeItems?? new ActivitiesGradesXmlGradeItems();
        ActivitiesGradesXmlActivityGradebook = gradebook?? new ActivitiesGradesXmlActivityGradebook();

        ActivitiesH5PActivityXmlActivity =h5PActivityXmlActivity?? new ActivitiesH5PActivityXmlActivity();
        ActivitiesH5PActivityXmlH5PActivity = h5PActivityXmlH5PActivity?? new ActivitiesH5PActivityXmlH5PActivity();

        ActivitiesRolesXmlRoles = roles?? new ActivitiesRolesXmlRoles();

        ActivitiesModuleXmlModule = module?? new ActivitiesModuleXmlModule();

        ActivitiesGradeHistoryXmlGradeHistory = gradeHistory?? new ActivitiesGradeHistoryXmlGradeHistory();

        ActivitiesInforefXmlFileBlock1 = inforefXmlFile?? new ActivitiesInforefXmlFile();
        ActivitiesInforefXmlFileBlock2 = inforefXmlFile?? new ActivitiesInforefXmlFile();
        ActivitiesInforefXmlFileref = inforefXmlFileref?? new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlGradeItem = inforefXmlGradeItem?? new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlGradeItemref = inforefXmlGradeItemref?? new ActivitiesInforefXmlGradeItemref();
        ActivitiesInforefXmlInforef = inforefXmlInforef?? new ActivitiesInforefXmlInforef();

        SectionsInforefXmlInforef = sectionsInforefXmlInforef?? new SectionsInforefXmlInforef();
        SectionsSectionXmlSection = sectionsSectionXmlSection?? new SectionsSectionXmlSection();

        ReadDsl = readDsl;
        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
    }
    
    
    /// <summary>
    /// Create H5P structure in files.xml, folder activity and folder sections for every H5P element in the DSL Document
    /// </summary>
    public void CreateH5PFileFactory()
    {

        if (ReadDsl != null)
        {   // Get all the H5P elements that are in the DSL Document
            List<LearningElementJson>? h5pElementsList = ReadDsl.GetH5PElementsList();
            filesXmlFilesList = new List<FilesXmlFile>();
            filesXmlFilesList = _fileManager.GetXmlFilesList();
            
            // For Each H5P element in the list 
            // (for files.xml) set the H5Pelement id, name, hashvalue, copy the File to the needed location in the backup structure
            // and all the other paramters in (H5PSetParametersFilesXml)
            // Create folder activities and folder Sections/section... 
            if (h5pElementsList != null)
                foreach (var h5pElement in h5pElementsList)
                {
                    h5pElementId = h5pElement.id.ToString();
                    if (h5pElement.identifier != null) h5pElementName = h5pElement.identifier.value;
                    _fileManager.CalculateHashCheckSumAndFileSize(Path.Join(currWorkDir, hardcodedPath, h5pElement.identifier.value));
                    _fileManager.CreateFolderAndFiles(Path.Join(currWorkDir, hardcodedPath, h5pElement.identifier.value), _fileManager.fileCheckSum);
                    H5PSetParametersFilesXml(_fileManager.fileCheckSum, _fileManager.fileSize);
                    H5PSetParametersActivity();
                    H5PSetParametersSections();
                    
                    // These ints are needed for the activities/inforef.xml file. 
                    XmlEntityManager.IncreaseFileId();
                }
        }
        
        FilesXmlFiles.File = filesXmlFilesList;
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
            filesXmlFilesList[filesXmlFilesList.Count - 1].Id = XmlEntityManager.GetFileIdBlock1().ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContentHash = hashCheckSum;
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContextId = h5pElementId;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filename = h5pElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Source = h5pElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filesize = filesize;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Component = "mod_h5pactivity";
            filesXmlFilesList[filesXmlFilesList.Count - 1].FileArea = "package";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Mimetype = "application/zip.h5p";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timecreated = currentTime;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timemodified = currentTime;
            
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].Id = XmlEntityManager.GetFileIdBlock2().ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContentHash = hashCheckSum;
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContextId = h5pElementId;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filename = h5pElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Source = h5pElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filesize = filesize;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Component = "mod_h5pactivity";
            filesXmlFilesList[filesXmlFilesList.Count - 1].FileArea = "package";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Mimetype = "application/zip.h5p";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timecreated = currentTime;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timemodified = currentTime;
        }
        
    }

    /// <summary>
    /// Create Folder Activity and the needed Activity Files
    /// </summary>
    public void H5PSetParametersActivity()
    {
        CreateActivityFolder(h5pElementId);
        
        //file activities/h5p.../grades.xml
        ActivitiesGradesXmlGradeItem.CategoryId = h5pElementId;
        ActivitiesGradesXmlGradeItem.ItemName = h5pElementName;
        ActivitiesGradesXmlGradeItem.ItemType = "mod";
        ActivitiesGradesXmlGradeItem.ItemModule = "h5pactivity";
        ActivitiesGradesXmlGradeItem.Timecreated = currentTime;
        ActivitiesGradesXmlGradeItem.Timemodified = currentTime;
        ActivitiesGradesXmlGradeItem.Id = h5pElementId;
        
        ActivitiesGradesXmlGradeItems.GradeItem = (ActivitiesGradesXmlGradeItem) ActivitiesGradesXmlGradeItem;
        ActivitiesGradesXmlActivityGradebook.GradeItems = (ActivitiesGradesXmlGradeItems) ActivitiesGradesXmlGradeItems;

        ActivitiesGradesXmlActivityGradebook.Serialize("h5pactivity", h5pElementId);
        
        //file activities/h5p.../h5pactivity.xml
        ActivitiesH5PActivityXmlH5PActivity.Name = h5pElementName;
        ActivitiesH5PActivityXmlH5PActivity.Timecreated = currentTime;
        ActivitiesH5PActivityXmlH5PActivity.Timemodified = currentTime;
        ActivitiesH5PActivityXmlH5PActivity.Id = h5pElementId;

        ActivitiesH5PActivityXmlActivity.H5pactivity = (ActivitiesH5PActivityXmlH5PActivity) ActivitiesH5PActivityXmlH5PActivity;
        ActivitiesH5PActivityXmlActivity.Id = h5pElementId;
        ActivitiesH5PActivityXmlActivity.ModuleId = h5pElementId;
        ActivitiesH5PActivityXmlActivity.ModuleName = "h5pactivity";
        ActivitiesH5PActivityXmlActivity.ContextId = h5pElementId;

        ActivitiesH5PActivityXmlActivity.Serialize("h5pactivity", h5pElementId);
        
        //file activities/h5p.../roles.xml
        ActivitiesRolesXmlRoles.RoleOverrides = "";
        ActivitiesRolesXmlRoles.RoleAssignments = "";
 
        ActivitiesRolesXmlRoles.Serialize("h5pactivity", h5pElementId);
        
        //file activities/h5p.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "h5pactivity";
        ActivitiesModuleXmlModule.SectionId = h5pElementId;
        ActivitiesModuleXmlModule.SectionNumber = h5pElementId;
        ActivitiesModuleXmlModule.IdNumber = "";
        ActivitiesModuleXmlModule.Added = currentTime;
        ActivitiesModuleXmlModule.ShowDescription = "0";
        ActivitiesModuleXmlModule.Id = h5pElementId;
        
        ActivitiesModuleXmlModule.Serialize("h5pactivity", h5pElementId);
        
        //file activities/h5p.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("h5pactivity", h5pElementId);
        
        //file activities/h5p.../inforef.xml
        ActivitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();
        if (ActivitiesInforefXmlFileList != null)
        {
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1].Id = XmlEntityManager.GetFileIdBlock1().ToString();
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1].Id = XmlEntityManager.GetFileIdBlock1().ToString();

            ActivitiesInforefXmlFileref.File = ActivitiesInforefXmlFileList;
        }
        
        ActivitiesInforefXmlGradeItemref.GradeItem = (ActivitiesInforefXmlGradeItem) ActivitiesInforefXmlGradeItem;
        
        ActivitiesInforefXmlInforef.Fileref = (ActivitiesInforefXmlFileref) ActivitiesInforefXmlFileref;
        ActivitiesInforefXmlInforef.GradeItemref = (ActivitiesInforefXmlGradeItemref) ActivitiesInforefXmlGradeItemref;
        
        ActivitiesInforefXmlInforef.Serialize("h5pactivity", h5pElementId);
    }

    /// <summary>
    /// Create Folder section/ in the folder sections. And both files inforef.xml and section.xml
    /// </summary>
    public void H5PSetParametersSections()
    {
        CreateSectionsFolder(h5pElementId);
        
        //file sections/section.../inforef.xml
        SectionsInforefXmlInforef.Serialize("", h5pElementId);
        
        //file sections/section.../section.xml
        SectionsSectionXmlSection.Number = h5pElementId;
        SectionsSectionXmlSection.Name = "$@NULL@$";
        SectionsSectionXmlSection.Summary = "$@NULL@$";
        SectionsSectionXmlSection.Timemodified = currentTime;
        SectionsSectionXmlSection.Id = h5pElementId;
        
        SectionsSectionXmlSection.Serialize("", h5pElementId);
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