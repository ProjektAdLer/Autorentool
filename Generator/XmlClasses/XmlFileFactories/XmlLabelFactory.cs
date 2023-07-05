using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Label.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;


namespace Generator.XmlClasses.XmlFileFactories;

public class XmlLabelFactory : IXmlLabelFactory
{
    public readonly string CurrentTime;
    private readonly IFileSystem _fileSystem;
    public string LabelId;
    public string LabelName;
    public string LabelGoal;
    public string LabelParentSpaceId;
    public string LabelDescription;
    public IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    public IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    public IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    public IActivitiesLabelXmlLabel ActivitiesLabelXmlLabel { get; }
    public IActivitiesLabelXmlActivity ActivitiesLabelXmlActivity { get; }
    public IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    public IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    public IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    public IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    public IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    public IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }
    public IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    public IReadDsl ReadDsl { get; }

    public XmlLabelFactory(IReadDsl readDsl, IFileSystem? fileSystem = null, IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null, IActivitiesGradesXmlActivityGradebook? gradebook = null,
        IActivitiesLabelXmlLabel? labelXmlLabel = null, IActivitiesLabelXmlActivity? labelXmlActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null, IActivitiesInforefXmlFileref? inforefXmlFileref = null, 
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null, IActivitiesInforefXmlGradeItemref? inforefXmlGradeItemref = null, 
        IActivitiesInforefXmlInforef? inforefXmlInforef = null)
    {        
        ReadDsl = readDsl;
        LabelId = "";
        LabelName = "";
        LabelGoal = "";
        LabelParentSpaceId = "";
        LabelDescription = "";

        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        _fileSystem = fileSystem?? new FileSystem();
        

        ActivitiesGradesXmlGradeItem = gradesGradeItem?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlGradeItems = gradesGradeItems?? new ActivitiesGradesXmlGradeItems();
        ActivitiesGradesXmlActivityGradebook = gradebook?? new ActivitiesGradesXmlActivityGradebook();
        
        ActivitiesLabelXmlLabel = labelXmlLabel?? new ActivitiesLabelXmlLabel();
        ActivitiesLabelXmlActivity = labelXmlActivity?? new ActivitiesLabelXmlActivity();

        ActivitiesRolesXmlRoles = roles?? new ActivitiesRolesXmlRoles();

        ActivitiesModuleXmlModule = module?? new ActivitiesModuleXmlModule();

        ActivitiesGradeHistoryXmlGradeHistory = gradeHistory?? new ActivitiesGradeHistoryXmlGradeHistory();
        
        ActivitiesInforefXmlFileref = inforefXmlFileref?? new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlGradeItem = inforefXmlGradeItem?? new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlGradeItemref = inforefXmlGradeItemref?? new ActivitiesInforefXmlGradeItemref();
        ActivitiesInforefXmlInforef = inforefXmlInforef?? new ActivitiesInforefXmlInforef();
        
    }
    
    /// <inheritdoc cref="IXmlLabelFactory.CreateLabelFactory"/>
    public void CreateLabelFactory()
    {
        var spaceAndWorldAttributesLabelList = ReadDsl.GetElementsOrderedList();
        var videoLinkLabelList = ReadDsl.GetLabelElementList();

        var labelList = videoLinkLabelList;
        labelList.AddRange(spaceAndWorldAttributesLabelList);
        
        LabelSetParameters(labelList);
    }

    /// <summary>
    /// Sets the parameters for each label in the provided list.
    /// </summary>
    private void LabelSetParameters(List<LearningElementJson> labelList)
    {
        foreach (var label in labelList)
        {
            LabelId = label.ElementId.ToString();
            LabelName = label.ElementName;
            LabelGoal = string.Join("<br>", label.ElementGoals);
            LabelParentSpaceId = label.LearningSpaceParentId.ToString();
            LabelDescription = label.ElementDescription ?? "";

            if (label.ElementCategory is "World Attributes")
            {
                LabelSetParametersWorldAttributes();
            }
        }
    }

    /// <summary>
    /// Sets the parameters for world attribute labels, creating files and serializing data.
    /// </summary>
    private void LabelSetParametersWorldAttributes()
    {
        CreateActivityFolder(LabelId);
        
        //file activities/label.../grades.xml
        ActivitiesGradesXmlGradeItems.GradeItem = ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem ?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlActivityGradebook.GradeItems = ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems ?? new ActivitiesGradesXmlGradeItems();

        ActivitiesGradesXmlActivityGradebook.Serialize("label", LabelId);
        
        //file activities/label.../label.xml
        ActivitiesLabelXmlLabel.Name = "DescriptionGoals";
        
        ActivitiesLabelXmlLabel.Id = LabelId;
        ActivitiesLabelXmlLabel.Intro = "<h5>Description:</h5> " + "<p>" + LabelDescription + "</p>" + 
                                        "<h5>Goals:</h5> " + "<p>" + LabelGoal + "</p>";
        ActivitiesLabelXmlLabel.Timemodified = CurrentTime;

        ActivitiesLabelXmlActivity.Label = ActivitiesLabelXmlLabel as ActivitiesLabelXmlLabel ?? new ActivitiesLabelXmlLabel();
        ActivitiesLabelXmlActivity.Id = LabelId;
        ActivitiesLabelXmlActivity.ModuleId = LabelId;
        ActivitiesLabelXmlActivity.ContextId = LabelId;

        ActivitiesLabelXmlActivity.Serialize("label", LabelId);

        //file activities/label.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("label", LabelId);
        
        //file activities/label.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "label";
        ActivitiesModuleXmlModule.SectionId = LabelParentSpaceId;
        ActivitiesModuleXmlModule.SectionNumber = LabelParentSpaceId;
        ActivitiesModuleXmlModule.Added = CurrentTime;
        ActivitiesModuleXmlModule.Id = LabelId;
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule = null;
        //Activity Completion is not needed on labels
        ActivitiesModuleXmlModule.Completion = "0";

        ActivitiesModuleXmlModule.Serialize("label", LabelId);
        
        //file activities/label.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("label", LabelId);
        
        //file activities/label.../inforef.xml
        ActivitiesInforefXmlGradeItemref.GradeItem = ActivitiesInforefXmlGradeItem as ActivitiesInforefXmlGradeItem ?? new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlInforef.Fileref = ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref ?? new ActivitiesInforefXmlFileref(); 
        ActivitiesInforefXmlInforef.GradeItemref = ActivitiesInforefXmlGradeItemref as ActivitiesInforefXmlGradeItemref ?? new ActivitiesInforefXmlGradeItemref();
        
        ActivitiesInforefXmlInforef.Serialize("label", LabelId);
    }
     
     /// <summary>
     /// Creates a label folder in the activity folder. Each activity needs an folder.
     /// </summary>
     /// <param name="moduleId"></param>
     private void CreateActivityFolder(string moduleId)
     {
         var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
         _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "label_"+moduleId));
     }
    
}