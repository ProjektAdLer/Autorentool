using System.Globalization;
using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities._activities.Url.xml;

namespace Generator.XmlClasses.XmlFileFactories;

public class XmlUrlFactory : IXmlUrlFactory
{
    private readonly IFileSystem _fileSystem;
    public readonly string CurrentTime;
    public string UrlDescription;
    public string UrlId;
    public string UrlLink;
    public List<ILearningElementJson> UrlList;
    public string UrlName;
    public string UrlParentSpaceId;
    public float UrlPoints;
    public string UrlUuid;

    public XmlUrlFactory(IReadDsl readDsl, IFileSystem? fileSystem = null,
        IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null,
        IActivitiesGradesXmlActivityGradebook? gradebook = null,
        IActivitiesUrlXmlUrl? urlXmlUrl = null, IActivitiesUrlXmlActivity? urlXmlActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null,
        IActivitiesInforefXmlFileref? inforefXmlFileref = null,
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null,
        IActivitiesInforefXmlGradeItemref? inforefXmlGradeItemref = null,
        IActivitiesInforefXmlInforef? inforefXmlInforef = null)
    {
        ReadDsl = readDsl;
        UrlId = "";
        UrlUuid = "";
        UrlName = "";
        UrlParentSpaceId = "";
        UrlLink = "";
        UrlDescription = "";
        UrlPoints = 0;
        UrlList = new List<ILearningElementJson>();

        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        _fileSystem = fileSystem ?? new FileSystem();


        ActivitiesGradesXmlGradeItem = gradesGradeItem ?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlGradeItems = gradesGradeItems ?? new ActivitiesGradesXmlGradeItems();
        ActivitiesGradesXmlActivityGradebook = gradebook ?? new ActivitiesGradesXmlActivityGradebook();

        ActivitiesUrlXmlUrl = urlXmlUrl ?? new ActivitiesUrlXmlUrl();
        ActivitiesUrlXmlActivity = urlXmlActivity ?? new ActivitiesUrlXmlActivity();

        ActivitiesRolesXmlRoles = roles ?? new ActivitiesRolesXmlRoles();

        ActivitiesModuleXmlModule = module ?? new ActivitiesModuleXmlModule();

        ActivitiesGradeHistoryXmlGradeHistory = gradeHistory ?? new ActivitiesGradeHistoryXmlGradeHistory();

        ActivitiesInforefXmlFileref = inforefXmlFileref ?? new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlGradeItem = inforefXmlGradeItem ?? new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlGradeItemref = inforefXmlGradeItemref ?? new ActivitiesInforefXmlGradeItemref();
        ActivitiesInforefXmlInforef = inforefXmlInforef ?? new ActivitiesInforefXmlInforef();
    }

    public IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    public IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    public IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    public IActivitiesUrlXmlActivity ActivitiesUrlXmlActivity { get; }
    public IActivitiesUrlXmlUrl ActivitiesUrlXmlUrl { get; }
    public IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    public IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    public IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    public IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    public IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    public IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }
    public IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    public IReadDsl ReadDsl { get; }

    /// <inheritdoc cref="IXmlUrlFactory.CreateUrlFactory"/>
    public void CreateUrlFactory()
    {
        UrlList = ReadDsl.GetUrlElementList();

        UrlSetParameters(UrlList);
    }

    /// <summary>
    /// Reads the URL list and sets parameters for each URL in the list.
    /// </summary>
    public void UrlSetParameters(List<ILearningElementJson> urlList)
    {
        foreach (var url in urlList)
        {
            UrlId = url.ElementId.ToString();
            UrlUuid = url.ElementUUID;
            UrlName = url.ElementName;
            UrlLink = url.Url;

            switch (url)
            {
                case BaseLearningElementJson:
                    UrlParentSpaceId = (ReadDsl.GetSpaceList().Count + 1).ToString();
                    break;
                case LearningElementJson learningElementJson:
                    UrlParentSpaceId = learningElementJson.LearningSpaceParentId.ToString();
                    UrlDescription = learningElementJson.ElementDescription ?? "";
                    UrlPoints = learningElementJson.ElementMaxScore;
                    break;
            }

            SetParametersActivityUrl();
        }
    }

    /// <summary>
    /// Sets parameters for a URL activity including its related XML files.
    /// </summary>
    public void SetParametersActivityUrl()
    {
        CreateActivityFolder(UrlId);

        //file activities/label.../grades.xml
        ActivitiesGradesXmlGradeItems.GradeItem = ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem ??
                                                  new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlActivityGradebook.GradeItems =
            ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems ?? new ActivitiesGradesXmlGradeItems();

        ActivitiesGradesXmlActivityGradebook.Serialize("url", UrlId);

        //file activities/url.../url.xml
        ActivitiesUrlXmlUrl.Name = UrlName;
        ActivitiesUrlXmlUrl.Intro = UrlLink + "<p style=\"position:relative; background-color:#e6e9ed;\">" +
                                    UrlDescription + "</p>";
        ActivitiesUrlXmlUrl.Externalurl = UrlLink;
        ActivitiesUrlXmlUrl.Timemodified = CurrentTime;

        ActivitiesUrlXmlActivity.Url = ActivitiesUrlXmlUrl as ActivitiesUrlXmlUrl ?? new ActivitiesUrlXmlUrl();
        ActivitiesUrlXmlActivity.Id = UrlId;
        ActivitiesUrlXmlActivity.Moduleid = UrlId;
        ActivitiesUrlXmlActivity.Contextid = UrlId;

        ActivitiesUrlXmlActivity.Serialize("url", UrlId);

        //file activities/label.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("url", UrlId);

        //file activities/label.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "url";
        ActivitiesModuleXmlModule.ShowDescription = "1";
        ActivitiesModuleXmlModule.Indent = "1";
        ActivitiesModuleXmlModule.SectionId = UrlParentSpaceId;
        ActivitiesModuleXmlModule.SectionNumber = UrlParentSpaceId;
        ActivitiesModuleXmlModule.Added = CurrentTime;
        ActivitiesModuleXmlModule.Id = UrlId;
        ActivitiesModuleXmlModule.Completion = "1";
        //AdlerScore can not be null at this point because it is set in the constructor
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.ScoreMax =
            UrlPoints.ToString("F5", CultureInfo.InvariantCulture);
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.Uuid = UrlUuid;

        ActivitiesModuleXmlModule.Serialize("url", UrlId);

        //file activities/label.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("url", UrlId);

        //file activities/label.../inforef.xml
        ActivitiesInforefXmlGradeItemref.GradeItem = ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ??
                                                     new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlInforef.Fileref = ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref ??
                                              new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlInforef.GradeItemref =
            ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref ??
            new ActivitiesInforefXmlGradeItemref();

        ActivitiesInforefXmlInforef.Serialize("url", UrlId);
    }

    /// <summary>
    /// Creates a label folder in the activity folder. Each activity needs an folder.
    /// </summary>
    /// <param name="moduleId"></param>
    public void CreateActivityFolder(string moduleId)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities",
            "url_" + moduleId));
    }
}