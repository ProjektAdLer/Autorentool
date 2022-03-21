using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses

{
	[XmlRoot(ElementName="detail")]
	public class MoodleBackupXmlDetail {
		
		[XmlElement(ElementName="type")]
		public string Type = "course";
		
		[XmlElement(ElementName="format")]
		public string Format = "moodle2";

		[XmlElement(ElementName = "interactive")]
		public string Interactive = "1";
		
		[XmlElement(ElementName="mode")]
		public string Mode = "10";

		[XmlElement(ElementName = "execution")]
		public string Execution = "1";

		[XmlElement(ElementName = "executiontime")]
		public string Executiontime = "0";

		[XmlAttribute(AttributeName = "backup_id")]
		public string Backup_id = "";
	}

	[XmlRoot(ElementName="details")]
	public class MoodleBackupXmlDetails {
		[XmlElement(ElementName="detail")]
		public MoodleBackupXmlDetail Detail;
	}

	[XmlRoot(ElementName="section")]
	public class MoodleBackupXmlSection
	{
		[XmlElement(ElementName = "sectionid")]
		public string Sectionid = "";
		
		[XmlElement(ElementName="title")]
		public string Title = "";

		[XmlElement(ElementName = "directory")]
		public string Directory = "";
	}

	[XmlRoot(ElementName="sections")]
	public class MoodleBackupXmlSections {
		[XmlElement(ElementName="section")]
		public MoodleBackupXmlSection Section;
	}

	[XmlRoot(ElementName="course")]
	public class MoodleBackupXmlCourse {
		
		[XmlElement(ElementName="courseid")]
		public string Courseid = "";
		[XmlElement(ElementName="title")]
		public string Title = "";
		[XmlElement(ElementName = "directory")]
		public string Directory = "course";
	}

	[XmlRoot(ElementName="contents")]
	public class MoodleBackupXmlContents {
		[XmlElement(ElementName="sections")]
		public MoodleBackupXmlSections Sections;
		[XmlElement(ElementName="course")]
		public MoodleBackupXmlCourse Course;
	}

	[XmlRoot(ElementName="setting")]
	public class MoodleBackupXmlSetting {
		[XmlElement(ElementName="level")]
		public string Level = "root";
		
		[XmlElement(ElementName="name")]
		public string Name = "";
		
		[XmlElement(ElementName="value")]
		public string Value = "";
		
		[XmlElement(ElementName="section")]
		public string Section = "";
	}

	[XmlRoot(ElementName="settings")]
	public class MoodleBackupXmlSettings {
		[XmlElement(ElementName="setting")]
		public List<MoodleBackupXmlSetting> Setting;
	}

	[XmlRoot(ElementName="information")]
	public class MoodleBackupXmlInformation {
		[XmlElement(ElementName="name")]
		public string Name = "";

		[XmlElement(ElementName = "moodle_version")]
		public string Moodle_version = "2021051703";

		[XmlElement(ElementName = "moodle_release")]
		public string Moodle_release = "3.11.3 (Build: 20210913)";

		[XmlElement(ElementName = "backup_version")]
		public string Backup_version = "2021051700";

		[XmlElement(ElementName = "backup_release")]
		public string Backup_release = "3.11";

		[XmlElement(ElementName = "backup_date")]
		public string Backup_date = "1645447382";

		[XmlElement(ElementName = "mnet_remoteusers")]
		public string Mnet_remoteusers = "0";

		[XmlElement(ElementName = "include_files")]
		public string Include_files = "0";

		[XmlElement(ElementName = "include_file_references_to_external_content")]
		public string Include_file_references_to_external_content = "0";

		[XmlElement(ElementName = "original_wwwroot")]
		public string Original_wwwroot = "https://moodle.cluuub.xyz";

		[XmlElement(ElementName = "original_site_identifier_hash")]
		public string Original_site_identifier_hash = "c9629ccd3c092478330b78bdf4dcdb18";

		[XmlElement(ElementName = "original_course_id")]
		public string Original_course_id = "";

		[XmlElement(ElementName = "original_course_format")]
		public string Original_course_format = "";

		[XmlElement(ElementName = "original_course_fullname")]
		public string Original_course_fullname = "";

		[XmlElement(ElementName = "original_course_shortname")]
		public string Original_course_shortname = "";

		[XmlElement(ElementName = "original_course_startdate")]
		public string Original_course_startdate = "";

		[XmlElement(ElementName = "original_course_enddate")]
		public string Original_course_enddate = "";

		[XmlElement(ElementName = "original_course_contextid")]
		public string Original_course_contextid = "";

		[XmlElement(ElementName = "original_system_contextid")]
		public string Original_system_contextid = "";
		
		[XmlElement(ElementName="details")]
		public MoodleBackupXmlDetails Details { get; set; }
		
		[XmlElement(ElementName="contents")]
		public MoodleBackupXmlContents Contents { get; set; }
		
		[XmlElement(ElementName="settings")]
		public MoodleBackupXmlSettings Settings { get; set; }
		
	}

	[XmlRoot(ElementName="moodle_backup")]
	public class MoodleBackupXmlMoodleBackup {
		[XmlElement(ElementName="information")]
		public MoodleBackupXmlInformation Information { get; set; }
	}

	public class MoodleBackupXmlInit
	{
		public MoodleBackupXmlMoodleBackup Init()
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
            return moodleBackup;
		}
	}
	
}
