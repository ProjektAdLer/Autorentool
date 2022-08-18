using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities.Gradebook.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Groups.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Outcomes.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Questions.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Roles.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Scales.xml;

namespace AuthoringTool.DataAccess.XmlClasses;

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
    internal IGradebookXmlGradeCategory GradebookXmlGradeCategory;
    internal IGradebookXmlGradeCategories GradebookXmlGradeCategories;
    internal IGradebookXmlGradeItem GradebookXmlGradeItem;
    internal IGradebookXmlGradeItems GradebookXmlGradeItems;

    internal IGradebookXmlGradeSetting GradebookXmlGradebookSetting { get; }
    internal IGradebookXmlGradeSettings GradebookXmlGradebookSettings { get; }
    internal IGradebookXmlGradebook GradebookXmlGradebook { get; }
    internal IGroupsXmlGroupingsList GroupsXmlGroupingsList { get; }
    internal IGroupsXmlGroups GroupsXmlGroups { get; }
    internal IMoodleBackupXmlDetail MoodleBackupXmlDetail { get; }
    internal IMoodleBackupXmlDetails MoodleBackupXmlDetails { get; }
    internal IMoodleBackupXmlActivities MoodleBackupXmlActivities { get; }
    internal IMoodleBackupXmlSection MoodleBackupXmlSection { get; }
    internal IMoodleBackupXmlSections MoodleBackupXmlSections { get; }
    internal IMoodleBackupXmlCourse MoodleBackupXmlCourse { get; }
    internal IMoodleBackupXmlContents MoodleBackupXmlContents { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingFilename { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingImscc11 { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingUsers { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingAnonymize { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingRoleAssignments { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingActivities { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingBlocks { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingFiles { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingFilters { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingComments { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingBadges { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingCalendarevents { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingUserscompletion { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingLogs { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingGradeHistories { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingQuestionbank { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingGroups { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingCompetencies { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingCustomfield { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingContentbankcontent { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingLegacyfiles { get; }
    internal IMoodleBackupXmlSettings MoodleBackupXmlSettings { get; }
    internal IMoodleBackupXmlInformation MoodleBackupXmlInformation { get; }
    internal IMoodleBackupXmlMoodleBackup MoodleBackupXmlMoodleBackup { get; }
    internal IOutcomesXmlOutcomesDefinition OutcomesXmlOutcomesDefinition { get; }
    internal IQuestionsXmlQuestionsCategories QuestionsXmlQuestionsCategories { get; }
    internal IScalesXmlScalesDefinition ScalesXmlScalesDefinition { get; }
    internal IRolesXmlRole RolesXmlRole { get; }
    internal IRolesXmlRolesDefinition RolesXmlRolesDefinition{ get;}

    internal List<MoodleBackupXmlActivity> MoodleBackupXmlActivityList;
    internal List<MoodleBackupXmlSetting> MoodleBackupXmlSettingList;
    internal List<MoodleBackupXmlSection> MoodleBackupXmlSectionList;
    
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
        GradebookXmlGradeItems.GradeItem = GradebookXmlGradeItem as GradebookXmlGradeItem;
        
        GradebookXmlGradeCategory.Timecreated = _currentTime;
        GradebookXmlGradeCategory.Timemodified = _currentTime;
        GradebookXmlGradeCategories.GradeCategory = GradebookXmlGradeCategory as GradebookXmlGradeCategory;
        
        GradebookXmlGradebookSettings.GradeSetting = GradebookXmlGradebookSetting as GradebookXmlGradeSetting; 
        
        GradebookXmlGradebook.GradeCategories = GradebookXmlGradeCategories as GradebookXmlGradeCategories;
        GradebookXmlGradebook.GradeItems = GradebookXmlGradeItems as GradebookXmlGradeItems;
        GradebookXmlGradebook.GradeSettings = GradebookXmlGradebookSettings as GradebookXmlGradeSettings;
       
        //create the gradebook.xml file
        GradebookXmlGradebook.Serialize();
    }

    public void CreateGroupsXml()
    {
        //set the parameter of groups.xml file
        GroupsXmlGroups.GroupingsList = GroupsXmlGroupingsList as GroupsXmlGroupingsList;
        
        //create groups.xml file
        GroupsXmlGroups.Serialize();
    }

    public void CreateMoodleBackupXml()
    {
        //set the parameter of the moodle_backup.xml file
        MoodleBackupXmlDetails.Detail = MoodleBackupXmlDetail as MoodleBackupXmlDetail;


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

        /*
        if (learningElement != null)
            foreach (var element in learningElement)
            {
                string? learningElementId = element.id.ToString();
                string? learningElementType = element.elementType;
                string? learningElementName = element.identifier!.value;
                if (learningElementType == "h5p")
                {
                    learningElementType = "lesson";
                }

                if (moodleBackupXmlActivityList != null)
                {
                    moodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity());
                    moodleBackupXmlActivityList[^1].SetParameters(
                        learningElementId,
                        learningElementId, learningElementType,
                        learningElementName, "activities/" + learningElementType + "_" + learningElementId);
                    MoodleBackupXmlActivities.SetParameters(moodleBackupXmlActivityList);
                }

                if (moodleBackupXmlSectionList != null)
                {
                    moodleBackupXmlSectionList.Add(new MoodleBackupXmlSection());
                    moodleBackupXmlSectionList[^1].SetParameters(
                        learningElementId,
                        learningElementId, "sections/section_" + learningElementId);
                    MoodleBackupXmlSections.SetParameters(moodleBackupXmlSectionList);
                }

                if (moodleBackupXmlSettingList != null)
                {
                    moodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting());
                    moodleBackupXmlSettingList[^1]!.SetParametersSection(
                        "section",
                        "section_" + learningElementId,
                        "section_" + learningElementId + "_included", "1");
                    moodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting());
                    moodleBackupXmlSettingList[^1]!.SetParametersSection(
                        "section",
                        "section_" + learningElementId,
                        "section_" + learningElementId + "_userinfo", "0");
                    moodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting());
                    moodleBackupXmlSettingList[^1]!.SetParametersActivity(
                        "activity",
                        learningElementType + "_" + learningElementId,
                        learningElementType + "_" + learningElementId + "_included", "1");
                    moodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting());
                    moodleBackupXmlSettingList[^1]!.SetParametersActivity(
                        "activity",
                        learningElementType + "_" + learningElementId,
                        learningElementType + "_" + learningElementId + "_userinfo", "0");
                }
            }
        */

        MoodleBackupXmlContents.Activities = MoodleBackupXmlActivities as MoodleBackupXmlActivities;
        MoodleBackupXmlContents.Sections = MoodleBackupXmlSections as MoodleBackupXmlSections;
        MoodleBackupXmlContents.Course = MoodleBackupXmlCourse as MoodleBackupXmlCourse;

        if (MoodleBackupXmlSettingList != null)
        {
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilename as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingImscc11 as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUsers as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingAnonymize as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingRoleAssignments as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingActivities as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBlocks as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFiles as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilters as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingComments as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBadges as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCalendarevents as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUserscompletion as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLogs as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGradeHistories as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingQuestionbank as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGroups as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCompetencies as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCustomfield as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingContentbankcontent as MoodleBackupXmlSetting);
            MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLegacyfiles as MoodleBackupXmlSetting);

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
        MoodleBackupXmlInformation.Details = MoodleBackupXmlDetails as MoodleBackupXmlDetails;
        MoodleBackupXmlInformation.Contents = MoodleBackupXmlContents as MoodleBackupXmlContents;
        MoodleBackupXmlInformation.Settings = MoodleBackupXmlSettings as MoodleBackupXmlSettings;
                

        MoodleBackupXmlMoodleBackup.Information = MoodleBackupXmlInformation as MoodleBackupXmlInformation;
        
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
        RolesXmlRolesDefinition.Role = RolesXmlRole as RolesXmlRole;
        
        //create roles.xml file
        RolesXmlRolesDefinition.Serialize();
    }

    public void CreateScalesXml()
    {
        
        //create scales.xml file
        ScalesXmlScalesDefinition.Serialize();
    }
    
}
