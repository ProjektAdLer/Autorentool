using Generator.DSL;
using Generator.XmlClasses.Entities.Gradebook.xml;
using Generator.XmlClasses.Entities.Groups.xml;
using Generator.XmlClasses.Entities.MoodleBackup.xml;
using Generator.XmlClasses.Entities.Outcomes.xml;
using Generator.XmlClasses.Entities.Questions.xml;
using Generator.XmlClasses.Entities.Roles.xml;
using Generator.XmlClasses.Entities.Scales.xml;

namespace Generator.XmlClasses;

/// <summary>
/// sets the Parameter of gradebook.xml, groups.xml, outcomes.xml, questions.xml, scales.xml, roles.xml and
///  moodle_backup.xml files and create it
/// </summary>
public class XmlBackupFactory
{
    private readonly string _currentTime;
    private readonly LearningWorldJson _learningWorld;
    private readonly List<LearningElementJson> _learningElement;
    private readonly List<LearningElementJson> _dslDocument;
    public readonly IGradebookXmlGradeCategory GradebookXmlGradeCategory;
    public readonly IGradebookXmlGradeCategories GradebookXmlGradeCategories;
    public readonly IGradebookXmlGradeItem GradebookXmlGradeItem;
    public readonly IGradebookXmlGradeItems GradebookXmlGradeItems;

