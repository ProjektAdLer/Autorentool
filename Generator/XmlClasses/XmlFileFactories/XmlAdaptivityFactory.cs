using System.Globalization;
using System.IO.Abstractions;
using Generator.ATF;
using Generator.ATF.AdaptivityElement;
using Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;

namespace Generator.XmlClasses.XmlFileFactories;

public class XmlAdaptivityFactory : IXmlAdaptivityFactory
{
    internal readonly string CurrentTime;
    internal readonly IFileSystem FileSystem;
    internal string AdaptivityElementDescriptionGoals;
    internal string AdaptivityElementId;
    internal string AdaptivityElementName;
    internal string AdaptivityElementParentSpaceId;
    internal float AdaptivityElementPoints;
    internal string AdaptivityElementUuid;
    internal List<IAdaptivityElementJson> ListAdaptivityElements;

    public XmlAdaptivityFactory(IReadAtf readAtf, IFileSystem? fileSystem = null,
        IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null,
        IActivitiesGradesXmlActivityGradebook? gradeBook = null,
        IActivitiesAdleradaptivityXmlActivity? activityAdLerAdaptivityXmlActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null,
        IActivitiesInforefXmlFileref? inforefXmlFileref = null,
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null,
        IActivitiesInforefXmlGradeItemref? inforefXmlGradeItemref = null,
        IActivitiesInforefXmlInforef? inforefXmlInforef = null)
    {
        ReadAtf = readAtf;

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

        ActivitiesAdleradaptivityXmlActivity =
            activityAdLerAdaptivityXmlActivity ?? new ActivitiesAdleradaptivityXmlActivity();

        AdaptivityElementId = "0";
        AdaptivityElementDescriptionGoals = "";
        AdaptivityElementName = "";
        AdaptivityElementParentSpaceId = "";
        AdaptivityElementPoints = 0;
        AdaptivityElementUuid = "";

        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        ListAdaptivityElements = new List<IAdaptivityElementJson>();

        FileSystem = fileSystem ?? new FileSystem();
    }

    internal IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }

    internal IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }

    internal IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    internal IActivitiesAdleradaptivityXmlActivity ActivitiesAdleradaptivityXmlActivity { get; set; }

    internal IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }

    internal IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }

    internal IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }

    internal IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }

    internal IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }

    internal IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }

    internal IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    internal IReadAtf ReadAtf { get; }

    public void CreateXmlAdaptivityFactory()
    {
        ListAdaptivityElements = ReadAtf.GetAdaptivityElementsList();

        AdaptivitySetParameters();
    }

    private void AdaptivitySetParameters()
    {
        foreach (var adaptivityElement in ListAdaptivityElements)
        {
            AdaptivityElementId = adaptivityElement.ElementId.ToString();
            AdaptivityElementName = adaptivityElement.ElementName;
            AdaptivityElementUuid = adaptivityElement.ElementUUID;
            AdaptivityElementParentSpaceId = adaptivityElement.LearningSpaceParentId.ToString();
            AdaptivityElementPoints = adaptivityElement.ElementMaxScore;
            AdaptivityElementDescriptionGoals = "<h5>Description:</h5> " + "<p>" +
                                                adaptivityElement.ElementDescription + "</p>" +
                                                "<h5>Goals:</h5> " + "<p>" +
                                                string.Join("<br>", adaptivityElement.ElementGoals) + "</p>";

            SetParametersActivityAdaptivity(adaptivityElement);
        }
    }

    private void SetParametersActivityAdaptivity(IAdaptivityElementJson adaptivityElement)
    {
        CreateActivityFolder(AdaptivityElementId);

        //file activities/adaptivity_.../grades.xml
        ActivitiesGradesXmlActivityGradebook.Serialize("adleradaptivity", AdaptivityElementId);

        //file activities/adaptivity_.../adleradaptivity.xml
        ActivitiesAdleradaptivityXmlActivity = new ActivitiesAdleradaptivityXmlActivity(AdaptivityElementId);

        ActivitiesAdleradaptivityXmlActivity.Adleradaptivity =
            new ActivitiesAdleradaptivityXmlActivityAdleradaptivity(AdaptivityElementId, AdaptivityElementName,
                AdaptivityElementDescriptionGoals);

        foreach (var task in adaptivityElement.AdaptivityContent.AdaptivityTasks)
        {
            var requiredDifficultyXml = task.Optional ? "$@NULL@$" : task.RequiredDifficulty.ToString();

            var activitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask =
                new ActivitiesAdleradaptivityXmlActivityAdleradaptivityTask(task.TaskId, task.TaskTitle, task.TaskUUID,
                    requiredDifficultyXml);

            foreach (var question in task.AdaptivityQuestions)
            {
                activitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask.Questions.Questions
                    .Add(new ActivitiesAdleradaptivityXmlActivityAdleradaptivityTaskQuestion(question.QuestionId,
                        question.QuestionDifficulty));
            }

            ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks
                .Add(activitiesAdLerAdaptivityXmlActivityAdlerAdaptivityTask);
        }

        ActivitiesAdleradaptivityXmlActivity.Serialize("adleradaptivity", AdaptivityElementId);

        //file activities/adaptivity_.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("adleradaptivity", AdaptivityElementId);

        //file activities/adaptivity_.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "adleradaptivity";
        ActivitiesModuleXmlModule.ShowDescription = "1";
        ActivitiesModuleXmlModule.Indent = "1";
        ActivitiesModuleXmlModule.SectionId = AdaptivityElementParentSpaceId;
        ActivitiesModuleXmlModule.SectionNumber = AdaptivityElementParentSpaceId;
        ActivitiesModuleXmlModule.Added = CurrentTime;
        ActivitiesModuleXmlModule.Id = AdaptivityElementId;
        ActivitiesModuleXmlModule.Completion = "2";
        //AdlerScore can not be null at this point because it is set in the constructor
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.ScoreMax =
            AdaptivityElementPoints.ToString("F5", CultureInfo.InvariantCulture);
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.Uuid = AdaptivityElementUuid;

        ActivitiesModuleXmlModule.Serialize("adleradaptivity", AdaptivityElementId);

        //file activities/adaptivity_.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("adleradaptivity", AdaptivityElementId);

        //file activities/adaptivity_.../inforef.xml
        ActivitiesInforefXmlGradeItemref.GradeItem = ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ??
                                                     new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlInforef.Fileref = ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref ??
                                              new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlInforef.GradeItemref =
            ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref ??
            new ActivitiesInforefXmlGradeItemref();

        ActivitiesInforefXmlInforef.Serialize("adleradaptivity", AdaptivityElementId);
    }

    private void CreateActivityFolder(string moduleId)
    {
        var currWorkDir = FileSystem.Directory.GetCurrentDirectory();
        FileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities",
            "adleradaptivity_" + moduleId));
    }
}