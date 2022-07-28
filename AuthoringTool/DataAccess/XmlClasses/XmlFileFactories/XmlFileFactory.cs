using System.IO.Abstractions;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.Entities.sections;

namespace AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;

public class XmlFileFactory
{
    private string dslPath;
    private List<FilesXmlFile>? filesXmlFilesList;
    private List<ActivitiesInforefXmlFile>? ActivitiesInforefXmlFileList;
    private string? currentTime;
    private IFileSystem _fileSystem;
    private string? fileElementId;
    private string? fileElementName;

    internal XmlFileManager _fileManager;
    internal IFilesXmlFiles FilesXmlFiles { get; }
    internal IFilesXmlFile FilesXmlFileBlock1 { get; }
    internal IFilesXmlFile FilesXmlFileBlock2 { get; }
    internal IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    internal IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    internal IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    internal IActivitiesResourceXmlResource ActivitiesFileResourceXmlResource { get; }
    internal IActivitiesResourceXmlActivity ActivitiesFileResourceXmlActivity { get; }
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

    public XmlFileFactory(IReadDSL readDsl, string dslpath, XmlFileManager xmlFileManager,
        IFileSystem? fileSystem = null, IFilesXmlFiles? filesXmlFiles = null,
        IFilesXmlFile? filesXmlFile = null, IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null, ActivitiesGradesXmlActivityGradebook? gradebook = null,
        IActivitiesResourceXmlResource? fileResourceXmlResource = null,
        IActivitiesResourceXmlActivity? fileResourceXmlActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null, IActivitiesInforefXmlFile? inforefXmlFile = null,
        IActivitiesInforefXmlFileref? inforefXmlFileref = null,
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null,
        IActivitiesInforefXmlGradeItemref? inforefXmlGradeItemref = null,
        IActivitiesInforefXmlInforef? inforefXmlInforef = null,
        ISectionsInforefXmlInforef? sectionsInforefXmlInforef = null,
        ISectionsSectionXmlSection? sectionsSectionXmlSection = null)
    {
        dslPath = dslpath;
        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        _fileSystem = fileSystem?? new FileSystem(); 
        ReadDsl = readDsl;
        _fileManager = xmlFileManager;

        _fileSystem = fileSystem?? new FileSystem(); 
        
        FilesXmlFileBlock1 = filesXmlFile?? new FilesXmlFile();
        FilesXmlFileBlock2 = filesXmlFile?? new FilesXmlFile();
        
        FilesXmlFiles = filesXmlFiles?? new FilesXmlFiles();

        ActivitiesGradesXmlGradeItem = gradesGradeItem?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlGradeItems = gradesGradeItems?? new ActivitiesGradesXmlGradeItems();
        ActivitiesGradesXmlActivityGradebook = gradebook?? new ActivitiesGradesXmlActivityGradebook();

        ActivitiesFileResourceXmlResource =fileResourceXmlResource??  new ActivitiesResourceXmlResource();
        ActivitiesFileResourceXmlActivity = fileResourceXmlActivity??  new ActivitiesResourceXmlActivity();

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
    }
    
    public void CreateFileFactory()
    {
        List<LearningElementJson>? listDslDocument = ReadDsl?.GetDslDocumentList();
        filesXmlFilesList = new List<FilesXmlFile>();
        
        if (listDslDocument != null)
        {
            foreach (var dsldocument in listDslDocument)
            {
                fileElementId = dsldocument.id.ToString();
                fileElementName = "DSL_Document";

                _fileManager.CalculateHashCheckSumAndFileSize(dslPath);
                _fileManager.CreateFolderAndFiles(dslPath, _fileManager.fileCheckSum);
                FileSetParametersFilesXml(_fileManager.fileCheckSum, _fileManager.fileSize);
                FileSetParametersActivity();
                FileSetParametersSections();
                
                // These ints are needed for the activities/inforef.xml file. 
                XmlEntityManager.IncreaseFileId();
            }
        }
        _fileManager.SetXmlFilesList(filesXmlFilesList);
    }

