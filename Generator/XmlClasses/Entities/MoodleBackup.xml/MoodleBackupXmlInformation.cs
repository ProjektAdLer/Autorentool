using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.MoodleBackup.xml;

[XmlRoot(ElementName="information")]
	public class MoodleBackupXmlInformation : IMoodleBackupXmlInformation{

		public MoodleBackupXmlInformation()
		{
			Name = "C#_AuthoringTool_Created_Backup.mbz";
			MoodleVersion = "2021051703";
			MoodleRelease = "3.11.3 (Build: 20210913)";
			BackupVersion = "2021051700";
			BackupRelease = "3.11";
			BackupDate = "";
			MnetRemoteUsers = "0";
			IncludeFiles = "1";
			IncludeFileReferencesToExternalContent = "0";
			OriginalWwwRoot = "https://moodle.cluuub.xyz";
			OriginalSiteIdentifierHash = "c9629ccd3c092478330b78bdf4dcdb18";
			OriginalCourseId = "1";
			OriginalCourseFormat = "topics";
			OriginalCourseFullname = "";
			OriginalCourseShortname = "";
			OriginalCourseStartDate = "";
			OriginalCourseEndDate = "2221567452";
			OriginalCourseContextId = "1";
			OriginalSystemContextId = "1";
			Details = new MoodleBackupXmlDetails();
			Contents = new MoodleBackupXmlContents();
			Settings = new MoodleBackupXmlSettings();
		}
		
		[XmlElement(ElementName="name")]
		public string Name { get; set; }
		
		[XmlElement(ElementName="moodle_version")]
		public string MoodleVersion { get; set; }
		
		[XmlElement(ElementName="moodle_release")]
		public string MoodleRelease { get; set; }
		
		[XmlElement(ElementName="backup_version")]
		public string BackupVersion { get; set; }
		
		[XmlElement(ElementName="backup_release")]
		public string BackupRelease { get; set; }
		
		[XmlElement(ElementName="backup_date")]
		public string BackupDate { get; set; }
		
		[XmlElement(ElementName="mnet_remoteusers")]
		public string MnetRemoteUsers { get; set; }
		
		[XmlElement(ElementName="include_files")]
		public string IncludeFiles { get; set; }
		
		[XmlElement(ElementName="include_file_references_to_external_content")]
		public string IncludeFileReferencesToExternalContent { get; set; }
		
		[XmlElement(ElementName="original_wwwroot")]
		public string OriginalWwwRoot { get; set; }
		
		[XmlElement(ElementName="original_site_identifier_hash")]
		public string OriginalSiteIdentifierHash { get; set; }
		
		[XmlElement(ElementName="original_course_id")]
		public string OriginalCourseId { get; set; }
		
		[XmlElement(ElementName="original_course_format")]
		public string OriginalCourseFormat { get; set; }
		
		[XmlElement(ElementName="original_course_fullname")]
		public string OriginalCourseFullname { get; set; }
		
		[XmlElement(ElementName="original_course_shortname")]
		public string OriginalCourseShortname { get; set; }
		
		[XmlElement(ElementName="original_course_startdate")]
		public string OriginalCourseStartDate { get; set; }
		
		[XmlElement(ElementName="original_course_enddate")]
		public string OriginalCourseEndDate { get; set; }
		
		[XmlElement(ElementName="original_course_contextid")]
		public string OriginalCourseContextId { get; set; }
		
		[XmlElement(ElementName="original_system_contextid")]
		public string OriginalSystemContextId { get; set; }
		
		[XmlElement(ElementName="details")]
		public MoodleBackupXmlDetails Details { get; set; }
		
		[XmlElement(ElementName="contents")]
		public MoodleBackupXmlContents Contents { get; set; }
		
		[XmlElement(ElementName="settings")]
		public MoodleBackupXmlSettings Settings { get; set; }
		
	}