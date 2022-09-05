using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Resource.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;
using Generator.XmlClasses.Entities.Files.xml;

namespace Generator.XmlClasses.XmlFileFactories;

public class XmlFileFactory : IXmlFileFactory
{
    private readonly string _dslPath;
    public List<FilesXmlFile> FilesXmlFilesList;
    private List<ActivitiesInforefXmlFile> _activitiesInforefXmlFileList;
    public readonly string CurrentTime;
    private readonly IFileSystem _fileSystem;
    public string FileElementId;
    public string FileElementName;
    public string FileElementParentSpace;

    public readonly IXmlFileManager FileManager;
    public IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    public IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    public IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    public IActivitiesResourceXmlResource ActivitiesFileResourceXmlResource { get; }
    public IActivitiesResourceXmlActivity ActivitiesFileResourceXmlActivity { get; }
    public IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    public IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    public IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    public IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock1 { get; }
    public IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock2 { get; }
    public IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    public IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    public IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }
    public IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    public ISectionsInforefXmlInforef SectionsInforefXmlInforef { get; }
    public ISectionsSectionXmlSection SectionsSectionXmlSection { get; }
    public IReadDsl ReadDsl { get; }

    public XmlFileFactory(IReadDsl readDsl, string dslPath, IXmlFileManager? xmlFileManager = null,
        IFileSystem? fileSystem = null, IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null, IActivitiesGradesXmlActivityGradebook? gradebook = null,
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
        ReadDsl = readDsl;   
        _dslPath = dslPath;
        FileElementId = "";
        FileElementName = "";
        FileElementParentSpace = "";
        
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        _fileSystem = fileSystem?? new FileSystem();

        FileManager = xmlFileManager?? new XmlFileManager();
        FilesXmlFilesList = new List<FilesXmlFile>();
        _activitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();

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
        var listDslDocument = ReadDsl.GetDslDocumentList();
        FilesXmlFilesList = new List<FilesXmlFile>();
        FilesXmlFilesList = FileManager.GetXmlFilesList();
        
        ReadFileListAndSetParameters(listDslDocument);
        
        FileManager.SetXmlFilesList(FilesXmlFilesList);
    }

    public void ReadFileListAndSetParameters(List<LearningElementJson> listDslDocument)
    {
        foreach (var dslDocument in listDslDocument)
        {
            FileElementId = dslDocument.Id.ToString();
            FileElementName = "DSL_Document";
            FileElementParentSpace = dslDocument.LearningSpaceParentId.ToString();

            FileManager.CalculateHashCheckSumAndFileSize(_dslPath);
            FileManager.CreateFolderAndFiles(_dslPath, FileManager.GetHashCheckSum());
            FileSetParametersFilesXml(FileManager.GetHashCheckSum(), FileManager.GetFileSize());
            FileSetParametersActivity();
            
            // These ints are needed for the activities/inforef.xml file. 
            XmlEntityManager.IncreaseFileId();
        }
    }
    public void FileSetParametersFilesXml(string hashCheckSum, string filesize)
    {
        //Let this Null-Check in, otherwise a Unit-Test will fail
        if(FilesXmlFilesList == null){ FilesXmlFilesList = new List<FilesXmlFile>();}
        var file1 = new FilesXmlFile
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString(),
            ContentHash = hashCheckSum,
            ContextId = FileElementId,
            Filename = FileElementName,
            Source = FileElementName,
            Filesize = filesize,
            Timecreated = CurrentTime,
            Timemodified = CurrentTime,
        };
        var file2 = (FilesXmlFile)file1.Clone();
        file2.Id = XmlEntityManager.GetFileIdBlock2().ToString();
        
        FilesXmlFilesList.Add(file1);
        FilesXmlFilesList.Add(file2);
    }
    
     public void FileSetParametersActivity()
    {
        CreateActivityFolder(FileElementId);
        
        //file activities/resource.../grades.xml
        ActivitiesGradesXmlGradeItems.GradeItem = ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem ?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlActivityGradebook.GradeItems = ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems ?? new ActivitiesGradesXmlGradeItems();

        ActivitiesGradesXmlActivityGradebook.Serialize("resource", FileElementId);
        
        //file activities/resource.../resource.xml
        ActivitiesFileResourceXmlResource.Name = FileElementName;
        ActivitiesFileResourceXmlResource.Timemodified = CurrentTime; 
        ActivitiesFileResourceXmlResource.Id = FileElementId;

        ActivitiesFileResourceXmlActivity.Resource = ActivitiesFileResourceXmlResource as ActivitiesResourceXmlResource ?? new ActivitiesResourceXmlResource();
        ActivitiesFileResourceXmlActivity.Id = FileElementId;
        ActivitiesFileResourceXmlActivity.ModuleId = FileElementId;
        ActivitiesFileResourceXmlActivity.ModuleName = "resource";
        ActivitiesFileResourceXmlActivity.ContextId = FileElementId;

        ActivitiesFileResourceXmlActivity.Serialize("resource", FileElementId);
        
        //file activities/resource.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("resource", FileElementId);
        
        //file activities/resource.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "resource";
        ActivitiesModuleXmlModule.SectionId = FileElementParentSpace;
        ActivitiesModuleXmlModule.SectionNumber = FileElementParentSpace;
        ActivitiesModuleXmlModule.Added = CurrentTime;
        ActivitiesModuleXmlModule.Id = FileElementId;
        
        ActivitiesModuleXmlModule.Serialize("resource", FileElementId);
        
        //file activities/resource.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("resource", FileElementId);
        
        //file activities/resource.../inforef.xml
        _activitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();
        
        var file1 = new ActivitiesInforefXmlFile
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString()
        };
        var file2 = new ActivitiesInforefXmlFile
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString()
        };

        _activitiesInforefXmlFileList.Add(file1);
        _activitiesInforefXmlFileList.Add(file2);
        
        ActivitiesInforefXmlFileref.File = _activitiesInforefXmlFileList;

        ActivitiesInforefXmlGradeItemref.GradeItem = ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ?? new ActivitiesInforefXmlGradeItem();
        
        ActivitiesInforefXmlInforef.Fileref = ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref ?? new ActivitiesInforefXmlFileref(); 
        ActivitiesInforefXmlInforef.GradeItemref = ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref ?? new ActivitiesInforefXmlGradeItemref();
        
        ActivitiesInforefXmlInforef.Serialize("resource", FileElementId);
    }
     
     /// <summary>
     /// Creates a Resource folder in the activity folder. Each activity needs an folder.
     /// </summary>
     /// <param name="moduleId"></param>
     public void CreateActivityFolder(string moduleId)
     {
         var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
         _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "resource_"+moduleId));
     }
    
}