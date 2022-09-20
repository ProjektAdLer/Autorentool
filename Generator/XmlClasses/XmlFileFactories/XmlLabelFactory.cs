﻿using System.IO.Abstractions;
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
    
    public void CreateLabelFactory()
    {
        var labelList = ReadDsl.GetSpacesAndElementsOrderedList();

        LabelSetParameters(labelList);
    }

    public void LabelSetParameters(List<LearningElementJson> labelList)
    {
        foreach (var label in labelList)
        {
            LabelId = label.Id.ToString();
            LabelName = label.Identifier.Value;
            LabelParentSpaceId = label.LearningSpaceParentId.ToString();
            LabelDescription = label.Description ?? "";
            
            //A Space got another Font in his Name.
            if (label.ElementType is "space")
            {
                FileSetParametersActivitySpace();
            }
        }
    }

    public void FileSetParametersActivitySpace()
    {
        CreateActivityFolder(LabelId);
        
        //file activities/label.../grades.xml
        ActivitiesGradesXmlGradeItems.GradeItem = ActivitiesGradesXmlGradeItem as ActivitiesGradesXmlGradeItem ?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlActivityGradebook.GradeItems = ActivitiesGradesXmlGradeItems as ActivitiesGradesXmlGradeItems ?? new ActivitiesGradesXmlGradeItems();

        ActivitiesGradesXmlActivityGradebook.Serialize("label", LabelId);
        
        //file activities/label.../label.xml
        ActivitiesLabelXmlLabel.Name = "<h4>"+LabelName+"</h4>"+"<p>&nbsp; &nbsp; &nbsp; "+LabelDescription+"</p>";
        ActivitiesLabelXmlLabel.Id = LabelId;
        ActivitiesLabelXmlLabel.Intro = "<h4>"+LabelName+"</h4>"+"<p>&nbsp; &nbsp; &nbsp; "+LabelDescription+"</p>";
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
     public void CreateActivityFolder(string moduleId)
     {
         var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
         _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "label_"+moduleId));
     }
    
}