using System.Xml.Serialization;
using AuthoringTool.DataAccess.XmlClasses.Entities;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="information")]
	public partial class MoodleBackupXmlInformation : IMoodleBackupXmlInformation{
		
		public void SetParameters(string? name, string? moodleVersion, string? moodleRelease, string? backupVersion,
			string? backupRelease, string? backupDate, string? mnetRemoteusers, string? includeFiles,
			string? includeFileReferencesToExternalContent, string? originalWwwroot, string? originalSiteIdentifierHash,
			string? originalCourseId, string? originalCourseFormat, string? originalCourseFullname,
			string? originalCourseShortname, string? originalCourseStartdate, string? originalCourseEnddate,
			string? originalCourseContextid, string? originalSystemContextid, MoodleBackupXmlDetails? details,
			MoodleBackupXmlContents? contents, MoodleBackupXmlSettings? settings)
		{
			Name = name;
			Moodle_version = moodleVersion;
			Moodle_release = moodleRelease;
			Backup_version = backupVersion;
			Backup_release = backupRelease;
			Backup_date = backupDate;
			Mnet_remoteusers = mnetRemoteusers;
			Include_files = includeFiles;
			Include_file_references_to_external_content = includeFileReferencesToExternalContent;
			Original_wwwroot = originalWwwroot;
			Original_site_identifier_hash = originalSiteIdentifierHash;
			Original_course_id = originalCourseId;
			Original_course_format = originalCourseFormat;
			Original_course_fullname = originalCourseFullname;
			Original_course_shortname = originalCourseShortname;
			Original_course_startdate = originalCourseStartdate;
			Original_course_enddate = originalCourseEnddate;
			Original_course_contextid = originalCourseContextid;
			Original_system_contextid = originalSystemContextid;
			Details = details;
			Contents = contents;
			Settings = settings;
		}

		[XmlElement(ElementName="name")]
		public string? Name { get; set; }
		[XmlElement(ElementName="moodle_version")]
		public string? Moodle_version { get; set; }
		[XmlElement(ElementName="moodle_release")]
		public string? Moodle_release { get; set; }
		[XmlElement(ElementName="backup_version")]
		public string? Backup_version { get; set; }
		[XmlElement(ElementName="backup_release")]
		public string? Backup_release { get; set; }
		[XmlElement(ElementName="backup_date")]
		public string? Backup_date { get; set; }
		[XmlElement(ElementName="mnet_remoteusers")]
		public string? Mnet_remoteusers { get; set; }
		[XmlElement(ElementName="include_files")]
		public string? Include_files { get; set; }
		[XmlElement(ElementName="include_file_references_to_external_content")]
		public string? Include_file_references_to_external_content { get; set; }
		[XmlElement(ElementName="original_wwwroot")]
		public string? Original_wwwroot { get; set; }
		[XmlElement(ElementName="original_site_identifier_hash")]
		public string? Original_site_identifier_hash { get; set; }
		[XmlElement(ElementName="original_course_id")]
		public string? Original_course_id { get; set; }
		[XmlElement(ElementName="original_course_format")]
		public string? Original_course_format { get; set; }
		[XmlElement(ElementName="original_course_fullname")]
		public string? Original_course_fullname { get; set; }
		[XmlElement(ElementName="original_course_shortname")]
		public string? Original_course_shortname { get; set; }
		[XmlElement(ElementName="original_course_startdate")]
		public string? Original_course_startdate { get; set; }
		[XmlElement(ElementName="original_course_enddate")]
		public string? Original_course_enddate { get; set; }
		[XmlElement(ElementName="original_course_contextid")]
		public string? Original_course_contextid { get; set; }
		[XmlElement(ElementName="original_system_contextid")]
		public string? Original_system_contextid { get; set; }
		[XmlElement(ElementName="details")]
		public MoodleBackupXmlDetails? Details { get; set; }
		[XmlElement(ElementName="contents")]
		public MoodleBackupXmlContents? Contents { get; set; }
		[XmlElement(ElementName="settings")]
		public MoodleBackupXmlSettings? Settings { get; set; }
		
	}