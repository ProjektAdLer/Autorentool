namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlInformation
{
  
		string Name { get; set; }
		
		string MoodleVersion { get; set; }
		
		string MoodleRelease { get; set; }
		
		string BackupVersion { get; set; }
		
		string BackupRelease { get; set; }
		
		string BackupDate { get; set; }
		
		string MnetRemoteUsers { get; set; }
		
		string IncludeFiles { get; set; }
		
		string IncludeFileReferencesToExternalContent { get; set; }
		
		string OriginalWwwRoot { get; set; }
		
		string OriginalSiteIdentifierHash { get; set; }
		
		string OriginalCourseId { get; set; }
		
		string OriginalCourseFormat { get; set; }
		
		string OriginalCourseFullname { get; set; }
		
		string OriginalCourseShortname { get; set; }
		
		string OriginalCourseStartDate { get; set; }
		
		string OriginalCourseEndDate { get; set; }
		
		string OriginalCourseContextId { get; set; }
		
		string OriginalSystemContextId { get; set; }
		
		MoodleBackupXmlDetails Details { get; set; }
		
		MoodleBackupXmlContents Contents { get; set; }
	    MoodleBackupXmlSettings Settings { get; set; }
}