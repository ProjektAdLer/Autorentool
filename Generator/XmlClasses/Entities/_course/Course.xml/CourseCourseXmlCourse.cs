using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._course.Course.xml;

[XmlRoot(ElementName="course")]
	public class CourseCourseXmlCourse : ICourseCourseXmlCourse
	{

		public CourseCourseXmlCourse()
		{
			Shortname = "";
			Fullname = "";
			IdNumber = "";
			Summary = "";
			SummaryFormat = "1";
			Format = "tiles";
			DefaultTileIcon = "pie-chart";
			BaseColour = "#009681";
			CourseUseSubtiles = "0";
			UseSubtilesSecZero = "0";
			CourseShowTileProgress = "2";
			DisplayFilterbar = "0";
			CourseUseBarForHeadings = "0";
			ShowGrades = "1";
			NewsItems = "5";
			Startdate = "1645484400";
			Enddate = "2221567452";
			Marker = "0";
			Maxbytes = "0";
			Legacyfiles = "0";
			ShowReports = "0";
			Visible = "1";
			GroupMode = "0";
			GroupModeForce = "0";
			DefaultGroupingId = "0";
			Lang = "";
			Theme = "boost";
			Timecreated = "";
			Timemodified = "";
			Requested = "0";
			ShowActivityDates = "1";
			ShowCompletionConditions = "1";
			EnableCompletion = "1";
			CompletionNotify = "0";
			HiddenSections = "0";
			CourseDisplay = "0";
			PluginLocalAdlerCourse = new CourseCourseXmlPluginLocalAdlerCourse();
			Category = new CourseCourseXmlCategory();
			Tags = "";
			CustomFields = "";
			Id = "1";
			ContextId = "1";
		}
		
		
		public void Serialize()
		{
			var xml = new XmlSerialize();
			xml.Serialize(this, Path.Join("course","course.xml"));
		}

		[XmlElement(ElementName = "shortname")]
		public string Shortname { get; set; }
		
		[XmlElement(ElementName="fullname")]
		public string Fullname { get; set; }
		
		[XmlElement(ElementName="idnumber")]
		public string IdNumber { get; set; }
		
		[XmlElement(ElementName="summary")]
		public string Summary { get; set; }

		[XmlElement(ElementName = "summaryformat")]
		public string SummaryFormat { get; set; }
		
		[XmlElement(ElementName="format")]
		public string Format { get; set; }
		
		[XmlElement(ElementName="defaulttileicon")]
		public string DefaultTileIcon { get; set; }
		
		[XmlElement(ElementName="basecolour")]
		public string BaseColour { get; set; }
		
		[XmlElement(ElementName="courseusesubtiles")]
		public string CourseUseSubtiles { get; set; }
		
		[XmlElement(ElementName="usesubtilesseczero")]
		public string UseSubtilesSecZero { get; set; }
		
		[XmlElement(ElementName="courseshowtileprogress")]
		public string CourseShowTileProgress { get; set; }
		
		[XmlElement(ElementName="displayfilterbar")]
		public string DisplayFilterbar { get; set; }
		
		[XmlElement(ElementName="courseusebarforheadings")]
		public string CourseUseBarForHeadings { get; set; }

		[XmlElement(ElementName = "showgrades")]
		public string ShowGrades { get; set; }
		
		[XmlElement(ElementName="newsitems")]
		public string NewsItems { get; set; }
		
		[XmlElement(ElementName="startdate")]
		public string Startdate { get; set; }
		
		[XmlElement(ElementName="enddate")]
		public string Enddate { get; set; }
		
		[XmlElement(ElementName="marker")]
		public string Marker { get; set; }
		
		[XmlElement(ElementName="maxbytes")]
		public string Maxbytes { get; set; }
		
		[XmlElement(ElementName="legacyfiles")]
		public string Legacyfiles { get; set; }
		
		[XmlElement(ElementName="showreports")]
		public string ShowReports { get; set; }
		
		[XmlElement(ElementName="visible")]
		public string Visible { get; set; }
		
		[XmlElement(ElementName="groupmode")]
		public string GroupMode { get; set; }
		
		[XmlElement(ElementName="groupmodeforce")]
		public string GroupModeForce { get; set; }
		
		[XmlElement(ElementName="defaultgroupingid")]
		public string DefaultGroupingId { get; set; }
		
		[XmlElement(ElementName="lang")]
		public string Lang { get; set; }
		
		[XmlElement(ElementName="theme")]
		public string Theme { get; set; }
		
		[XmlElement(ElementName="timecreated")]
		public string Timecreated { get; set; }
		
		[XmlElement(ElementName="timemodified")]
		public string Timemodified { get; set; }
		
		[XmlElement(ElementName="requested")]
		public string Requested { get; set; }
		
		[XmlElement(ElementName="showactivitydates")]
		public string ShowActivityDates { get; set; }
		
		[XmlElement(ElementName="showcompletionconditions")]
		public string ShowCompletionConditions { get; set; }
		
		[XmlElement(ElementName="enablecompletion")]
		public string EnableCompletion { get; set; }
		
		[XmlElement(ElementName="completionnotify")]
		public string CompletionNotify { get; set; }
		
		[XmlElement(ElementName="hiddensections")]
		public string HiddenSections { get; set; }
		
		[XmlElement(ElementName="coursedisplay")]
		public string CourseDisplay { get; set; }
		
		[XmlElement(ElementName = "plugin_local_adler_course")]
		public CourseCourseXmlPluginLocalAdlerCourse PluginLocalAdlerCourse { get; set; }
		
		[XmlElement(ElementName="category")]
		public CourseCourseXmlCategory Category { get; set; }
		
		[XmlElement(ElementName="tags")]
		public string Tags { get; set; }
		
		[XmlElement(ElementName="customfields")]
		public string CustomFields { get; set; }
		
		[XmlAttribute(AttributeName="id")]
		public string Id { get; set; }
		
		[XmlAttribute(AttributeName="contextid")]
		public string ContextId { get; set; }
		
	}
	