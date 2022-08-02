using System.IO.Abstractions;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.GradeHistory.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Grades.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Lesson.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Module.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Roles.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Section.xml;

namespace AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;


public class XmlLessonFactory {
    
    private string? currWorkDir;
    private string hardcodedPath = "XMLFilesForExport";
    private string? learningSpaceId;
    private string? learningSpaceName;
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
    
    internal IActivitiesLessonXmlAnswer ActivitiesLessonXmlAnswer { get; }
    internal IActivitiesLessonXmlAnswers ActivitiesLessonXmlAnswers { get; }
    internal IActivitiesLessonXmlPage ActivitiesLessonXmlPage { get; }
    internal IActivitiesLessonXmlPages ActivitiesLessonXmlPages { get; }
    internal IActivitiesLessonXmlLesson ActivitiesLessonXmlLesson { get; }
    internal IActivitiesLessonXmlActivity ActivitiesLessonXmlActivity { get; }
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
    

    public XmlLessonFactory(IReadDSL readDsl, XmlFileManager xmlFileManager, IFileSystem? fileSystem = null,
        IFilesXmlFiles? filesXmlFiles = null,
        IFilesXmlFile? filesXmlFile = null, IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null, ActivitiesGradesXmlActivityGradebook? gradebook = null,
        ActivitiesLessonXmlAnswer? lessonXmlAnswer = null, ActivitiesLessonXmlAnswers? lessonXmlAnswers = null, 
        ActivitiesLessonXmlPage? activitiesLessonXmlPage = null, ActivitiesLessonXmlPages? activitiesLessonXmlPages = null, 
        ActivitiesLessonXmlLesson? activitiesLessonXmlLesson = null, ActivitiesLessonXmlActivity? activitiesLessonXmlActivity = null,
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

        ActivitiesLessonXmlAnswer = lessonXmlAnswer?? new ActivitiesLessonXmlAnswer();
        ActivitiesLessonXmlAnswers = lessonXmlAnswers?? new ActivitiesLessonXmlAnswers();
        ActivitiesLessonXmlPage = activitiesLessonXmlPage?? new ActivitiesLessonXmlPage();
        ActivitiesLessonXmlPages = activitiesLessonXmlPages?? new ActivitiesLessonXmlPages();
        ActivitiesLessonXmlLesson = activitiesLessonXmlLesson?? new ActivitiesLessonXmlLesson();
        ActivitiesLessonXmlActivity = activitiesLessonXmlActivity?? new ActivitiesLessonXmlActivity();

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


    public void CreateLessonFactory()
    {

        List<LearningSpaceJson>? learningSpaceList = ReadDsl.GetLearningSpaceList();
        filesXmlFilesList = new List<FilesXmlFile>();
        filesXmlFilesList = _fileManager.GetXmlFilesList();
        var learningElement = ReadDsl.GetH5PElementsList();
        
        foreach (var space in learningSpaceList)
        {
            learningSpaceId = space.spaceId.ToString();
            learningSpaceName = space.identifier.value;
            
            _fileManager.CalculateHashCheckSumAndFileSize(Path.Join(currWorkDir, hardcodedPath, learningElement[0].identifier.value));
            _fileManager.CreateFolderAndFiles(Path.Join(currWorkDir, hardcodedPath, learningElement[0].identifier.value), _fileManager.fileCheckSum);
            LessonSetParametersFilesXml(learningElement[0], _fileManager.fileCheckSum, _fileManager.fileSize);
            LessonSetParametersActivity(learningElement[0]);
            LessonSetParametersSection();
            
            // These ints are needed for the activities/inforef.xml file. 
            XmlEntityManager.IncreaseFileId();
        }
        
        FilesXmlFiles.File = filesXmlFilesList;
        FilesXmlFiles.Serialize();
    }

    /// <summary>
    /// Setting Parameters for h5p element in files.xml, 
    /// </summary>
    /// <param name="hashCheckSum"></param> SHA1 Hash value for the file
    /// <param name="filesize"></param> Byte Filesize for the file
    public void LessonSetParametersFilesXml(LearningElementJson learningElementJson, string? hashCheckSum, string? filesize)
    {

        if (filesXmlFilesList != null)
        {
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].Id = XmlEntityManager.GetFileIdBlock1().ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContentHash = hashCheckSum;
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContextId = learningElementJson.id.ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].Component = "mod_lesson";
            filesXmlFilesList[filesXmlFilesList.Count - 1].FileArea = "page_contents";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filename = learningElementJson.identifier.value;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filesize = filesize;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Mimetype = "application/zip.h5p";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timecreated = currentTime;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timemodified = currentTime;
            
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].Id = XmlEntityManager.GetFileIdBlock2().ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContentHash = hashCheckSum;
            filesXmlFilesList[filesXmlFilesList.Count - 1].ContextId = learningElementJson.id.ToString();
            filesXmlFilesList[filesXmlFilesList.Count - 1].Component = "mod_lesson";
            filesXmlFilesList[filesXmlFilesList.Count - 1].FileArea = "page_contents";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filename = learningElementJson.identifier.value;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Filesize = filesize;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Mimetype = "application/zip.h5p";
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timecreated = currentTime;
            filesXmlFilesList[filesXmlFilesList.Count - 1].Timemodified = currentTime;
        }
    }

    
    /// <summary>
    /// Create Folder Activity and the needed Activity Files
    /// </summary>
    public void LessonSetParametersActivity(LearningElementJson learningElementJson)
    {
        CreateActivityFolder(learningSpaceId);
        
        //file activities/lesson.../grades.xml
        ActivitiesGradesXmlGradeItem.CategoryId = learningSpaceId;
        ActivitiesGradesXmlGradeItem.ItemName = learningSpaceName;
        ActivitiesGradesXmlGradeItem.ItemType = "mod";
        ActivitiesGradesXmlGradeItem.ItemModule = "lesson";
        ActivitiesGradesXmlGradeItem.IdNumber = "";
        ActivitiesGradesXmlGradeItem.Aggregationcoef2 = "0.25000";
        ActivitiesGradesXmlGradeItem.Sortorder = "5";
        ActivitiesGradesXmlGradeItem.Timecreated = currentTime;
        ActivitiesGradesXmlGradeItem.Timemodified = currentTime;
        ActivitiesGradesXmlGradeItem.Id = learningSpaceId;

        ActivitiesGradesXmlGradeItems.GradeItem = (ActivitiesGradesXmlGradeItem) ActivitiesGradesXmlGradeItem;
        ActivitiesGradesXmlActivityGradebook.GradeItems = (ActivitiesGradesXmlGradeItems) ActivitiesGradesXmlGradeItems;

        ActivitiesGradesXmlActivityGradebook.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../lesson.xml
        ActivitiesLessonXmlAnswer.JumpTo = "0";
        ActivitiesLessonXmlAnswer.Timecreated = currentTime;
        ActivitiesLessonXmlAnswer.Timemodified = currentTime;
        ActivitiesLessonXmlAnswer.AnswerText = "This_Page";
        ActivitiesLessonXmlAnswer.Id = "1";

        ActivitiesLessonXmlAnswers.Answer = (ActivitiesLessonXmlAnswer) ActivitiesLessonXmlAnswer;

        ActivitiesLessonXmlPage.PrevPageId = "0";
        ActivitiesLessonXmlPage.NextPageId = "0";
        ActivitiesLessonXmlPage.Timecreated = currentTime;
        ActivitiesLessonXmlPage.Timemodified = currentTime;
        ActivitiesLessonXmlPage.Title = "Content_Page_1";
        ActivitiesLessonXmlPage.Contents = "@@PLUGINFILE@@/" + learningElementJson.identifier.value;
        ActivitiesLessonXmlPage.Answers = (ActivitiesLessonXmlAnswers) ActivitiesLessonXmlAnswers;
        ActivitiesLessonXmlPage.Id = "1";
        
        ActivitiesLessonXmlPages.Page = (ActivitiesLessonXmlPage) ActivitiesLessonXmlPage;

        ActivitiesLessonXmlLesson.Name = learningSpaceName;
        ActivitiesLessonXmlLesson.Bgcolor = "#FFFFF";
        ActivitiesLessonXmlLesson.Progressbar = "1";
        ActivitiesLessonXmlLesson.Pages = (ActivitiesLessonXmlPages) ActivitiesLessonXmlPages;
        ActivitiesLessonXmlLesson.Id = learningSpaceId;
 
        ActivitiesLessonXmlActivity.Lesson = (ActivitiesLessonXmlLesson) ActivitiesLessonXmlLesson;
        ActivitiesLessonXmlActivity.Id = learningSpaceId;
        ActivitiesLessonXmlActivity.ModuleId = learningSpaceId;
        ActivitiesLessonXmlActivity.ModuleName = "lesson";
        
        ActivitiesLessonXmlActivity.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../roles.xml
        ActivitiesRolesXmlRoles.RoleOverrides = "";
        ActivitiesRolesXmlRoles.RoleAssignments = "";
        
        ActivitiesRolesXmlRoles.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "lesson";
        ActivitiesModuleXmlModule.SectionId = learningSpaceId;
        ActivitiesModuleXmlModule.SectionNumber = learningSpaceId;
        ActivitiesModuleXmlModule.Added = currentTime;
        ActivitiesModuleXmlModule.GroupingId = "0";
        ActivitiesModuleXmlModule.Id = learningSpaceId;

        ActivitiesModuleXmlModule.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../inforef.xml
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
        
        ActivitiesInforefXmlInforef.Serialize("lesson", learningSpaceId);
    }
    
    /// <summary>
    /// Create Folder section/ in the folder sections. And both files inforef.xml and section.xml
    /// </summary>
    public void LessonSetParametersSection()
    {
        CreateSectionsFolder(learningSpaceId);
        
        //file sections/section.../inforef.xml
        SectionsInforefXmlInforef.Serialize("", learningSpaceId);
        
        //file sections/section.../section.xml
        SectionsSectionXmlSection.Number = learningSpaceId;
        SectionsSectionXmlSection.Name = "$@NULL@$";
        SectionsSectionXmlSection.Summary = "$@NULL@$";
        SectionsSectionXmlSection.Timemodified = currentTime;
        SectionsSectionXmlSection.Id = learningSpaceId;

        SectionsSectionXmlSection.Serialize("", learningSpaceId);
    }
    

    /// <summary>
    /// Creates a h5p folder in the activity folder. Each activity needs an folder.
    /// </summary>
    /// <param name="moduleId"></param>
    public void CreateActivityFolder(string? moduleId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "lesson_"+moduleId));
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