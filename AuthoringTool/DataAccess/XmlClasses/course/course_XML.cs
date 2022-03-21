using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course

{
	[XmlRoot(ElementName="category")]
	public class CourseCourseXmlCategory {
		
		[XmlElement(ElementName="name")]
		public string Name = "";

		[XmlElement(ElementName = "description")]
		public string Description = "$@NULL@$"; 
		
		[XmlAttribute(AttributeName="id")]
		public string Id = "";
	}

	[XmlRoot(ElementName="course")]
	public class CourseCourseXmlCourse
	{

		[XmlElement(ElementName = "shortname")]
		public string Shortname = "";
		
		[XmlElement(ElementName="fullname")]
		public string Fullname = "";
		
		[XmlElement(ElementName="idnumber")]
		public string Idnumber = "";
		
		[XmlElement(ElementName="summary")]
		public string Summary = "";

		[XmlElement(ElementName = "summaryformat")]
		public string Summaryformat = "";
		
		[XmlElement(ElementName="format")]
		public string Format = "";

		[XmlElement(ElementName = "showgrades")]
		public string Showgrades = "";
		
		[XmlElement(ElementName="newsitems")]
		public string Newsitems = "";
		
		[XmlElement(ElementName="startdate")]
		public string Startdate = "";
		
		[XmlElement(ElementName="enddate")]
		public string Enddate = "";
		
		[XmlElement(ElementName="marker")]
		public string Marker = "0";
		
		[XmlElement(ElementName="maxbytes")]
		public string Maxbytes = "0";
		
		[XmlElement(ElementName="legacyfiles")]
		public string Legacyfiles = "0";
		
		[XmlElement(ElementName="showreports")]
		public string Showreports = "0";
		
		[XmlElement(ElementName="visible")]
		public string Visible = "1";
		
		[XmlElement(ElementName="groupmode")]
		public string Groupmode = "0";
		
		[XmlElement(ElementName="groupmodeforce")]
		public string Groupmodeforce = "0";
		
		[XmlElement(ElementName="defaultgroupingid")]
		public string Defaultgroupingid = "0";
		
		[XmlElement(ElementName="lang")]
		public string Lang = "";
		
		[XmlElement(ElementName="theme")]
		public string Theme = "";
		
		[XmlElement(ElementName="timecreated")]
		public string Timecreated = "";
		
		[XmlElement(ElementName="timemodified")]
		public string Timemodified = "";
		
		[XmlElement(ElementName="requested")]
		public string Requested = "0";
		
		[XmlElement(ElementName="showactivitydates")]
		public string Showactivitydates = "1";
		
		[XmlElement(ElementName="showcompletionconditions")]
		public string Showcompletionconditions = "1";
		
		[XmlElement(ElementName="enablecompletion")]
		public string Enablecompletion = "1";
		
		[XmlElement(ElementName="completionnotify")]
		public string Completionnotify = "0";
		
		[XmlElement(ElementName="hiddensections")]
		public string Hiddensections = "0";
		
		[XmlElement(ElementName="coursedisplay")]
		public string Coursedisplay = "0";
		
		[XmlElement(ElementName="category")]
		public CourseCourseXmlCategory Category;
		
		[XmlElement(ElementName="tags")]
		public string Tags;
		
		[XmlElement(ElementName="customfields")]
		public string Customfields = "";
		
		[XmlAttribute(AttributeName="id")]
		public string Id = "";
		
		[XmlAttribute(AttributeName="contextid")]
		public string Contextid = "";
		
	}

	public class CourseCourseXmlInit
	{
		public CourseCourseXmlCourse Init()
		{
			var currTime = DateTimeOffset.Now.ToUnixTimeSeconds(); 
			var courseCourse = new CourseCourseXmlCourse();
			courseCourse.Id = "53";
			courseCourse.Contextid = "286";
			courseCourse.Shortname = "XML_LK";
			courseCourse.Fullname = "XML_Leerer Kurs";
			courseCourse.Summaryformat = "1";
			courseCourse.Format = "topics";
			courseCourse.Showgrades = "1";
			courseCourse.Newsitems = "5";
			courseCourse.Startdate = currTime.ToString();
			courseCourse.Timecreated = currTime.ToString();
			courseCourse.Timemodified = currTime.ToString();

			var courseCourseCategory = new CourseCourseXmlCategory();
			courseCourseCategory.Name = "Miscellaneous";
			courseCourseCategory.Id = "1";

			courseCourse.Category = courseCourseCategory;
			return courseCourse;
		}
	}

}