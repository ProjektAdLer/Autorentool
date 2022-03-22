using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;

public class CourseEnrolmentsXmlInit : IXMLInit
{
	public void XmlInit()
	{
			var currTime = DateTimeOffset.Now.ToUnixTimeSeconds(); 
			var courseEnrol1 = new CourseEnrolmentsXmlEnrol();
			courseEnrol1.Roleid = "5";
			courseEnrol1.Timecreated = currTime.ToString();
			courseEnrol1.Timemodified = currTime.ToString();
			courseEnrol1.Id = "153";
			courseEnrol1.Enrolchild = "manual";
			courseEnrol1.Status = "0";
            
			var courseEnrol2 = new CourseEnrolmentsXmlEnrol();
			courseEnrol2.Roleid = "0";
			courseEnrol2.Timecreated = currTime.ToString();
			courseEnrol2.Timemodified = currTime.ToString();
			courseEnrol2.Id = "154";
			courseEnrol2.Enrolchild = "guest";
			courseEnrol2.Status = "1";
            
			var courseEnrol3 = new CourseEnrolmentsXmlEnrol();
			courseEnrol3.Roleid = "5";
			courseEnrol3.Timecreated = currTime.ToString();
			courseEnrol3.Timemodified = currTime.ToString();
			courseEnrol3.Id = "155";
			courseEnrol3.Enrolchild = "self";
			courseEnrol3.Status = "1";
			courseEnrol3.Customint1 = "0";
			courseEnrol3.Customint2 = "0";
			courseEnrol3.Customint3 = "0";
			courseEnrol3.Customint4 = "1";
			courseEnrol3.Customint5 = "0";
			courseEnrol3.Customint6 = "1";

			var courseEnrols = new CourseEnrolmentsXmlEnrols();
			courseEnrols.Enrol = new List<CourseEnrolmentsXmlEnrol>();
			courseEnrols.Enrol.Add(courseEnrol1);
			courseEnrols.Enrol.Add(courseEnrol2);
			courseEnrols.Enrol.Add(courseEnrol3);

			var courseEnrolment = new CourseEnrolmentsXmlEnrolments();
			courseEnrolment.Enrols = courseEnrols;
			
			var xml = new XmlSer();
			xml.serialize(courseEnrolment, "course/enrolments.xml");

	}
}

