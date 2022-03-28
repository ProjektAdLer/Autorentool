namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlBackupFactory
{
    public XmlBackupFactory()
    {
        //create files.xml file
        var filesFiles = new FilesXmlFiles();
        filesFiles.Serialize();
        
        //create gradebook.xml file
        var gradebookSetting = new GradebookXmlGradeSetting();
        gradebookSetting.SetParameters("minmaxtouse", "1");
        var gradebookSettings = new GradebookXmlGradeSettings();
        gradebookSettings.SetParameters(gradebookSetting);
        var gradebook = new GradebookXmlGradebook();
        gradebook.SetParameters(gradebookSettings);
        gradebook.Serialize();
        
        //create groups.xml file
        var groupingsList = new GroupsXmlGroupingsList();
        var groups = new GroupsXmlGroups();
        groups.SetParameters(groupingsList);
        groups.Serialize();
        
        //create moodle_backup.xml file
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        moodlebackupDetail.SetParameters("6a4e8e833791eb72e5f3ee2227ee1b74");
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.SetParameters(moodlebackupDetail);

        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "sections/section_160", "1");
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.SetParameters(moodlebackupSection);

        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");

        var moodlebackupContents = new MoodleBackupXmlContents();
        moodlebackupContents.SetParameters(moodlebackupSections, moodlebackupCourse);

        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        moodlebackupSetting1.SetParametersShort("filename", "C#_XML_Created_Backup.mbz");
        var moodlebackupSetting2 = new MoodleBackupXmlSetting();
        moodlebackupSetting2.SetParametersShort("imscc11", "0");
        var moodlebackupSetting3 = new MoodleBackupXmlSetting();
        moodlebackupSetting3.SetParametersShort("users", "0");
        var moodlebackupSetting4 = new MoodleBackupXmlSetting();
        moodlebackupSetting4.SetParametersShort("anonymize", "0");
        var moodlebackupSetting5 = new MoodleBackupXmlSetting();
        moodlebackupSetting5.SetParametersShort("role_assignments", "0");
        var moodlebackupSetting6 = new MoodleBackupXmlSetting();
        moodlebackupSetting6.SetParametersShort("activities", "0");
        var moodlebackupSetting7 = new MoodleBackupXmlSetting();
        moodlebackupSetting7.SetParametersShort("blocks", "0");
        var moodlebackupSetting8 = new MoodleBackupXmlSetting();
        moodlebackupSetting8.SetParametersShort("files", "0");
        var moodlebackupSetting9 = new MoodleBackupXmlSetting();
        moodlebackupSetting9.SetParametersShort("filters", "0");
        var moodlebackupSetting10 = new MoodleBackupXmlSetting();
        moodlebackupSetting10.SetParametersShort("comments", "0");
        var moodlebackupSetting11 = new MoodleBackupXmlSetting();
        moodlebackupSetting11.SetParametersShort("badges", "0");
        var moodlebackupSetting12 = new MoodleBackupXmlSetting();
        moodlebackupSetting12.SetParametersShort("calendarevents", "0");
        var moodlebackupSetting13 = new MoodleBackupXmlSetting();
        moodlebackupSetting13.SetParametersShort("userscompletion", "0");
        var moodlebackupSetting14 = new MoodleBackupXmlSetting();
        moodlebackupSetting14.SetParametersShort("logs", "0");
        var moodlebackupSetting15 = new MoodleBackupXmlSetting();
        moodlebackupSetting15.SetParametersShort("grade_histories", "0");
        var moodlebackupSetting16 = new MoodleBackupXmlSetting();
        moodlebackupSetting16.SetParametersShort("questionbank", "0");
        var moodlebackupSetting17 = new MoodleBackupXmlSetting();
        moodlebackupSetting17.SetParametersShort("groups", "0");
        var moodlebackupSetting18 = new MoodleBackupXmlSetting();
        moodlebackupSetting18.SetParametersShort("competencies", "0");
        var moodlebackupSetting19 = new MoodleBackupXmlSetting();
        moodlebackupSetting19.SetParametersShort("customfield", "0");
        var moodlebackupSetting20 = new MoodleBackupXmlSetting();
        moodlebackupSetting20.SetParametersShort("contentbankcontent", "0");
        var moodlebackupSetting21 = new MoodleBackupXmlSetting();
        moodlebackupSetting21.SetParametersShort("legacyfiles", "0");
        var moodlebackupSetting22 = new MoodleBackupXmlSetting();
        moodlebackupSetting22.SetParametersFull("section_160_included", "1", "section", "section_160");
        var moodlebackupSetting23 = new MoodleBackupXmlSetting();
        moodlebackupSetting23.SetParametersFull("section_160_userinfo", "0", "section", "section_160");

        var moodlebackupSettings = new MoodleBackupXmlSettings();
        moodlebackupSettings.SetParameters();
        moodlebackupSettings.Setting.Add(moodlebackupSetting1);
        moodlebackupSettings.Setting.Add(moodlebackupSetting2);
        moodlebackupSettings.Setting.Add(moodlebackupSetting3);
        moodlebackupSettings.Setting.Add(moodlebackupSetting4);
        moodlebackupSettings.Setting.Add(moodlebackupSetting5);
        moodlebackupSettings.Setting.Add(moodlebackupSetting6);
        moodlebackupSettings.Setting.Add(moodlebackupSetting7);
        moodlebackupSettings.Setting.Add(moodlebackupSetting8);
        moodlebackupSettings.Setting.Add(moodlebackupSetting9);
        moodlebackupSettings.Setting.Add(moodlebackupSetting10);
        moodlebackupSettings.Setting.Add(moodlebackupSetting11);
        moodlebackupSettings.Setting.Add(moodlebackupSetting12);
        moodlebackupSettings.Setting.Add(moodlebackupSetting13);
        moodlebackupSettings.Setting.Add(moodlebackupSetting14);
        moodlebackupSettings.Setting.Add(moodlebackupSetting15);
        moodlebackupSettings.Setting.Add(moodlebackupSetting16);
        moodlebackupSettings.Setting.Add(moodlebackupSetting17);
        moodlebackupSettings.Setting.Add(moodlebackupSetting18);
        moodlebackupSettings.Setting.Add(moodlebackupSetting19);
        moodlebackupSettings.Setting.Add(moodlebackupSetting20);
        moodlebackupSettings.Setting.Add(moodlebackupSetting21);
        moodlebackupSettings.Setting.Add(moodlebackupSetting22);
        moodlebackupSettings.Setting.Add(moodlebackupSetting23);
        
        var moodlebackupInformation = new MoodleBackupXmlInformation();
        moodlebackupInformation.SetParameters("C#_XML_Created_Backup.mbz", 
            "53", "topics", "XML_Leerer Kurs", 
            "XML_LK", "286", "1", "1645484400", 
            "1677020400", moodlebackupDetails, moodlebackupContents, moodlebackupSettings);

        var moodlebackup = new MoodleBackupXmlMoodleBackup();
        moodlebackup.SetParameters(moodlebackupInformation);
        
        moodlebackup.Serialize();
        
        //write outcomes.xml file
        var outcomesOutcomesDefinition = new OutcomesXmlOutcomesDefinition();
        outcomesOutcomesDefinition.Serialize();
        
        //write questions.xml file
        var questionsQuestionsCategories = new QuestionsXmlQuestionsCategories();
        questionsQuestionsCategories.Serialize();
        
        //write roles.xml file
        var rolesRole = new RolesXmlRole();
        rolesRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        var rolesRolesDefinition = new RolesXmlRolesDefinition();
        rolesRolesDefinition.SetParameters(rolesRole);
        rolesRolesDefinition.Serialize();
        
        //write scales.xml file
        var scalesScalesDefinition = new ScalesXmlScalesDefinition();
        scalesScalesDefinition.Serialize();


    }
}