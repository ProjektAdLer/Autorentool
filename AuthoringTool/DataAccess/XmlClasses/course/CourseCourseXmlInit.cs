using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;

public class CourseCourseXmlInit : IXMLInit
{
	public void XmlInit()
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
			
			var xml = new XmlSer();
			xml.serialize(courseCourse, "course/course.xml");

	}
}
	