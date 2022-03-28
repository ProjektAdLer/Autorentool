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
        var groups = new GroupsXmlGroups(groupingsList);
        groups.Serialize();
        
        //create moodle_backup.xml file
        var moodlebackupDetail = new MoodleBackupXmlDetail("6a4e8e833791eb72e5f3ee2227ee1b74");
        var moodlebackupDetails = new MoodleBackupXmlDetails(moodlebackupDetail);

        var moodlebackupSection = new MoodleBackupXmlSection("160", "sections/section_160", "1");
        var moodlebackupSections = new MoodleBackupXmlSections(moodlebackupSection);

        var moodlebackupCourse = new MoodleBackupXmlCourse("53", "XML_LK", "course");

        var moodlebackupContents = new MoodleBackupXmlContents(moodlebackupSections, moodlebackupCourse);

        var moodlebackupSetting1 = new MoodleBackupXmlSetting("filename", "C#_XML_Created_Backup.mbz");
        var moodlebackupSetting2 = new MoodleBackupXmlSetting("imscc11", "0");
        var moodlebackupSetting3 = new MoodleBackupXmlSetting("users", "0");
        var moodlebackupSetting4 = new MoodleBackupXmlSetting("anonymize", "0");
        var moodlebackupSetting5 = new MoodleBackupXmlSetting("role_assignments", "0");
        var moodlebackupSetting6 = new MoodleBackupXmlSetting("activities", "0");
        var moodlebackupSetting7 = new MoodleBackupXmlSetting("blocks", "0");
        var moodlebackupSetting8 = new MoodleBackupXmlSetting("files", "0");
        var moodlebackupSetting9 = new MoodleBackupXmlSetting("filters", "0");
        var moodlebackupSetting10 = new MoodleBackupXmlSetting("comments", "0");
        var moodlebackupSetting11 = new MoodleBackupXmlSetting("badges", "0");
        var moodlebackupSetting12 = new MoodleBackupXmlSetting("calendarevents", "0");
        var moodlebackupSetting13 = new MoodleBackupXmlSetting("userscompletion", "0");
        var moodlebackupSetting14 = new MoodleBackupXmlSetting("logs", "0");
        var moodlebackupSetting15 = new MoodleBackupXmlSetting("grade_histories", "0");
        var moodlebackupSetting16 = new MoodleBackupXmlSetting("questionbank", "0");
        var moodlebackupSetting17 = new MoodleBackupXmlSetting("groups", "0");
        var moodlebackupSetting18 = new MoodleBackupXmlSetting("competencies", "0");
        var moodlebackupSetting19 = new MoodleBackupXmlSetting("customfield", "0");
        var moodlebackupSetting20 = new MoodleBackupXmlSetting("contentbankcontent", "0");
        var moodlebackupSetting21 = new MoodleBackupXmlSetting("legacyfiles", "0");
        var moodlebackupSetting22 = new MoodleBackupXmlSetting("section_160_included", "1", "section", "section_160");
        var moodlebackupSetting23 = new MoodleBackupXmlSetting("section_160_userinfo", "0", "section", "section_160");

        var moodlebackupSettings = new MoodleBackupXmlSettings();
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
        
        var moodlebackupInformation = new MoodleBackupXmlInformation("C#_XML_Created_Backup.mbz", 
            "53", "topics", "XML_Leerer Kurs", 
            "XML_LK", "286", "1", "1645484400", 
            "1677020400", moodlebackupDetails, moodlebackupContents, moodlebackupSettings);

        var moodlebackup = new MoodleBackupXmlMoodleBackup(moodlebackupInformation);
        
        moodlebackup.Serialize();
        
        //write outcomes.xml file
        var outcomesOutcomesDefinition = new OutcomesXmlOutcomesDefinition();
        outcomesOutcomesDefinition.Serialize();
        
        //write questions.xml file
        var questionsQuestionsCategories = new QuestionsXmlQuestionsCategories();
        questionsQuestionsCategories.Serialize();
        
        //write roles.xml file
        var rolesRole = new RolesXmlRole("", "", "5", "student", "$@NULL@$", "5", "student");
        var rolesRolesDefinition = new RolesXmlRolesDefinition(rolesRole);
        rolesRolesDefinition.Serialize();
        
        //write scales.xml file
        var scalesScalesDefinition = new ScalesXmlScalesDefinition();
        scalesScalesDefinition.Serialize();


    }
}