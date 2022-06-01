using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities;

namespace AuthoringTool.DataAccess.XmlClasses;

/// <summary>
/// sets the Parameter of gradebook.xml, groups.xml, outcomes.xml, questions.xml, scales.xml, roles.xml and
///  moodle_backup.xml files and create it
/// </summary>
public class XmlBackupFactory
{
    private string? currentTime;
    private LearningWorldJson? learningWorld;
    private List<LearningElementJson>? learningElement;
    internal IGradebookXmlGradeCategory GradebookXmlGradeCategory = null!;
    internal IGradebookXmlGradeCategories GradebookXmlGradeCategories = null!;
    internal IGradebookXmlGradeItem GradebookXmlGradeItem = null!;
    internal IGradebookXmlGradeItems GradebookXmlGradeItems = null!;

    internal IGradebookXmlGradeSetting GradebookXmlGradebookSetting { get; }
    internal IGradebookXmlGradeSettings GradebookXmlGradebookSettings { get; }
    internal IGradebookXmlGradebook GradebookXmlGradebook { get; }
    internal IGroupsXmlGroupingsList GroupsXmlGroupingsList { get; }
    internal IGroupsXmlGroups GroupsXmlGroups { get; }
    internal IMoodleBackupXmlDetail MoodleBackupXmlDetail { get; }
    internal IMoodleBackupXmlDetails MoodleBackupXmlDetails { get; }
    internal IMoodleBackupXmlActivities MoodleBackupXmlActivities { get; } = null!;
    internal IMoodleBackupXmlActivity MoodleBackupXmlActivity { get; } = null!;
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
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingSection_included { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingSection_userinfo { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingActivity_included { get; } = null!;
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingActivity_userinfo { get; } = null!;

    internal IMoodleBackupXmlSettings MoodleBackupXmlSettings { get; }
    internal IMoodleBackupXmlInformation MoodleBackupXmlInformation { get; }
    internal IMoodleBackupXmlMoodleBackup MoodleBackupXmlMoodleBackup { get; }
    internal IOutcomesXmlOutcomesDefinition OutcomesXmlOutcomesDefinition { get; }
    internal IQuestionsXmlQuestionsCategories QuestionsXmlQuestionsCategories { get; }
    internal IScalesXmlScalesDefinition ScalesXmlScalesDefinition { get; }
    internal IRolesXmlRole RolesXmlRole { get; }
    internal IRolesXmlRolesDefinition RolesXmlRolesDefinition{ get;}
    internal IReadDSL? ReadDsl { get; }
    internal List<MoodleBackupXmlActivity>? moodleBackupXmlActivityList;
    internal List<MoodleBackupXmlSetting?>? moodleBackupXmlSettingList = null!;
    internal List<MoodleBackupXmlSection>? moodleBackupXmlSectionList = null!;

/*
    public XmlBackupFactory(ReadDSL? readDsl)
    {
        GradebookXmlGradeItem = new GradebookXmlGradeItem();
        GradebookXmlGradeItems = new GradebookXmlGradeItems();
        GradebookXmlGradeCategory = new GradebookXmlGradeCategory();
        GradebookXmlGradeCategories = new GradebookXmlGradeCategories();
        GradebookXmlGradebookSetting = new GradebookXmlGradeSetting();
        GradebookXmlGradebookSettings = new GradebookXmlGradeSettings();
        GradebookXmlGradebook = new GradebookXmlGradebook();
        
        GroupsXmlGroupingsList = new GroupsXmlGroupingsList();
        GroupsXmlGroups = new GroupsXmlGroups();

        MoodleBackupXmlDetail = new MoodleBackupXmlDetail();
        MoodleBackupXmlDetails = new MoodleBackupXmlDetails();

        MoodleBackupXmlActivities = new MoodleBackupXmlActivities();
        MoodleBackupXmlActivity = new MoodleBackupXmlActivity();
        
        MoodleBackupXmlSection = new MoodleBackupXmlSection();
        MoodleBackupXmlSections = new MoodleBackupXmlSections();
        MoodleBackupXmlCourse = new MoodleBackupXmlCourse();
        MoodleBackupXmlContents = new MoodleBackupXmlContents();
        MoodleBackupXmlSettingFilename = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingImscc11 = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingUsers = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingAnonymize = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingRoleAssignments = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingActivities = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingBlocks = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingFiles = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingFilters = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingComments = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingBadges = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCalendarevents = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingUserscompletion = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingLogs = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingGradeHistories = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingQuestionbank = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingGroups = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCompetencies = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCustomfield = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingContentbankcontent = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingLegacyfiles = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingSection_included = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingSection_userinfo = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingActivity_included = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingActivity_userinfo = new MoodleBackupXmlSetting();
        
        MoodleBackupXmlSettings = new MoodleBackupXmlSettings();
        MoodleBackupXmlInformation = new MoodleBackupXmlInformation();
        MoodleBackupXmlMoodleBackup = new MoodleBackupXmlMoodleBackup();

        OutcomesXmlOutcomesDefinition = new OutcomesXmlOutcomesDefinition();

        QuestionsXmlQuestionsCategories = new QuestionsXmlQuestionsCategories();
        
        RolesXmlRole = new RolesXmlRole();
        RolesXmlRolesDefinition = new RolesXmlRolesDefinition();
        
        ScalesXmlScalesDefinition = new ScalesXmlScalesDefinition();
        
        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        ReadDsl = readDsl;
        learningWorld = ReadDsl!.GetLearningWorld();
        learningElement = ReadDsl.GetH5PElementsList();
        moodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        moodleBackupXmlSettingList = new List<MoodleBackupXmlSetting?>();
        moodleBackupXmlSectionList = new List<MoodleBackupXmlSection>();
    }*/

    public XmlBackupFactory(IReadDSL readDsl, IGradebookXmlGradeItem? gradebookXmlGradeItem=null,
        IGradebookXmlGradeItems? gradebookXmlGradeItems=null,IGradebookXmlGradeCategory? gradebookXmlGradeCategory=null,
        IGradebookXmlGradeCategories? gradebookXmlGradeCategories=null, IGradebookXmlGradeSetting? gradebookXmlGradebookSetting=null, 
        IGradebookXmlGradeSettings? gradebookXmlGradebookSettings=null,IGradebookXmlGradebook? gradebookXmlGradebook=null,
        IGroupsXmlGroupingsList? groupsXmlGroupingsList=null, IGroupsXmlGroups? groupsXmlGroups=null,
        IMoodleBackupXmlDetail? moodleBackupXmlDetail=null, IMoodleBackupXmlDetails? moodleBackupXmlDetails=null,
        IMoodleBackupXmlActivities? moodleBackupXmlActivities=null, IMoodleBackupXmlActivity? moodleBackupXmlActivity=null,
        IMoodleBackupXmlSection? moodleBackupXmlSection=null, IMoodleBackupXmlSections? moodleBackupXmlSections=null, 
        IMoodleBackupXmlCourse? moodleBackupXmlCourse=null, IMoodleBackupXmlContents? moodleBackupXmlContents=null,
        IMoodleBackupXmlSetting? moodleBackupXmlSetting=null, IMoodleBackupXmlSettings? moodleBackupXmlSettings=null, 
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
        MoodleBackupXmlActivity = moodleBackupXmlActivity?? new MoodleBackupXmlActivity();
        
        MoodleBackupXmlSection = moodleBackupXmlSection?? new MoodleBackupXmlSection();
        MoodleBackupXmlSections = moodleBackupXmlSections?? new MoodleBackupXmlSections();
        MoodleBackupXmlCourse = moodleBackupXmlCourse?? new MoodleBackupXmlCourse();
        MoodleBackupXmlContents = moodleBackupXmlContents?? new MoodleBackupXmlContents();
        
        MoodleBackupXmlSettingFilename = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingImscc11 = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingUsers = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingAnonymize = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingRoleAssignments = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingActivities = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
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
        MoodleBackupXmlSettingSection_included = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingSection_userinfo = moodleBackupXmlSetting?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettings = moodleBackupXmlSettings?? new MoodleBackupXmlSettings();

        MoodleBackupXmlInformation = moodleBackupXmlInformation?? new MoodleBackupXmlInformation();
        MoodleBackupXmlMoodleBackup = moodleBackupXmlMoodleBackup?? new MoodleBackupXmlMoodleBackup();

        OutcomesXmlOutcomesDefinition = outcomesXmlOutcomesDefinition?? new OutcomesXmlOutcomesDefinition();

        QuestionsXmlQuestionsCategories = questionsXmlQuestionsCategories?? new QuestionsXmlQuestionsCategories();

        RolesXmlRole = rolesXmlRole?? new RolesXmlRole();
        RolesXmlRolesDefinition = rolesXmlRolesDefinition?? new RolesXmlRolesDefinition();

        ScalesXmlScalesDefinition = scalesXmlScalesDefinition?? new ScalesXmlScalesDefinition();
        
        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        
        ReadDsl = readDsl;
        learningWorld = readDsl.GetLearningWorld();
        learningElement = readDsl.GetH5PElementsList();
        moodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        moodleBackupXmlSettingList = new List<MoodleBackupXmlSetting?>();
        moodleBackupXmlSectionList = new List<MoodleBackupXmlSection>();
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
        GradebookXmlGradeItem.SetParameters("$@NULL@$", "$@NULL@$", "course",
            "$@NULL@$", "1", "$@NULL@$", "$@NULL@$",
            "$@NULL@$", "$@NULL@$", "1", "100.00000", "0.00000",
            "$@NULL@$", "$@NULL@$", "0.00000", "1.00000",
            "0.00000", "0.00000", "0.00000","0",
            "1", "0", "$@NULL@$", "0", "0", "0",
            "0", currentTime, currentTime, "", "1");
        GradebookXmlGradeItems.SetParameters(GradebookXmlGradeItem as GradebookXmlGradeItem);
        GradebookXmlGradeCategory.SetParameters("$@NULL@$", "1", "/1/", "?", "13",
            "0", "0", "1", "0", currentTime, currentTime,
            "0", "1");
        GradebookXmlGradeCategories.SetParameters(GradebookXmlGradeCategory as GradebookXmlGradeCategory);
        GradebookXmlGradebookSetting.SetParameters("minmaxtouse", "1", "");
        GradebookXmlGradebookSettings.SetParameters(GradebookXmlGradebookSetting as GradebookXmlGradeSetting) ; 
        GradebookXmlGradebook.SetParameters("",GradebookXmlGradeCategories as GradebookXmlGradeCategories,
            GradebookXmlGradeItems as GradebookXmlGradeItems,"",GradebookXmlGradebookSettings as GradebookXmlGradeSettings);
        
        //create the gradebook.xml file
        GradebookXmlGradebook.Serialize();
    }

    public void CreateGroupsXml()
    {
        //set the parameter of groups.xml file
        GroupsXmlGroupingsList.SetParameters("");
        GroupsXmlGroups.SetParameters(GroupsXmlGroupingsList as GroupsXmlGroupingsList);
        
        //create groups.xml file
        GroupsXmlGroups.Serialize();
    }

    public void CreateMoodleBackupXml()
    {
        //set the parameter of the moodle_backup.xml file
        MoodleBackupXmlDetail.SetParameters("course", "moodle2", "1",
            "10", "1", "0", "36d63c7b4624cf6a79e0405be770974d");
        MoodleBackupXmlDetails.SetParameters(MoodleBackupXmlDetail as MoodleBackupXmlDetail);
        
        if (learningWorld != null)
        {
            if (learningWorld.identifier != null)
            {
                MoodleBackupXmlCourse.SetParameters("1", learningWorld.identifier.value, "course");

                //MoodleBackupXmlSetting are Tags that describe the Moodle Backup Settings.
                //Its the same Options that are displayed, when a backup is created in moodle. 
                //Its very important, that files & activities are imported with the backup (value=1)
                MoodleBackupXmlSettingFilename.SetParametersSetting("root", "filename",
                    "C#_AuthoringTool_Created_Backup.mbz");
                MoodleBackupXmlSettingImscc11.SetParametersSetting("root", "imscc11", "0");
                MoodleBackupXmlSettingUsers.SetParametersSetting("root", "users", "0");
                MoodleBackupXmlSettingAnonymize.SetParametersSetting("root", "anonymize", "0");
                MoodleBackupXmlSettingRoleAssignments.SetParametersSetting("root", "role_assignments", "0");
                MoodleBackupXmlSettingActivities.SetParametersSetting("root", "activities", "1");
                MoodleBackupXmlSettingBlocks.SetParametersSetting("root", "blocks", "0");
                MoodleBackupXmlSettingFiles.SetParametersSetting("root", "files", "1");
                MoodleBackupXmlSettingFilters.SetParametersSetting("root", "filters", "0");
                MoodleBackupXmlSettingComments.SetParametersSetting("root", "comments", "0");
                MoodleBackupXmlSettingBadges.SetParametersSetting("root", "badges", "0");
                MoodleBackupXmlSettingCalendarevents.SetParametersSetting("root", "calendarevents", "0");
                MoodleBackupXmlSettingUserscompletion.SetParametersSetting("root", "userscompletion", "0");
                MoodleBackupXmlSettingLogs.SetParametersSetting("root", "logs", "0");
                MoodleBackupXmlSettingGradeHistories.SetParametersSetting("root", "grade_histories", "0");
                MoodleBackupXmlSettingQuestionbank.SetParametersSetting("root", "questionbank", "0");
                MoodleBackupXmlSettingGroups.SetParametersSetting("root", "groups", "0");
                MoodleBackupXmlSettingCompetencies.SetParametersSetting("root", "competencies", "0");
                MoodleBackupXmlSettingCustomfield.SetParametersSetting("root", "customfield", "0");
                MoodleBackupXmlSettingContentbankcontent.SetParametersSetting("root", "contentbankcontent", "0");
                MoodleBackupXmlSettingLegacyfiles.SetParametersSetting("root", "legacyfiles", "0");

                //Every activity needs the following tags in the moodle_backup.xml file
                //The ElementType will be changed for other element types
                if (learningElement != null)
                    foreach (var element in learningElement)
                    {
                        string? learningElementId = element.id.ToString();
                        string? learningElementType = element.elementType;
                        string? learningElementName = element.identifier!.value;
                        if (learningElementType == "H5P")
                        {
                            learningElementType = "h5pactivity";
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

                MoodleBackupXmlContents.SetParameters(MoodleBackupXmlActivities as MoodleBackupXmlActivities,
                    MoodleBackupXmlSections as MoodleBackupXmlSections, MoodleBackupXmlCourse as MoodleBackupXmlCourse);

                if (moodleBackupXmlSettingList != null)
                {
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilename as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingImscc11 as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUsers as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingAnonymize as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingRoleAssignments as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingActivities as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBlocks as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFiles as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilters as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingComments as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBadges as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCalendarevents as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUserscompletion as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLogs as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGradeHistories as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingQuestionbank as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGroups as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCompetencies as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCustomfield as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingContentbankcontent as MoodleBackupXmlSetting);
                    moodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLegacyfiles as MoodleBackupXmlSetting);

                    MoodleBackupXmlSettings.SetParameters(moodleBackupXmlSettingList);
                }

                //Base information about the moodle_backup.xml file. The value in originalCourseFullName & 
                //OriginalCourseShortName will be displayed, when the .mbz file is uploaded to moodle.
                //Its important that the paramter "includeFiles" is set to "1".
                //With the OriginalCourseFormat the future learningWorld can be individualised
                MoodleBackupXmlInformation.SetParameters("C#_AuthoringTool_Created_Backup.mbz", "2021051703",
                    "3.11.3 (Build: 20210913)", "2021051700", "3.11", currentTime, "0",
                    "1", "0", "https://moodle.cluuub.xyz",
                    "c9629ccd3c092478330b78bdf4dcdb18", "1", "topics",
                    learningWorld.identifier.value!.ToString(), learningWorld.identifier.value.ToString(),
                    currentTime, "2221567452", "1", "1",
                    MoodleBackupXmlDetails as MoodleBackupXmlDetails,
                    MoodleBackupXmlContents as MoodleBackupXmlContents,
                    MoodleBackupXmlSettings as MoodleBackupXmlSettings);
            }
        }

        MoodleBackupXmlMoodleBackup.SetParameters(MoodleBackupXmlInformation as MoodleBackupXmlInformation);
        
        //create moodle_backup.xml file
        MoodleBackupXmlMoodleBackup.Serialize();
    }

    public void CreateOutcomesXml()
    {
        //set parameters of outcomes.xml file
        OutcomesXmlOutcomesDefinition.SetParameters();
        
        //create outcomes.xml file
        OutcomesXmlOutcomesDefinition.Serialize();
    }

    public void CreateQuestionsXml()
    {
        //set parameters of questions.xml file
        QuestionsXmlQuestionsCategories.SetParameters();
        
        //create questions.xml file
        QuestionsXmlQuestionsCategories.Serialize();
    }

    public void CreateRolesXml()
    {
        //set parameters of the roles.xml file
        RolesXmlRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        RolesXmlRolesDefinition.SetParameters(RolesXmlRole as RolesXmlRole);
        
        //create roles.xml file
        RolesXmlRolesDefinition.Serialize();
    }

    public void CreateScalesXml()
    {
        //set parameters of the scales.xml file
        ScalesXmlScalesDefinition.SetParameters();
        
        //create scales.xml file
        ScalesXmlScalesDefinition.Serialize();
    }
    
}
