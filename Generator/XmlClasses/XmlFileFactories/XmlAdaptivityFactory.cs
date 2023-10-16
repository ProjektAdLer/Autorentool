using System.Globalization;
using System.IO.Abstractions;
using Generator.DSL;
using Generator.DSL.AdaptivityElement;
using Generator.XmlClasses.Entities._activities.AdLerAdaptivity.xml;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;

namespace Generator.XmlClasses.XmlFileFactories;

public class XmlAdaptivityFactory : IXmlAdaptivityFactory
{
    private readonly string _currentTime;
    private readonly IFileSystem _fileSystem;
    private string _adaptivityElementId;
    private string _adaptivityElementName;
    private string _adaptivityElementParentSpaceId;
    private float _adaptivityElementPoints;
    private string _adaptivityElementUuid;
    private List<IAdaptivityElementJson> _listAdaptivityElements;

    public XmlAdaptivityFactory(IReadDsl readDsl, IFileSystem? fileSystem = null,
        IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null,
        IActivitiesGradesXmlActivityGradebook? gradeBook = null,
        IActivitiesAdLerAdaptivityXmlActivity? activityAdLerAdaptivityXmlActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null,
        IActivitiesInforefXmlFileref? inforefXmlFileref = null,
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null,
        IActivitiesInforefXmlGradeItemref? inforefXmlGradeItemref = null,
        IActivitiesInforefXmlInforef? inforefXmlInforef = null)
    {
        ReadDsl = readDsl;

        ActivitiesGradesXmlGradeItem = gradesGradeItem ?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlGradeItems = gradesGradeItems ?? new ActivitiesGradesXmlGradeItems();
        ActivitiesGradesXmlActivityGradebook = gradeBook ?? new ActivitiesGradesXmlActivityGradebook();

        ActivitiesRolesXmlRoles = roles ?? new ActivitiesRolesXmlRoles();

        ActivitiesModuleXmlModule = module ?? new ActivitiesModuleXmlModule();

        ActivitiesGradeHistoryXmlGradeHistory = gradeHistory ?? new ActivitiesGradeHistoryXmlGradeHistory();

        ActivitiesInforefXmlFileref = inforefXmlFileref ?? new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlGradeItem = inforefXmlGradeItem ?? new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlGradeItemref = inforefXmlGradeItemref ?? new ActivitiesInforefXmlGradeItemref();
        ActivitiesInforefXmlInforef = inforefXmlInforef ?? new ActivitiesInforefXmlInforef();

        ActivitiesAdLerAdaptivityXmlActivity =
            activityAdLerAdaptivityXmlActivity ?? new ActivitiesAdLerAdaptivityXmlActivity();

        _adaptivityElementId = "0";
        _adaptivityElementName = "";
        _adaptivityElementParentSpaceId = "";
        _adaptivityElementPoints = 0;
        _adaptivityElementUuid = "";

        _currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        _listAdaptivityElements = new List<IAdaptivityElementJson>();

        _fileSystem = fileSystem ?? new FileSystem();
    }

    private IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }

    private IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }

    private IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    private IActivitiesAdLerAdaptivityXmlActivity ActivitiesAdLerAdaptivityXmlActivity { get; set; }

    private IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }

    private IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }

    private IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }

    private IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }

    private IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }

    private IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }

    private IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    private IReadDsl ReadDsl { get; }

    public void CreateXmlAdaptivityFactory()
    {
        _listAdaptivityElements = ReadDsl.GetAdaptivityElementsList();

        AdaptivitySetParameters();
    }

    private void AdaptivitySetParameters()
    {
        foreach (var adaptivityElement in _listAdaptivityElements)
        {
            _adaptivityElementId = adaptivityElement.ElementId.ToString();
            _adaptivityElementName = adaptivityElement.ElementName;
            _adaptivityElementUuid = adaptivityElement.ElementUUID;
            _adaptivityElementParentSpaceId = adaptivityElement.LearningSpaceParentId.ToString();
            _adaptivityElementPoints = adaptivityElement.ElementMaxScore;

            SetParametersActivityAdaptivity(adaptivityElement);
        }
    }

    private void SetParametersActivityAdaptivity(IAdaptivityElementJson adaptivityElement)
    {
        CreateActivityFolder(_adaptivityElementId);

        //file activities/adaptivity_.../grades.xml
        ActivitiesGradesXmlGradeItems.GradeItem = ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem ??
                                                  new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlActivityGradebook.GradeItems =
            ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems ?? new ActivitiesGradesXmlGradeItems();

        ActivitiesGradesXmlActivityGradebook.Serialize("adlerAdaptivity", _adaptivityElementId);

        //file activities/adaptivity_.../adlerAdaptivity.xml
        ActivitiesAdLerAdaptivityXmlActivity = new ActivitiesAdLerAdaptivityXmlActivity(_adaptivityElementId);

        ActivitiesAdLerAdaptivityXmlActivity.AdlerAdaptivity =
            new ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivity(_adaptivityElementId, _adaptivityElementName);

        foreach (var task in adaptivityElement.AdaptivityContent.AdaptivityTask)
        {
            var optional = task.Optional ? 1 : 0;

            var activitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask =
                new ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask(task.TaskId, task.TaskTitle, task.TaskUUID,
                    optional, task.RequiredDifficulty.ToString());

            foreach (var question in task.AdaptivityQuestions)
            {
                activitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask.Questions.Questions
                    .Add(new ActivitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTaskQuestion(question.QuestionId,
                        question.QuestionDifficulty));
            }

            ActivitiesAdLerAdaptivityXmlActivity.AdlerAdaptivity.Tasks.Tasks
                .Add(activitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask);
        }

        ActivitiesAdLerAdaptivityXmlActivity.Serialize("adlerAdaptivity", _adaptivityElementId);

        //file activities/adaptivity_.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("adlerAdaptivity", _adaptivityElementId);

        //file activities/adaptivity_.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "adlerAdaptivity";
        ActivitiesModuleXmlModule.ShowDescription = "1";
        ActivitiesModuleXmlModule.Indent = "1";
        ActivitiesModuleXmlModule.SectionId = _adaptivityElementParentSpaceId;
        ActivitiesModuleXmlModule.SectionNumber = _adaptivityElementParentSpaceId;
        ActivitiesModuleXmlModule.Added = _currentTime;
        ActivitiesModuleXmlModule.Id = _adaptivityElementId;
        ActivitiesModuleXmlModule.Completion = "1";
        //AdlerScore can not be null at this point because it is set in the constructor
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.ScoreMax =
            _adaptivityElementPoints.ToString("F5", CultureInfo.InvariantCulture);
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.Uuid = _adaptivityElementUuid;

        ActivitiesModuleXmlModule.Serialize("adlerAdaptivity", _adaptivityElementId);

        //file activities/adaptivity_.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("adlerAdaptivity", _adaptivityElementId);

        //file activities/adaptivity_.../inforef.xml
        ActivitiesInforefXmlGradeItemref.GradeItem = ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ??
                                                     new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlInforef.Fileref = ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref ??
                                              new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlInforef.GradeItemref =
            ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref ??
            new ActivitiesInforefXmlGradeItemref();

        ActivitiesInforefXmlInforef.Serialize("adlerAdaptivity", _adaptivityElementId);
    }

    private void CreateActivityFolder(string moduleId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities",
            "adlerAdaptivity_" + moduleId));
    }
}