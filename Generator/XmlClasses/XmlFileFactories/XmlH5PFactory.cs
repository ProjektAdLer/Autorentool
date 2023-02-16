using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.H5PActivity.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities.Files.xml;

namespace Generator.XmlClasses.XmlFileFactories;

/// <summary>
/// Creates all H5P files in the needed XML File.
/// </summary>
public class XmlH5PFactory : IXmlH5PFactory
{
    private readonly string _currWorkDir;
    private readonly string _hardcodedPath = "XMLFilesForExport";
    public string H5PElementId;
    public string H5PElementName;
    public string H5PElementParentSpaceString;
    public string H5PElementType;
    public string H5PElementDesc;
    public string CurrentTime;
    private List<FilesXmlFile> _filesXmlFilesList;
    private List<ActivitiesInforefXmlFile> _activitiesInforefXmlFileList;

    public readonly IXmlFileManager FileManager;
    public IFilesXmlFiles FilesXmlFiles { get; }
    public IFilesXmlFile FilesXmlFileBlock1 { get; }
    public IFilesXmlFile FilesXmlFileBlock2 { get; }
    public IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    public IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    public IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    public IActivitiesH5PActivityXmlActivity ActivitiesH5PActivityXmlActivity { get; }
    public IActivitiesH5PActivityXmlH5PActivity ActivitiesH5PActivityXmlH5PActivity { get; }
    public IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    public IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    public IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    public IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock1 { get; }
    public IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock2 { get; }
    public IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    public IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    public IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }
    public IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    public IReadDsl ReadDsl { get; }

    private readonly IFileSystem _fileSystem;

    

    public XmlH5PFactory(IReadDsl readDsl, IXmlFileManager? xmlFileManager = null, IFileSystem? fileSystem = null,
        IFilesXmlFiles? filesXmlFiles = null,
        IFilesXmlFile? filesXmlFile = null, IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null, IActivitiesGradesXmlActivityGradebook? gradebook = null,
        IActivitiesH5PActivityXmlActivity? h5PActivityXmlActivity = null,
        IActivitiesH5PActivityXmlH5PActivity? h5PActivityXmlH5PActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null, IActivitiesInforefXmlFile? inforefXmlFile = null,
        IActivitiesInforefXmlFileref? inforefXmlFileref = null,
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null,
        IActivitiesInforefXmlGradeItemref? inforefXmlGradeItemref = null,
        IActivitiesInforefXmlInforef? inforefXmlInforef = null)
    {
        H5PElementId = "";
        H5PElementName = "";
        H5PElementParentSpaceString = "";
        H5PElementType = "";
        H5PElementDesc = "";
        _filesXmlFilesList = new List<FilesXmlFile>();
        _activitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();
        _fileSystem = fileSystem?? new FileSystem();
        
        FileManager = xmlFileManager?? new XmlFileManager();
        
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

        ReadDsl = readDsl;
        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        _currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
    }
    
    
    /// <summary>
    /// Create H5P structure in files.xml, folder activity and folder sections for every H5P element in the DSL Document
    /// </summary>
    public void CreateH5PFileFactory()
    {
        // Get all the H5P elements that are in the DSL Document
        List<LearningElementJson> h5PElementsList = ReadDsl.GetH5PElementsList();

        _filesXmlFilesList = new List<FilesXmlFile>();
        _filesXmlFilesList = FileManager.GetXmlFilesList();

        ReadH5PListAndSetParameters(h5PElementsList);

        FilesXmlFiles.File = _filesXmlFilesList;
        FilesXmlFiles.Serialize();
        FileManager.SetXmlFilesList(_filesXmlFilesList);
    }
    
    public void ReadH5PListAndSetParameters(List<LearningElementJson> h5PElementsList)
    {
        // For Each H5P element in the list 
        // (for files.xml) set the H5P element id, name, hash value, copy the File to the needed location in the backup structure
        // and all the other parameters in (H5PSetParametersFilesXml)
        // Create folder activities
        foreach (var h5PElement in h5PElementsList)
        {
            H5PElementId = h5PElement.Id.ToString();
            H5PElementName = h5PElement.LmsElementIdentifierJson.Value;
            H5PElementParentSpaceString = h5PElement.LearningSpaceParentId.ToString();
            H5PElementType = h5PElement.ElementFileType;
            H5PElementDesc = h5PElement.ElementDescription ?? "";

            FileManager.CalculateHashCheckSumAndFileSize(_fileSystem.Path.Join(_currWorkDir, _hardcodedPath,
                h5PElement.LmsElementIdentifierJson.Value+"."+h5PElement.ElementFileType));
            FileManager.CreateFolderAndFiles(_fileSystem.Path.Join(_currWorkDir, _hardcodedPath, 
                h5PElement.LmsElementIdentifierJson.Value+"."+h5PElement.ElementFileType), 
            FileManager.GetHashCheckSum());
            H5PSetParametersFilesXml(FileManager.GetHashCheckSum(), FileManager.GetFileSize());
            H5PSetParametersActivity();

            // These ints are needed for the activities/inforef.xml file. 
            XmlEntityManager.IncreaseFileId();
        }
    }

    /// <summary>
    /// Setting Parameters for h5p element in files.xml, 
    /// </summary>
    /// <param name="hashCheckSum"></param> SHA1 Hash value for the file
    /// <param name="filesize"></param> Byte Filesize for the file
    public void H5PSetParametersFilesXml(string hashCheckSum, string filesize)
    {
        var file1 = new FilesXmlFile()
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString(),
            ContentHash = hashCheckSum,
            ContextId = H5PElementId,
            Filename = H5PElementName,
            Source = H5PElementName+"."+H5PElementType,
            Filesize = filesize,
            Component = "mod_h5pactivity",
            FileArea = "package",
            Mimetype = "application/zip.h5p",
            Timecreated = CurrentTime,
            Timemodified = CurrentTime,
        };

        FilesXmlFile file2 = (FilesXmlFile) file1.Clone();
        file2.Id = XmlEntityManager.GetFileIdBlock2().ToString();

        _filesXmlFilesList.Add(file1);
        _filesXmlFilesList.Add(file2);
    }

    /// <summary>
    /// Create Folder Activity and the needed Activity Files
    /// </summary>
    public void H5PSetParametersActivity()
    {
        CreateActivityFolder(H5PElementId);
        
        //file activities/h5p.../grades.xml
        ActivitiesGradesXmlGradeItem.CategoryId = H5PElementId;
        ActivitiesGradesXmlGradeItem.ItemName = H5PElementName;
        ActivitiesGradesXmlGradeItem.ItemType = "mod";
        ActivitiesGradesXmlGradeItem.ItemModule = "h5pactivity";
        ActivitiesGradesXmlGradeItem.Timecreated = CurrentTime;
        ActivitiesGradesXmlGradeItem.Timemodified = CurrentTime;
        ActivitiesGradesXmlGradeItem.Id = H5PElementId;
        
        ActivitiesGradesXmlGradeItems.GradeItem = ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem ?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlActivityGradebook.GradeItems = ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems ?? new ActivitiesGradesXmlGradeItems();

        ActivitiesGradesXmlActivityGradebook.Serialize("h5pactivity", H5PElementId);
        
        //file activities/h5p.../h5pactivity.xml
        ActivitiesH5PActivityXmlH5PActivity.Name = H5PElementName;
        ActivitiesH5PActivityXmlH5PActivity.Timecreated = CurrentTime;
        ActivitiesH5PActivityXmlH5PActivity.Timemodified = CurrentTime;
        ActivitiesH5PActivityXmlH5PActivity.Id = H5PElementId;
        ActivitiesH5PActivityXmlH5PActivity.Intro = "<p style=\"position:relative; background-color:#e6e9ed;\">"+H5PElementDesc+"</p>";

        ActivitiesH5PActivityXmlActivity.H5Pactivity = ActivitiesH5PActivityXmlH5PActivity as ActivitiesH5PActivityXmlH5PActivity ?? new ActivitiesH5PActivityXmlH5PActivity();
        ActivitiesH5PActivityXmlActivity.Id = H5PElementId;
        ActivitiesH5PActivityXmlActivity.ModuleId = H5PElementId;
        ActivitiesH5PActivityXmlActivity.ModuleName = "h5pactivity";
        ActivitiesH5PActivityXmlActivity.ContextId = H5PElementId;

        ActivitiesH5PActivityXmlActivity.Serialize("h5pactivity", H5PElementId);
        
        //file activities/h5p.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("h5pactivity", H5PElementId);
        
        //file activities/h5p.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "h5pactivity";
        ActivitiesModuleXmlModule.SectionId = H5PElementParentSpaceString;
        ActivitiesModuleXmlModule.SectionNumber = H5PElementParentSpaceString;
        ActivitiesModuleXmlModule.IdNumber = "";
        ActivitiesModuleXmlModule.Indent = "1";
        ActivitiesModuleXmlModule.Added = CurrentTime;
        ActivitiesModuleXmlModule.ShowDescription = "0";
        ActivitiesModuleXmlModule.Id = H5PElementId;
        ActivitiesModuleXmlModule.ShowDescription = "1";

        ActivitiesModuleXmlModule.Serialize("h5pactivity", H5PElementId);
        
        //file activities/h5p.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("h5pactivity", H5PElementId);
        
        //file activities/h5p.../inforef.xml
        _activitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();
  
        var inforefFile1 = new ActivitiesInforefXmlFile()
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString()
        };
        var inforefFile2 = new ActivitiesInforefXmlFile()
        {
            Id = XmlEntityManager.GetFileIdBlock2().ToString()
        };
            
        _activitiesInforefXmlFileList.Add(inforefFile1);
        _activitiesInforefXmlFileList.Add(inforefFile2);

        ActivitiesInforefXmlFileref.File = _activitiesInforefXmlFileList;
        
        ActivitiesInforefXmlGradeItemref.GradeItem = ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ?? new ActivitiesInforefXmlGradeItem();
        
        ActivitiesInforefXmlInforef.Fileref = ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref ?? new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlInforef.GradeItemref = ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref ?? new ActivitiesInforefXmlGradeItemref();
        
        ActivitiesInforefXmlInforef.Serialize("h5pactivity", H5PElementId);
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
    
}
