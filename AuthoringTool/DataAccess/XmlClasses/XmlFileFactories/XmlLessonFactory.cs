
using System.IO.Abstractions;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.Entities.sections;

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
        
        FilesXmlFiles.SetParameters(filesXmlFilesList);
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
            filesXmlFilesList[filesXmlFilesList.Count - 1].SetParameters(hashCheckSum, learningElementJson.id.ToString(), "mod_lesson",
                "page_contents",
                "0", learningElementJson.identifier.value, filesize, "application/zip.h5p", "/", currentTime,
                currentTime, "$@NULL@$", "0", XmlEntityManager.GetFileIdBlock1().ToString());
            filesXmlFilesList.Add(new FilesXmlFile());
            filesXmlFilesList[filesXmlFilesList.Count - 1].SetParameters(hashCheckSum, learningElementJson.id.ToString(), "mod_lesson",
                "page_contents",
                "0", h5pElementName, filesize, "application/zip.h5p", "/", currentTime,
                currentTime, "$@NULL@$", "0", XmlEntityManager.GetFileIdBlock2().ToString());
        }
    }

    
    /// <summary>
    /// Create Folder Activity and the needed Activity Files
    /// </summary>
    public void LessonSetParametersActivity(LearningElementJson learningElementJson)
    {
        CreateActivityFolder(learningSpaceId);
        
        //file activities/lesson.../grades.xml
        ActivitiesGradesXmlGradeItem.SetParameters(learningSpaceId, learningSpaceName,
            "mod", "lesson", "1", "0", "$@NULL@$", "",
            "$@NULL@$", "1", "100.00000", "0.00000", "$@NULL@$", "$@NULL@$",
            "0.00000", "1.00000", "0.00000", "0.00000", "0.25000",
            "0", "5", "0", "$@NULL@$", "0", "0", "0", "0",
            currentTime, currentTime, "", learningSpaceId);
        ActivitiesGradesXmlGradeItems.SetParameters(ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem);
        ActivitiesGradesXmlActivityGradebook.SetParameterts(ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems,
            "");
        ActivitiesGradesXmlActivityGradebook.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../lesson.xml
        ActivitiesLessonXmlAnswer.SetParameters("0", "0", "0", "0", currentTime,
            currentTime, "This_Page", "$@NULL@$", "0", "0", "", "1");
        ActivitiesLessonXmlAnswers.SetParameters(ActivitiesLessonXmlAnswer as ActivitiesLessonXmlAnswer);
        ActivitiesLessonXmlPage.SetParameters("0", "0", "20", "0", "1", "1",
            currentTime, currentTime, "Content_Page_1", 
            "@@PLUGINFILE@@/"+learningElementJson.identifier.value, "1", 
            ActivitiesLessonXmlAnswers as ActivitiesLessonXmlAnswers, "", "1");
        ActivitiesLessonXmlPages.SetParameters(ActivitiesLessonXmlPage as ActivitiesLessonXmlPage);
        ActivitiesLessonXmlLesson.SetParameters("1", learningSpaceName, "", "1",
            "0", "0", "0", "", "0", "", "100", "1",
            "0", "0", "5", "1", "0", "0", "0",
            "0", "1", "0", "0", "0", "", "480",
            "640", "0", "0", "480", "640", "#FFFFF", "0",
            "0", "0", "0", "0", currentTime, "0",
            "0", "0", ActivitiesLessonXmlPages as ActivitiesLessonXmlPages, 
            "", "", "", learningSpaceId);
        ActivitiesLessonXmlActivity.SetParameters(ActivitiesLessonXmlLesson as ActivitiesLessonXmlLesson,
            learningSpaceId, learningSpaceId, "lesson", "1");
        ActivitiesLessonXmlActivity.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../roles.xml
        ActivitiesRolesXmlRoles.SetParameterts("", "");
        ActivitiesRolesXmlRoles.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../module.xml
        ActivitiesModuleXmlModule.SetParameterts("lesson", learningSpaceId, learningSpaceId,
            "", currentTime, "0", "0", "1",
            "1", "1", "0", "0",
            "1", "$@NULL@$", "0",
            "0", "$@NULL@$", "0", "", 
            learningSpaceId, "2021051700");
        ActivitiesModuleXmlModule.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.SetParameterts("");
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("lesson", learningSpaceId);
        
        //file activities/lesson.../inforef.xml
        ActivitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();
        if (ActivitiesInforefXmlFileList != null)
        {
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1]
                .SetParameters(XmlEntityManager.GetFileIdBlock1().ToString());
            ActivitiesInforefXmlFileList.Add(new ActivitiesInforefXmlFile());
            ActivitiesInforefXmlFileList[ActivitiesInforefXmlFileList.Count - 1]
                .SetParameters(XmlEntityManager.GetFileIdBlock2().ToString());

            ActivitiesInforefXmlFileref.SetParameters(ActivitiesInforefXmlFileList);
        }
        
        ActivitiesInforefXmlGradeItem.SetParameters("1");
        ActivitiesInforefXmlGradeItemref.SetParameters(ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ?? throw new InvalidOperationException());
        
        ActivitiesInforefXmlInforef.SetParameters(ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref, 
            ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref);
        ActivitiesInforefXmlInforef.Serialize("lesson", learningSpaceId);
    }
    
    /// <summary>
    /// Create Folder section/ in the folder sections. And both files inforef.xml and section.xml
    /// </summary>
    public void LessonSetParametersSection()
    {
        CreateSectionsFolder(learningSpaceId);
        
        //file sections/section.../inforef.xml
        SectionsInforefXmlInforef.SetParameters();
        SectionsInforefXmlInforef.Serialize("", learningSpaceId);
        
        //file sections/section.../section.xml
        SectionsSectionXmlSection.SetParameters(learningSpaceId, "$@NULL@$",
            "$@NULL@$", "0", "$@NULL@$", "1", 
            "$@NULL@$", currentTime, learningSpaceId);
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