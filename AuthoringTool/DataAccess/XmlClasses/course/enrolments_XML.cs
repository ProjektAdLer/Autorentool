using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course

{
	[XmlRoot(ElementName="enrol")]
	public class CourseEnrolmentsXmlEnrol {
		
		[XmlElement(ElementName="enrol")]
		public string Enrolchild = "";
		
		[XmlElement(ElementName="status")]
		public string Status = "";
		
		[XmlElement(ElementName="name")]
		public string Name = "$@NULL@$";

		[XmlElement(ElementName = "enrolperiod")]
		public string Enrolperiod = "0";

		[XmlElement(ElementName = "enrolstartdate")]
		public string Enrolstartdate = "0";

		[XmlElement(ElementName = "enrolenddate")]
		public string Enrolenddate = "0";

		[XmlElement(ElementName = "expirynotify")]
		public string Expirynotify = "0";

		[XmlElement(ElementName = "expirythreshold")]
		public string Expirythreshold = "86400";

		[XmlElement(ElementName = "notifyall")]
		public string Notifyall = "0";
		
		[XmlElement(ElementName="password")]
		public string Password = "$@NULL@$";
		
		[XmlElement(ElementName="cost")]
		public string Cost = "$@NULL@$";
		
		[XmlElement(ElementName="currency")]
		public string Currency = "$@NULL@$";
		
		[XmlElement(ElementName="roleid")]
		public string Roleid ="";
		
		[XmlElement(ElementName="customint1")]
		public string Customint1 = "$@NULL@$";
		
		[XmlElement(ElementName="customint2")]
		public string Customint2 = "$@NULL@$";
		
		[XmlElement(ElementName="customint3")]
		public string Customint3 = "$@NULL@$";
		
		[XmlElement(ElementName="customint4")]
		public string Customint4 = "$@NULL@$";
		
		[XmlElement(ElementName="customint5")]
		public string Customint5 = "$@NULL@$";
		
		[XmlElement(ElementName="customint6")]
		public string Customint6 = "$@NULL@$";
		
		[XmlElement(ElementName="customint7")]
		public string Customint7 = "$@NULL@$";
		
		[XmlElement(ElementName="customint8")]
		public string Customint8 = "$@NULL@$";
		
		[XmlElement(ElementName="customchar1")]
		public string Customchar1 = "$@NULL@$";
		
		[XmlElement(ElementName="customchar2")]
		public string Customchar2 = "$@NULL@$";
		
		[XmlElement(ElementName="customchar3")]
		public string Customchar3 = "$@NULL@$";
		
		[XmlElement(ElementName="customdec1")]
		public string Customdec1 = "$@NULL@$";
		
		[XmlElement(ElementName="customdec2")]
		public string Customdec2 = "$@NULL@$";
		
		[XmlElement(ElementName="customtext1")]
		public string Customtext1 = "$@NULL@$";
		
		[XmlElement(ElementName="customtext2")]
		public string Customtext2 = "$@NULL@$";
		
		[XmlElement(ElementName="customtext3")]
		public string Customtext3 = "$@NULL@$";
		
		[XmlElement(ElementName="customtext4")]
		public string Customtext4 = "$@NULL@$";

		[XmlElement(ElementName = "timecreated")] 
		public string Timecreated = "";

		[XmlElement(ElementName = "timemodified")] 
		public string Timemodified = "";

		[XmlElement(ElementName = "user_enrolments")] 
		public string User_enrolments = "";
		
		[XmlAttribute(AttributeName="id")]
		public string Id = "";
	}

	[XmlRoot(ElementName="enrols")]
	public class CourseEnrolmentsXmlEnrols {
		[XmlElement(ElementName="enrol")] 
		public List<CourseEnrolmentsXmlEnrol> Enrol;
	}

	[XmlRoot(ElementName="enrolments")]
	public class CourseEnrolmentsXmlEnrolments
	{
		[XmlElement(ElementName = "enrols")] 
		public CourseEnrolmentsXmlEnrols Enrols;
	}

	public class CourseEnrolmentsXmlInit
	{
		public CourseEnrolmentsXmlEnrolments Init()
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
			
			return courseEnrolment;
		}
	}

}
