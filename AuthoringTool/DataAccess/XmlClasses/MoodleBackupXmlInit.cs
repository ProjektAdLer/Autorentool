using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;
public class MoodleBackupXmlInit : IXMLInit
{
	public void XmlInit() 
	{
			var moodleBackup = new MoodleBackupXmlMoodleBackup();

            var moodleBackupInformation = new MoodleBackupXmlInformation();
            moodleBackupInformation.Name = "C#_XML_Created_Backup.mbz";
            moodleBackupInformation.Original_course_id = "53";
            moodleBackupInformation.Original_course_format = "topics";
            moodleBackupInformation.Original_course_fullname = "XML_Leerer Kurs";
            moodleBackupInformation.Original_course_shortname = "XML_LK";
            moodleBackupInformation.Original_course_startdate = "";
            moodleBackupInformation.Original_course_enddate = "";
            moodleBackupInformation.Original_course_contextid = "286";
            moodleBackupInformation.Original_system_contextid = "1";
            moodleBackupInformation.Original_course_startdate = "1645484400";
            moodleBackupInformation.Original_course_enddate = "1677020400";

            var moodleBackupDetail = new MoodleBackupXmlDetail();
            moodleBackupDetail.Backup_id = "6a4e8e833791eb72e5f3ee2227ee1b74";

            var moodleBackupDetails = new MoodleBackupXmlDetails();
            moodleBackupDetails.Detail = moodleBackupDetail;

            var moodleBackupSection = new MoodleBackupXmlSection();
            moodleBackupSection.Sectionid = "160";
            moodleBackupSection.Directory = "sections/section_160";
            moodleBackupSection.Title = "1";
            
            var moodleBackupSections = new MoodleBackupXmlSections();
            moodleBackupSections.Section = moodleBackupSection;
            
            var moodleBackupContents = new MoodleBackupXmlContents();
            moodleBackupContents.Sections = moodleBackupSections;

            var moodleBackupCourse = new MoodleBackupXmlCourse();
            moodleBackupCourse.Courseid = "53";
            moodleBackupCourse.Title = "XML_LK";
            moodleBackupCourse.Directory = "course";

            moodleBackupContents.Course = moodleBackupCourse;

            var moodleBackupSettings = new MoodleBackupXmlSettings();
            
            var moodleBackupSetting1 = new MoodleBackupXmlSetting();
            moodleBackupSetting1.Name = "filename";
            moodleBackupSetting1.Value = "C#_XML_Created_Backup.mbz";
            
            var moodleBackupSetting2 = new MoodleBackupXmlSetting();
            moodleBackupSetting2.Name = "imscc11";
            moodleBackupSetting2.Value = "0";
            
            var moodleBackupSetting3 = new MoodleBackupXmlSetting();
            moodleBackupSetting3.Name = "users";
            moodleBackupSetting3.Value = "0";
            
            var moodleBackupSetting4 = new MoodleBackupXmlSetting();
            moodleBackupSetting4.Name = "anonymize";
            moodleBackupSetting4.Value = "0";
            
            var moodleBackupSetting5 = new MoodleBackupXmlSetting();
            moodleBackupSetting5.Name = "role_assignments";
            moodleBackupSetting5.Value = "0";
            
            var moodleBackupSetting6 = new MoodleBackupXmlSetting();
            moodleBackupSetting6.Name = "activities";
            moodleBackupSetting6.Value = "0";
            
            var moodleBackupSetting7 = new MoodleBackupXmlSetting();
            moodleBackupSetting7.Name = "blocks";
            moodleBackupSetting7.Value = "0";

            var moodleBackupSetting8 = new MoodleBackupXmlSetting();
            moodleBackupSetting8.Name = "files";
            moodleBackupSetting8.Value = "0";
            
            var moodleBackupSetting9 = new MoodleBackupXmlSetting();
            moodleBackupSetting9.Name = "filters";
            moodleBackupSetting9.Value = "0";
            
            var moodleBackupSetting10 = new MoodleBackupXmlSetting();
            moodleBackupSetting10.Name = "comments";
            moodleBackupSetting10.Value = "0";
            
            var moodleBackupSetting11 = new MoodleBackupXmlSetting();
            moodleBackupSetting11.Name = "badges";
            moodleBackupSetting11.Value = "0";
            
            var moodleBackupSetting12 = new MoodleBackupXmlSetting();
            moodleBackupSetting12.Name = "calendarevents";
            moodleBackupSetting12.Value = "0";
            
            var moodleBackupSetting13 = new MoodleBackupXmlSetting();
            moodleBackupSetting13.Name = "userscompletion";
            moodleBackupSetting13.Value = "0";
            
            var moodleBackupSetting14 = new MoodleBackupXmlSetting();
            moodleBackupSetting14.Name = "logs";
            moodleBackupSetting14.Value = "0";
            
            var moodleBackupSetting15 = new MoodleBackupXmlSetting();
            moodleBackupSetting15.Name = "grade_histories";
            moodleBackupSetting15.Value = "0";
            
            var moodleBackupSetting16 = new MoodleBackupXmlSetting();
            moodleBackupSetting16.Name = "questionbank";
            moodleBackupSetting16.Value = "0";
            
            var moodleBackupSetting17 = new MoodleBackupXmlSetting();
            moodleBackupSetting17.Name = "groups";
            moodleBackupSetting17.Value = "0";
            
            var moodleBackupSetting18 = new MoodleBackupXmlSetting();
            moodleBackupSetting18.Name = "competencies";
            moodleBackupSetting18.Value = "0";
            
            var moodleBackupSetting19 = new MoodleBackupXmlSetting();
            moodleBackupSetting19.Name = "customfield";
            moodleBackupSetting19.Value = "0";
            
            var moodleBackupSetting20 = new MoodleBackupXmlSetting();
            moodleBackupSetting20.Name = "contentbankcontent";
            moodleBackupSetting20.Value = "0";
            
            var moodleBackupSetting21 = new MoodleBackupXmlSetting();
            moodleBackupSetting21.Name = "legacyfiles";
            moodleBackupSetting21.Value = "0";
            
            var moodleBackupSetting22 = new MoodleBackupXmlSetting();
            moodleBackupSetting22.Level = "section";
            moodleBackupSetting22.Name = "section_160_included";
            moodleBackupSetting22.Value = "1";
            moodleBackupSetting22.Section = "section_160";
            
            var moodleBackupSetting23 = new MoodleBackupXmlSetting();
            moodleBackupSetting23.Level = "section";
            moodleBackupSetting23.Name = "section_160_userinfo";
            moodleBackupSetting23.Value = "0";
            moodleBackupSetting23.Section = "section_160";

            moodleBackupSettings.Setting = new List<MoodleBackupXmlSetting>();
            moodleBackupSettings.Setting.Add(moodleBackupSetting1);
            moodleBackupSettings.Setting.Add(moodleBackupSetting2);
            moodleBackupSettings.Setting.Add(moodleBackupSetting3);
            moodleBackupSettings.Setting.Add(moodleBackupSetting4);
            moodleBackupSettings.Setting.Add(moodleBackupSetting5);
            moodleBackupSettings.Setting.Add(moodleBackupSetting6);
            moodleBackupSettings.Setting.Add(moodleBackupSetting7);
            moodleBackupSettings.Setting.Add(moodleBackupSetting8);
            moodleBackupSettings.Setting.Add(moodleBackupSetting9);
            moodleBackupSettings.Setting.Add(moodleBackupSetting10);
            moodleBackupSettings.Setting.Add(moodleBackupSetting11);
            moodleBackupSettings.Setting.Add(moodleBackupSetting12);
            moodleBackupSettings.Setting.Add(moodleBackupSetting13);
            moodleBackupSettings.Setting.Add(moodleBackupSetting14);
            moodleBackupSettings.Setting.Add(moodleBackupSetting15);
            moodleBackupSettings.Setting.Add(moodleBackupSetting16);
            moodleBackupSettings.Setting.Add(moodleBackupSetting17);
            moodleBackupSettings.Setting.Add(moodleBackupSetting18);
            moodleBackupSettings.Setting.Add(moodleBackupSetting19);
            moodleBackupSettings.Setting.Add(moodleBackupSetting20);
            moodleBackupSettings.Setting.Add(moodleBackupSetting21);
            moodleBackupSettings.Setting.Add(moodleBackupSetting22);
            moodleBackupSettings.Setting.Add(moodleBackupSetting23);
            
            moodleBackupInformation.Details = moodleBackupDetails;
            moodleBackupInformation.Contents = moodleBackupContents;
            moodleBackupInformation.Settings = moodleBackupSettings;
            
            moodleBackup.Information = moodleBackupInformation;
            
            var xml = new XmlSer();
            xml.serialize(moodleBackup, "moodle_backup.xml"); 
	}
}
