using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="information")]
	public partial class MoodleBackupXmlInformation : IMoodleBackupXmlInformation{
		
		public void SetParameters(string name, string originalCourseId,
                                  			string originalCourseFormat, string originalCourseFullname, string originalCourseShortname,
                                  			string originalCourseContextid, string originalSystemContextid, string originalCourseStartdate, 
                                  			string originalCourseEnddate, MoodleBackupXmlDetails backupDetails, MoodleBackupXmlContents backupContents, 
                                  			MoodleBackupXmlSettings backupSettings)
		{
        Name = name;
        Original_course_id = originalCourseId;
        Original_course_format = originalCourseFormat;
        Original_course_fullname = originalCourseFullname;
        Original_course_shortname = originalCourseShortname;
        Original_course_startdate = originalCourseStartdate;
        Original_course_enddate = originalCourseEnddate;
        Original_course_contextid = originalCourseContextid;
        Original_system_contextid = originalSystemContextid;
        Original_course_startdate = originalCourseStartdate;
        Original_course_enddate = originalCourseEnddate;
        Details = backupDetails;
        Contents = backupContents;
        Settings = backupSettings;
		}
		
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