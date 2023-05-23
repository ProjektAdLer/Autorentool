namespace Generator.XmlClasses.Entities._course.Course.xml;

public interface ICourseCourseXmlCourse : IXmlSerializable
{
		string Shortname { get; set; }
		
		string Fullname { get; set; }
		
		string IdNumber { get; set; }
		
		string Summary { get; set; }

		string SummaryFormat { get; set; }
		
		string Format { get; set; }
		
		string DefaultTileIcon { get; set; }
		
		string BaseColour { get; set; }
		
		string CourseUseSubtiles { get; set; }
		
		string UseSubtilesSecZero { get; set; }
		
		string CourseShowTileProgress { get; set; }
		
		string DisplayFilterbar { get; set; }
		
		string CourseUseBarForHeadings { get; set; }

		string ShowGrades { get; set; }
		
		string NewsItems { get; set; }
		
		string Startdate { get; set; }
		
		string Enddate { get; set; }
		
		string Marker { get; set; }
		
		string Maxbytes { get; set; }
		
		string Legacyfiles { get; set; }
		
		string ShowReports { get; set; }
		
		string Visible { get; set; }
		
		string GroupMode { get; set; }
		
		string GroupModeForce { get; set; }
		
		string DefaultGroupingId { get; set; }
		
		string Lang { get; set; }

		string Theme { get; set; }
		
		string Timecreated { get; set; }
		
		string Timemodified { get; set; }
		
		string Requested { get; set; }
		
		string ShowActivityDates { get; set; }
		
		string ShowCompletionConditions { get; set; }

		string EnableCompletion { get; set; }
		
		string CompletionNotify { get; set; }
		
		string HiddenSections { get; set; }
		
		string CourseDisplay { get; set; }
		
		CourseCourseXmlCategory Category { get; set; }
		
		string Tags { get; set; }
		
		string CustomFields { get; set; }
		
		string Id { get; set; }
		
		string ContextId { get; set; }
		CourseCourseXmlPluginLocalAdlerCourse PluginLocalAdlerCourse { get; set; }
}