    public void FileSetParametersFilesXml(string? hashCheckSum, string? filesize)
    {
        if (filesXmlFilesList != null)
        {
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].Id = XmlEntityManager.GetFileIdBlock1().ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContentHash = hashCheckSum;
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContextId = fileElementId;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filename = fileElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Source = fileElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filesize = filesize;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timecreated = currentTime;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timemodified = currentTime;

            
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].Id = XmlEntityManager.GetFileIdBlock2().ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContentHash = hashCheckSum;
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContextId = fileElementId;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filename = fileElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Source = fileElementName;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filesize = filesize;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timecreated = currentTime;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timemodified = currentTime;
            
        }
    }
    
     public void FileSetParametersActivity()
    {
        CreateActivityFolder(fileElementId);
        
        //file activities/resource.../grades.xml
        ActivitiesGradesXmlGradeItems.GradeItem = (ActivitiesGradesXmlGradeItem) ActivitiesGradesXmlGradeItem;
        ActivitiesGradesXmlActivityGradebook.GradeItems = (ActivitiesGradesXmlGradeItems) ActivitiesGradesXmlGradeItems;

        ActivitiesGradesXmlActivityGradebook.Serialize("resource", fileElementId);
        
        //file activities/resource.../resource.xml
        ActivitiesFileResourceXmlResource.Name = fileElementName;
        ActivitiesFileResourceXmlResource.Timemodified = currentTime; 
        ActivitiesFileResourceXmlResource.Id = fileElementId;

        ActivitiesFileResourceXmlActivity.Resource = (ActivitiesResourceXmlResource) ActivitiesFileResourceXmlResource;
        ActivitiesFileResourceXmlActivity.Id = fileElementId;
        ActivitiesFileResourceXmlActivity.ModuleId = fileElementId;
        ActivitiesFileResourceXmlActivity.ModuleName = "resource";
        ActivitiesFileResourceXmlActivity.ContextId = fileElementId;

        ActivitiesFileResourceXmlActivity.Serialize("resource", fileElementId);
        
        //file activities/resource.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("resource", fileElementId);
        
        //file activities/resource.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "resource";
        ActivitiesModuleXmlModule.SectionId = fileElementId;
        ActivitiesModuleXmlModule.SectionNumber = fileElementId;
        ActivitiesModuleXmlModule.Added = currentTime;
        ActivitiesModuleXmlModule.Id = fileElementId;
        
        ActivitiesModuleXmlModule.Serialize("resource", fileElementId);
        
        //file activities/resource.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("resource", fileElementId);
        
        //file activities/resource.../inforef.xml
        ActivitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();
        if (ActivitiesInforefXmlFileList != null)
        {
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1].Id = XmlEntityManager.GetFileIdBlock1().ToString();
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1].Id = XmlEntityManager.GetFileIdBlock2().ToString();

            ActivitiesInforefXmlFileref.File = ActivitiesInforefXmlFileList;
        }

        ActivitiesInforefXmlGradeItemref.GradeItem = (ActivitiesInforefXmlGradeItem) ActivitiesInforefXmlGradeItem;
        
        ActivitiesInforefXmlInforef.Fileref = (ActivitiesInforefXmlFileref) ActivitiesInforefXmlFileref; 
        ActivitiesInforefXmlInforef.GradeItemref = (ActivitiesInforefXmlGradeItemref) ActivitiesInforefXmlGradeItemref;
        
        ActivitiesInforefXmlInforef.Serialize("resource", fileElementId);
    }
     
     /// <summary>
     /// Create Folder section/ in the folder sections. And both files inforef.xml and section.xml
     /// </summary>
     public void FileSetParametersSections()
     {
         CreateSectionsFolder(fileElementId);
        
         //file sections/section.../inforef.xml
         SectionsInforefXmlInforef.Serialize("",fileElementId);
        
         //file sections/section.../section.xml
         SectionsSectionXmlSection.Number = fileElementId;
         SectionsSectionXmlSection.Name = "$@NULL@$";
         SectionsSectionXmlSection.Summary = "$@NULL@$";
         SectionsSectionXmlSection.Timemodified = currentTime;
         SectionsSectionXmlSection.Id = fileElementId;

         SectionsSectionXmlSection.Serialize("",fileElementId);
     }
     
     
     /// <summary>
     /// Creates a h5p folder in the activity folder. Each activity needs an folder.
     /// </summary>
     /// <param name="moduleId"></param>
     public void CreateActivityFolder(string? moduleId)
     {
         var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
         _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "resource_"+moduleId));
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