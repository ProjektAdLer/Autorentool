namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlBackupFactory
{
    internal IFilesXmlFiles FilesXmlFiles { get;}
    internal IGradebookXmlGradeSetting GradebookXmlGradebookSetting { get; }
    internal IGradebookXmlGradeSettings GradebookXmlGradebookSettings { get; }
    internal IGradebookXmlGradebook GradebookXmlGradebook { get; }
    internal IGroupsXmlGroupingsList GroupsXmlGroupingsList { get; }
    internal IGroupsXmlGroups GroupsXmlGroups { get; }
    internal IMoodleBackupXmlDetail MoodleBackupXmlDetail { get; }
    internal IMoodleBackupXmlDetails MoodleBackupXmlDetails { get; }
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
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingSection_160_Included { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingSection_160_userinfo { get; }
    internal IMoodleBackupXmlSettings MoodleBackupXmlSettings { get; }
    internal IMoodleBackupXmlInformation MoodleBackupXmlInformation { get; }
    internal IMoodleBackupXmlMoodleBackup MoodleBackupXmlMoodleBackup { get; }

    public XmlBackupFactory()
    {
        FilesXmlFiles = new FilesXmlFiles();
        
        GradebookXmlGradebookSetting = new GradebookXmlGradeSetting();
        GradebookXmlGradebookSettings = new GradebookXmlGradeSettings();
        GradebookXmlGradebook = new GradebookXmlGradebook();
        
        GroupsXmlGroupingsList = new GroupsXmlGroupingsList();
        GroupsXmlGroups = new GroupsXmlGroups();

        MoodleBackupXmlDetail = new MoodleBackupXmlDetail();
        MoodleBackupXmlDetails = new MoodleBackupXmlDetails();
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
        MoodleBackupXmlSettingSection_160_Included = new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingSection_160_userinfo = new MoodleBackupXmlSetting();
        
        MoodleBackupXmlSettings = new MoodleBackupXmlSettings();
        MoodleBackupXmlInformation = new MoodleBackupXmlInformation();
        MoodleBackupXmlMoodleBackup = new MoodleBackupXmlMoodleBackup();

        CreateFilesXml();
        CreateGradebookXml();
        CreateGroupsXml();
        CreateOutcomesXml();
        CreateQuestionsXml();
        CreateRolesXml();
        CreateScalesXml();
        CreateMoodleBackupXml();
    }

    //Just for Testing
    public XmlBackupFactory(IFilesXmlFiles filesXmlFiles, 
        IGradebookXmlGradeSetting gradebookXmlGradebookSetting, IGradebookXmlGradeSettings gradebookXmlGradebookSettings,IGradebookXmlGradebook gradebookXmlGradebook,
        IGroupsXmlGroupingsList groupsXmlGroupingsList, IGroupsXmlGroups groupsXmlGroups,
        IMoodleBackupXmlDetail moodleBackupXmlDetail, IMoodleBackupXmlDetails moodleBackupXmlDetails, IMoodleBackupXmlSection moodleBackupXmlSection,
        IMoodleBackupXmlSections moodleBackupXmlSections, IMoodleBackupXmlCourse moodleBackupXmlCourse, IMoodleBackupXmlContents moodleBackupXmlContents,
        IMoodleBackupXmlSetting moodleBackupXmlSetting, IMoodleBackupXmlSettings moodleBackupXmlSettings, IMoodleBackupXmlInformation moodleBackupXmlInformation, IMoodleBackupXmlMoodleBackup moodleBackupXmlMoodleBackup)
    {
        FilesXmlFiles = filesXmlFiles;
        
        GradebookXmlGradebookSetting = gradebookXmlGradebookSetting;
        GradebookXmlGradebookSettings = gradebookXmlGradebookSettings;
        GradebookXmlGradebook = gradebookXmlGradebook;

        GroupsXmlGroupingsList = groupsXmlGroupingsList;
        GroupsXmlGroups = groupsXmlGroups;

        MoodleBackupXmlDetail = moodleBackupXmlDetail;
        MoodleBackupXmlDetails = moodleBackupXmlDetails;
        MoodleBackupXmlSection = moodleBackupXmlSection;
        MoodleBackupXmlSections = moodleBackupXmlSections;
        MoodleBackupXmlCourse = moodleBackupXmlCourse;
        MoodleBackupXmlContents = moodleBackupXmlContents;
        
        MoodleBackupXmlSettingFilename = moodleBackupXmlSetting;
        MoodleBackupXmlSettingImscc11 = moodleBackupXmlSetting;
        MoodleBackupXmlSettingUsers = moodleBackupXmlSetting;
        MoodleBackupXmlSettingAnonymize = moodleBackupXmlSetting;
        MoodleBackupXmlSettingRoleAssignments = moodleBackupXmlSetting;
        MoodleBackupXmlSettingActivities = moodleBackupXmlSetting;
        MoodleBackupXmlSettingBlocks = moodleBackupXmlSetting;
        MoodleBackupXmlSettingFiles = moodleBackupXmlSetting;
        MoodleBackupXmlSettingFilters = moodleBackupXmlSetting;
        MoodleBackupXmlSettingComments = moodleBackupXmlSetting;
        MoodleBackupXmlSettingBadges = moodleBackupXmlSetting;
        MoodleBackupXmlSettingCalendarevents = moodleBackupXmlSetting;
        MoodleBackupXmlSettingUserscompletion = moodleBackupXmlSetting;
        MoodleBackupXmlSettingLogs = moodleBackupXmlSetting;
        MoodleBackupXmlSettingGradeHistories = moodleBackupXmlSetting;
        MoodleBackupXmlSettingQuestionbank = moodleBackupXmlSetting;
        MoodleBackupXmlSettingGroups = moodleBackupXmlSetting;
        MoodleBackupXmlSettingCompetencies = moodleBackupXmlSetting;
        MoodleBackupXmlSettingCustomfield = moodleBackupXmlSetting;
        MoodleBackupXmlSettingContentbankcontent = moodleBackupXmlSetting;
        MoodleBackupXmlSettingLegacyfiles = moodleBackupXmlSetting;
        MoodleBackupXmlSettingSection_160_Included = moodleBackupXmlSetting;
        MoodleBackupXmlSettingSection_160_userinfo = moodleBackupXmlSetting;
        MoodleBackupXmlSettings = moodleBackupXmlSettings;

        MoodleBackupXmlInformation = moodleBackupXmlInformation;
        MoodleBackupXmlMoodleBackup = moodleBackupXmlMoodleBackup;
    }

    public void CreateFilesXml()
    {
        //create files.xml file
        FilesXmlFiles.SetParameters();
        
        FilesXmlFiles.Serialize();
    }

    public void CreateGradebookXml()
    {
        //create gradebook.xml file
        GradebookXmlGradebookSetting.SetParameters("minmaxtouse", "1");
        GradebookXmlGradebookSettings.SetParameters(GradebookXmlGradebookSetting as GradebookXmlGradeSetting);
        GradebookXmlGradebook.SetParameters(GradebookXmlGradebookSettings as GradebookXmlGradeSettings);
        
        GradebookXmlGradebook.Serialize();
    }

    public void CreateGroupsXml()
    {
        //create groups.xml file
        GroupsXmlGroupingsList.SetParameters("");
        GroupsXmlGroups.SetParameters(GroupsXmlGroupingsList as GroupsXmlGroupingsList);
        
        GroupsXmlGroups.Serialize();
    }

    public void CreateMoodleBackupXml()
    {
        //create moodle_backup.xml file
        MoodleBackupXmlDetail.SetParameters("6a4e8e833791eb72e5f3ee2227ee1b74");
        MoodleBackupXmlDetails.SetParameters(MoodleBackupXmlDetail as MoodleBackupXmlDetail);

        MoodleBackupXmlSection.SetParameters("160", "1", "sections/section_160");
        MoodleBackupXmlSections.SetParameters(MoodleBackupXmlSection as MoodleBackupXmlSection);

        MoodleBackupXmlCourse.SetParameters("53", "XML_LK", "course");

        MoodleBackupXmlContents.SetParameters(MoodleBackupXmlSections as MoodleBackupXmlSections, MoodleBackupXmlCourse as MoodleBackupXmlCourse);

        MoodleBackupXmlSettingFilename.SetParametersShort("filename", "C#_XML_Created_Backup.mbz");
        MoodleBackupXmlSettingImscc11.SetParametersShort("imscc11", "0");
        MoodleBackupXmlSettingUsers.SetParametersShort("users", "0");
        MoodleBackupXmlSettingAnonymize.SetParametersShort("anonymize", "0");
        MoodleBackupXmlSettingRoleAssignments.SetParametersShort("role_assignments", "0");
        MoodleBackupXmlSettingActivities.SetParametersShort("activities", "0");
        MoodleBackupXmlSettingBlocks.SetParametersShort("blocks", "0");
        MoodleBackupXmlSettingFiles.SetParametersShort("files", "0");
        MoodleBackupXmlSettingFilters.SetParametersShort("filters", "0");
        MoodleBackupXmlSettingComments.SetParametersShort("comments", "0");
        MoodleBackupXmlSettingBadges.SetParametersShort("badges", "0");
        MoodleBackupXmlSettingCalendarevents.SetParametersShort("calendarevents", "0");
        MoodleBackupXmlSettingUserscompletion.SetParametersShort("userscompletion", "0");
        MoodleBackupXmlSettingLogs.SetParametersShort("logs", "0");
        MoodleBackupXmlSettingGradeHistories.SetParametersShort("grade_histories", "0");
        MoodleBackupXmlSettingQuestionbank.SetParametersShort("questionbank", "0");
        MoodleBackupXmlSettingGroups.SetParametersShort("groups", "0");
        MoodleBackupXmlSettingCompetencies.SetParametersShort("competencies", "0");
        MoodleBackupXmlSettingCustomfield.SetParametersShort("customfield", "0");
        MoodleBackupXmlSettingContentbankcontent.SetParametersShort("contentbankcontent", "0");
        MoodleBackupXmlSettingLegacyfiles.SetParametersShort("legacyfiles", "0");
        MoodleBackupXmlSettingSection_160_Included.SetParametersFull("section_160_included", "1", "section", "section_160");
        MoodleBackupXmlSettingSection_160_userinfo.SetParametersFull("section_160_userinfo", "0", "section", "section_160");

        MoodleBackupXmlSettings.SetParameters();
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingFilename as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingImscc11 as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingUsers as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingAnonymize as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingRoleAssignments as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingActivities as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingBlocks as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingFiles as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingFilters as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingComments as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingBadges as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingCalendarevents as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingUserscompletion as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingLogs as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingGradeHistories as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingQuestionbank as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingGroups as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingCompetencies as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingCustomfield as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingContentbankcontent as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingLegacyfiles as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingSection_160_Included as MoodleBackupXmlSetting);
        MoodleBackupXmlSettings.FillSettings(MoodleBackupXmlSettingSection_160_userinfo as MoodleBackupXmlSetting);
        
        MoodleBackupXmlInformation.SetParameters("C#_XML_Created_Backup.mbz", 
            "53", "topics", "XML_Leerer Kurs", 
            "XML_LK", "286", "1", "1645484400", 
            "1677020400", MoodleBackupXmlDetails as MoodleBackupXmlDetails, MoodleBackupXmlContents as MoodleBackupXmlContents, 
            MoodleBackupXmlSettings as MoodleBackupXmlSettings);

        MoodleBackupXmlMoodleBackup.SetParameters(MoodleBackupXmlInformation as MoodleBackupXmlInformation);
        
        MoodleBackupXmlMoodleBackup.Serialize();
    }

    public void CreateOutcomesXml()
    {
        //write outcomes.xml file
        var outcomesOutcomesDefinition = new OutcomesXmlOutcomesDefinition();
        outcomesOutcomesDefinition.SetParameters();
        
        outcomesOutcomesDefinition.Serialize();
    }

    public void CreateQuestionsXml()
    {
        //write questions.xml file
        var questionsQuestionsCategories = new QuestionsXmlQuestionsCategories();
        questionsQuestionsCategories.SetParameters();
        
        questionsQuestionsCategories.Serialize();
    }

    public void CreateRolesXml()
    {
        //write roles.xml file
        var rolesRole = new RolesXmlRole();
        rolesRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        var rolesRolesDefinition = new RolesXmlRolesDefinition();
        rolesRolesDefinition.SetParameters(rolesRole);
        
        rolesRolesDefinition.Serialize();
    }

    public void CreateScalesXml()
    {
        //write scales.xml file
        var scalesScalesDefinition = new ScalesXmlScalesDefinition();
        scalesScalesDefinition.SetParameters();
        
        scalesScalesDefinition.Serialize();
    }
    
}