    public IGradebookXmlGradeSetting GradebookXmlGradebookSetting { get; }
    public IGradebookXmlGradeSettings GradebookXmlGradebookSettings { get; }
    public IGradebookXmlGradebook GradebookXmlGradebook { get; }
    public IGroupsXmlGroupingsList GroupsXmlGroupingsList { get; }
    public IGroupsXmlGroups GroupsXmlGroups { get; }
    public IMoodleBackupXmlDetail MoodleBackupXmlDetail { get; }
    public IMoodleBackupXmlDetails MoodleBackupXmlDetails { get; }
    public IMoodleBackupXmlActivities MoodleBackupXmlActivities { get; }
    public IMoodleBackupXmlSection MoodleBackupXmlSection { get; }
    public IMoodleBackupXmlSections MoodleBackupXmlSections { get; }
    public IMoodleBackupXmlCourse MoodleBackupXmlCourse { get; }
    public IMoodleBackupXmlContents MoodleBackupXmlContents { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingFilename { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingImscc11 { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingUsers { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingAnonymize { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingRoleAssignments { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingActivities { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingBlocks { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingFiles { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingFilters { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingComments { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingBadges { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingCalendarevents { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingUserscompletion { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingLogs { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingGradeHistories { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingQuestionbank { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingGroups { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingCompetencies { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingCustomfield { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingContentbankcontent { get; }
    public IMoodleBackupXmlSetting MoodleBackupXmlSettingLegacyfiles { get; }
    public IMoodleBackupXmlSettings MoodleBackupXmlSettings { get; }
    public IMoodleBackupXmlInformation MoodleBackupXmlInformation { get; }
    public IMoodleBackupXmlMoodleBackup MoodleBackupXmlMoodleBackup { get; }
    public IOutcomesXmlOutcomesDefinition OutcomesXmlOutcomesDefinition { get; }
    public IQuestionsXmlQuestionsCategories QuestionsXmlQuestionsCategories { get; }
    public IScalesXmlScalesDefinition ScalesXmlScalesDefinition { get; }
    public IRolesXmlRole RolesXmlRole { get; }
    public IRolesXmlRolesDefinition RolesXmlRolesDefinition{ get;}

    public readonly List<MoodleBackupXmlActivity> MoodleBackupXmlActivityList;
    public readonly List<MoodleBackupXmlSetting> MoodleBackupXmlSettingList;
    public readonly List<MoodleBackupXmlSection> MoodleBackupXmlSectionList;
    
    public XmlBackupFactory(IReadDsl readDsl, IGradebookXmlGradeItem? gradebookXmlGradeItem=null,
        IGradebookXmlGradeItems? gradebookXmlGradeItems=null,IGradebookXmlGradeCategory? gradebookXmlGradeCategory=null,
        IGradebookXmlGradeCategories? gradebookXmlGradeCategories=null, IGradebookXmlGradeSetting? gradebookXmlGradebookSetting=null, 
        IGradebookXmlGradeSettings? gradebookXmlGradebookSettings=null,IGradebookXmlGradebook? gradebookXmlGradebook=null,
        IGroupsXmlGroupingsList? groupsXmlGroupingsList=null, IGroupsXmlGroups? groupsXmlGroups=null,
        IMoodleBackupXmlDetail? moodleBackupXmlDetail=null, IMoodleBackupXmlDetails? moodleBackupXmlDetails=null,
        IMoodleBackupXmlActivities? moodleBackupXmlActivities=null, IMoodleBackupXmlSection? moodleBackupXmlSection=null, 
        IMoodleBackupXmlSections? moodleBackupXmlSections=null, IMoodleBackupXmlCourse? moodleBackupXmlCourse=null, 
        IMoodleBackupXmlContents? moodleBackupXmlContents=null, IMoodleBackupXmlSetting? moodleBackupXmlSetting=null, 
        IMoodleBackupXmlInformation? moodleBackupXmlInformation=null, IMoodleBackupXmlMoodleBackup? moodleBackupXmlMoodleBackup=null,
        IOutcomesXmlOutcomesDefinition? outcomesXmlOutcomesDefinition=null, IQuestionsXmlQuestionsCategories? questionsXmlQuestionsCategories=null,
        IRolesXmlRole? rolesXmlRole=null, IRolesXmlRolesDefinition? rolesXmlRolesDefinition=null, IScalesXmlScalesDefinition? scalesXmlScalesDefinition=null)
    {
    
        GradebookXmlGradeItem = gradebookXmlGradeItem?? new GradebookXmlGradeItem();
        GradebookXmlGradeItems = gradebookXmlGradeItems?? new GradebookXmlGradeItems();
        GradebookXmlGradeCategory = gradebookXmlGradeCategory?? new GradebookXmlGradeCategory();
        GradebookXmlGradeCategories = gradebookXmlGradeCategories?? new GradebookXmlGradeCategories();
        GradebookXmlGradebookSetting = gradebookXmlGradebookSetting?? new GradebookXmlGradeSetting();
        GradebookXmlGradebookSettings = gradebookXmlGradebookSettings?? new GradebookXmlGradeSettings();
        GradebookXmlGradebook = gradebookXmlGradebook?? new GradebookXmlGradebook();

        GroupsXmlGroupingsList = groupsXmlGroupingsList?? new GroupsXmlGroupingsList();
        GroupsXmlGroups = groupsXmlGroups?? new GroupsXmlGroups();

        MoodleBackupXmlDetail = moodleBackupXmlDetail?? new MoodleBackupXmlDetail();
        MoodleBackupXmlDetails = moodleBackupXmlDetails?? new MoodleBackupXmlDetails();
        
        MoodleBackupXmlActivities = moodleBackupXmlActivities?? new MoodleBackupXmlActivities();
        
        MoodleBackupXmlSection = moodleBackupXmlSection?? new MoodleBackupXmlSection();
        MoodleBackupXmlSections = moodleBackupXmlSections?? new MoodleBackupXmlSections();
        MoodleBackupXmlCourse = moodleBackupXmlCourse?? new MoodleBackupXmlCourse();
        MoodleBackupXmlContents = moodleBackupXmlContents?? new MoodleBackupXmlContents();
        
        MoodleBackupXmlSettingFilename = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingImscc11 = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingUsers = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingAnonymize =  moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingRoleAssignments =  moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingActivities =  moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingBlocks = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingFiles = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingFilters = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingComments = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingBadges = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCalendarevents = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingUserscompletion = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingLogs = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingGradeHistories = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingQuestionbank = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingGroups = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCompetencies = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCustomfield = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingContentbankcontent = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingLegacyfiles = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();

        MoodleBackupXmlSettings =  new MoodleBackupXmlSettings();

        MoodleBackupXmlInformation = moodleBackupXmlInformation?? new MoodleBackupXmlInformation();
        MoodleBackupXmlMoodleBackup = moodleBackupXmlMoodleBackup?? new MoodleBackupXmlMoodleBackup();

        OutcomesXmlOutcomesDefinition = outcomesXmlOutcomesDefinition?? new OutcomesXmlOutcomesDefinition();

        QuestionsXmlQuestionsCategories = questionsXmlQuestionsCategories?? new QuestionsXmlQuestionsCategories();

        RolesXmlRole = rolesXmlRole?? new RolesXmlRole();
        RolesXmlRolesDefinition = rolesXmlRolesDefinition?? new RolesXmlRolesDefinition();

        ScalesXmlScalesDefinition = scalesXmlScalesDefinition?? new ScalesXmlScalesDefinition();
        
        _currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        
        _learningWorld = readDsl.GetLearningWorld();
        _learningElement = readDsl.GetH5PElementsList();
        _dslDocument = readDsl.GetDslDocumentList();
        MoodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        MoodleBackupXmlSettingList = new List<MoodleBackupXmlSetting>();
        MoodleBackupXmlSectionList = new List<MoodleBackupXmlSection>();
    }

    /// <summary>
    /// Use all Methods of the current class.
    /// </summary>
    public void CreateXmlBackupFactory()
    {
        //Set parameter and create gradebook.xml
        CreateGradebookXml();
        
        //Set parameter and create groups.xml
        CreateGroupsXml();
        
        //Set parameter and create outcomes.xml
        CreateOutcomesXml();
        
        //Set parameter and create questions.xml
        CreateQuestionsXml();
        
        //Set parameter and create roles.xml
        CreateRolesXml();
        
        //Set parameter and create scales.xml
        CreateScalesXml();
        
        //Set parameter and create moodle_backup.xml
        CreateMoodleBackupXml();
    }
    

    /// <summary>
    /// Set the parameter of the gradebook.xml file and create it.
    /// </summary>
    public void CreateGradebookXml()
    {
        //set the parameter of gradebook.xml file
        GradebookXmlGradeItem.Timecreated = _currentTime;
        GradebookXmlGradeItem.Timemodified = _currentTime;
        GradebookXmlGradeItems.GradeItem = GradebookXmlGradeItem as GradebookXmlGradeItem ?? new GradebookXmlGradeItem();
        
        GradebookXmlGradeCategory.Timecreated = _currentTime;
        GradebookXmlGradeCategory.Timemodified = _currentTime;
        GradebookXmlGradeCategories.GradeCategory = GradebookXmlGradeCategory as GradebookXmlGradeCategory ?? new GradebookXmlGradeCategory();
        
        GradebookXmlGradebookSettings.GradeSetting = GradebookXmlGradebookSetting as GradebookXmlGradeSetting ?? new GradebookXmlGradeSetting(); 
        
        GradebookXmlGradebook.GradeCategories = GradebookXmlGradeCategories as GradebookXmlGradeCategories ?? new GradebookXmlGradeCategories();
        GradebookXmlGradebook.GradeItems = GradebookXmlGradeItems as GradebookXmlGradeItems ?? new GradebookXmlGradeItems();
        GradebookXmlGradebook.GradeSettings = GradebookXmlGradebookSettings as GradebookXmlGradeSettings ?? new GradebookXmlGradeSettings();
       
        //create the gradebook.xml file
        GradebookXmlGradebook.Serialize();
    }

    public void CreateGroupsXml()
    {
        //set the parameter of groups.xml file
        GroupsXmlGroups.GroupingsList = GroupsXmlGroupingsList as GroupsXmlGroupingsList ?? new GroupsXmlGroupingsList();
        
        //create groups.xml file
        GroupsXmlGroups.Serialize();
    }

    public void CreateMoodleBackupXml()
    {
        //set the parameter of the moodle_backup.xml file
        MoodleBackupXmlDetails.Detail = MoodleBackupXmlDetail as MoodleBackupXmlDetail ?? new MoodleBackupXmlDetail();


        MoodleBackupXmlCourse.Title = _learningWorld.Identifier.Value;

        //MoodleBackupXmlSettingSetting are Tags that describe the Moodle Backup Settings.
        //They are the same Options that are displayed, when a backup is created in moodle. 
        //Its very important, that files & activities are imported with the backup (value=1)
        MoodleBackupXmlSettingFilename.Name = "filename";
        MoodleBackupXmlSettingFilename.Value = "C#_AuthoringTool_Created_Backup.mbz";

        MoodleBackupXmlSettingImscc11.Name = "imscc11";

        MoodleBackupXmlSettingUsers.Name = "users";

        MoodleBackupXmlSettingAnonymize.Name = "anonymize";

        MoodleBackupXmlSettingRoleAssignments.Name = "role_assignments";

        MoodleBackupXmlSettingActivities.Name = "activities";
        MoodleBackupXmlSettingActivities.Value = "1";

        MoodleBackupXmlSettingBlocks.Name = "blocks";

        MoodleBackupXmlSettingFiles.Name = "files";
        MoodleBackupXmlSettingFiles.Value = "1";

        MoodleBackupXmlSettingFilters.Name = "filters";

        MoodleBackupXmlSettingComments.Name = "comments";

        MoodleBackupXmlSettingBadges.Name = "badges";

        MoodleBackupXmlSettingCalendarevents.Name = "calendarevents";

        MoodleBackupXmlSettingUserscompletion.Name = "userscompletion";

        MoodleBackupXmlSettingLogs.Name = "logs";

        MoodleBackupXmlSettingGradeHistories.Name = "grade_histories";

        MoodleBackupXmlSettingQuestionbank.Name = "questionbank";

        MoodleBackupXmlSettingGroups.Name = "groups";

        MoodleBackupXmlSettingCompetencies.Name = "competencies";

        MoodleBackupXmlSettingCustomfield.Name = "customfield";

        MoodleBackupXmlSettingContentbankcontent.Name = "contentbankcontent";

        MoodleBackupXmlSettingLegacyfiles.Name = "legacyfiles";

        //create Tags for the DSL Document
        //Documentpath and name is hardcoded
       foreach (var document in _dslDocument)
       {
           string dslDocumentId = document.Id.ToString();
           string dslDocumentType = "resource";
           string dslDocumentName = document.Identifier.Value;

           if (MoodleBackupXmlActivityList != null)
           {
               MoodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity());
               MoodleBackupXmlActivityList[^1].ModuleId = dslDocumentId;
               MoodleBackupXmlActivityList[^1].SectionId = dslDocumentId;
               MoodleBackupXmlActivityList[^1].ModuleName = dslDocumentType;
               MoodleBackupXmlActivityList[^1].Title = dslDocumentName;
               MoodleBackupXmlActivityList[^1].Directory = "activities/" + dslDocumentType + "_" + dslDocumentId;

               MoodleBackupXmlActivities.Activity = MoodleBackupXmlActivityList;
           }

           if (MoodleBackupXmlSectionList != null)
           {
               MoodleBackupXmlSectionList.Add(new MoodleBackupXmlSection());
               MoodleBackupXmlSectionList[^1].SectionId = dslDocumentId;
               MoodleBackupXmlSectionList[^1].Title = dslDocumentId;
               MoodleBackupXmlSectionList[^1].Directory = "sections/section_" + dslDocumentId;

               MoodleBackupXmlSections.Section = MoodleBackupXmlSectionList;
           }

           if (MoodleBackupXmlSettingList != null)
           {
               MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                   "section_" + dslDocumentId + "_included", "1", "section_" + dslDocumentId, true));
               MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                   "section_" + dslDocumentId + "_userinfo", "0", "section_" + dslDocumentId, true));

               MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("activity",
                   dslDocumentType + "_" + dslDocumentId + "_included", "1",
                   dslDocumentType + "_" + dslDocumentId, false));
               MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("activity",
                   dslDocumentType + "_" + dslDocumentId + "_userinfo", "0",
                   dslDocumentType + "_" + dslDocumentId, false));
           }
       }

        //Every activity needs the following tags in the moodle_backup.xml file
        //The ElementType will be changed for other element types
        foreach (var element in _learningElement)
        {
            string learningElementId = element.Id.ToString();
            string learningElementType = element.ElementType;
            string learningElementName = element.Identifier.Value;
            if (learningElementType == "h5p")
            {
                learningElementType = "h5pactivity";
            }

            if (MoodleBackupXmlActivityList != null)
            {
                MoodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity());
                MoodleBackupXmlActivityList[^1].ModuleId = learningElementId;
                MoodleBackupXmlActivityList[^1].SectionId = learningElementId;
                MoodleBackupXmlActivityList[^1].ModuleName = learningElementType;
                MoodleBackupXmlActivityList[^1].Title = learningElementName;
                MoodleBackupXmlActivityList[^1].Directory =
                    "activities/" + learningElementType + "_" + learningElementId;

                MoodleBackupXmlActivities.Activity = MoodleBackupXmlActivityList;
            }

            if (MoodleBackupXmlSectionList != null)
            {
                MoodleBackupXmlSectionList.Add(new MoodleBackupXmlSection());
                MoodleBackupXmlSectionList[^1].SectionId = learningElementId;
                MoodleBackupXmlSectionList[^1].Title = learningElementId;
                MoodleBackupXmlSectionList[^1].Directory = "sections/section_" + learningElementId;

                MoodleBackupXmlSections.Section = MoodleBackupXmlSectionList;
            }

            if (MoodleBackupXmlSettingList != null)
            {
                MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                    "section_" + learningElementId + "_included", "1",
                    "section_" + learningElementId, true));

                MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                    "section_" + learningElementId + "_userinfo", "0",
                    "section_" + learningElementId, true));


                MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("activity",
                    learningElementType + "_" + learningElementId + "_included", "1",
                    learningElementType + "_" + learningElementId, false));

                MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("activity",
                    learningElementType + "_" + learningElementId + "_userinfo", "0",
                    learningElementType + "_" + learningElementId, false));
            }
        }

        MoodleBackupXmlContents.Activities = MoodleBackupXmlActivities as MoodleBackupXmlActivities ?? new MoodleBackupXmlActivities();
        MoodleBackupXmlContents.Sections = MoodleBackupXmlSections as MoodleBackupXmlSections ?? new MoodleBackupXmlSections();
        MoodleBackupXmlContents.Course = MoodleBackupXmlCourse as MoodleBackupXmlCourse ?? new MoodleBackupXmlCourse();

        if (MoodleBackupXmlSettingList != null)
        {
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilename as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingImscc11 as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUsers as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingAnonymize as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingRoleAssignments as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingActivities as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBlocks as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFiles as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilters as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingComments as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBadges as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCalendarevents as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUserscompletion as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLogs as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGradeHistories as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingQuestionbank as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGroups as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCompetencies as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCustomfield as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingContentbankcontent as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLegacyfiles as MoodleBackupXmlSetting ?? new MoodleBackupXmlSetting());

            MoodleBackupXmlSettings.Setting = MoodleBackupXmlSettingList;
        }

        //Base information about the moodle_backup.xml file. The value in originalCourseFullName & 
        //OriginalCourseShortName will be displayed, when the .mbz file is uploaded to moodle.
        //Its important that the parameter "includeFiles" is set to "1".
        //With the OriginalCourseFormat the future learningWorld can be individualised
        MoodleBackupXmlInformation.BackupDate = _currentTime;
        MoodleBackupXmlInformation.OriginalCourseFullname = _learningWorld.Identifier.Value;
        MoodleBackupXmlInformation.OriginalCourseShortname = _learningWorld.Identifier.Value;
        MoodleBackupXmlInformation.OriginalCourseStartDate = _currentTime;
        MoodleBackupXmlInformation.Details = MoodleBackupXmlDetails as MoodleBackupXmlDetails ?? new MoodleBackupXmlDetails();
        MoodleBackupXmlInformation.Contents = MoodleBackupXmlContents as MoodleBackupXmlContents ?? new MoodleBackupXmlContents();
        MoodleBackupXmlInformation.Settings = MoodleBackupXmlSettings as MoodleBackupXmlSettings ?? new MoodleBackupXmlSettings();
                

        MoodleBackupXmlMoodleBackup.Information = MoodleBackupXmlInformation as MoodleBackupXmlInformation ?? new MoodleBackupXmlInformation();
        
        //create moodle_backup.xml file
        MoodleBackupXmlMoodleBackup.Serialize();
    }

    public void CreateOutcomesXml()
    {
       
        //create outcomes.xml file
        OutcomesXmlOutcomesDefinition.Serialize();
    }

    public void CreateQuestionsXml()
    {
        
        //create questions.xml file
        QuestionsXmlQuestionsCategories.Serialize();
    }

    public void CreateRolesXml()
    {
        //set parameters of the roles.xml file
        RolesXmlRolesDefinition.Role = RolesXmlRole as RolesXmlRole ?? new RolesXmlRole();
        
        //create roles.xml file
        RolesXmlRolesDefinition.Serialize();
    }

    public void CreateScalesXml()
    {
        
        //create scales.xml file
        ScalesXmlScalesDefinition.Serialize();
    }
    
}
