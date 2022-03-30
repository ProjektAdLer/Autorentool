using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="enrol")]
	public partial class CourseEnrolmentsXmlEnrol {

		
		public void SetParametersShort(string roleid, string id, string enrolchild, string status)
		{
			var currTime = DateTimeOffset.Now.ToUnixTimeSeconds(); 
			Roleid = roleid;
			Timecreated = currTime.ToString();
			Timemodified = currTime.ToString();
			Id = id;
			Enrolchild = enrolchild;
			Status = status;
		}
		
		public void SetParametersFull(string roleid, string id, string enrolchild, string status,
                                      			string customint1, string customint2, string customint3, string customint4, string customint5,
                                      			string customint6)
		{
			var currTime = DateTimeOffset.Now.ToUnixTimeSeconds(); 
			Roleid = roleid;
			Timecreated = currTime.ToString();
			Timemodified = currTime.ToString();
			Id = id;
			Enrolchild = enrolchild;
			Status = status;
			Customint1 = customint1;
			Customint2 = customint2;
			Customint3 = customint3;
			Customint4 = customint4;
			Customint5 = customint5;
			Customint6 = customint6;
		}

		
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