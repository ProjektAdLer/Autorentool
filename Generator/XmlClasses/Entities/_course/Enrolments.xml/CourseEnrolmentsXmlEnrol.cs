using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._course.Enrolments.xml;

[XmlRoot(ElementName="enrol")]
	public class CourseEnrolmentsXmlEnrol : ICourseEnrolmentsXmlEnrol{

		
		public CourseEnrolmentsXmlEnrol()
		{
			Id = "";
			EnrolMethod = "";
			Status = "";
			Name = "$@NULL@$";
			EnrolPeriod = "0";
			EnrolStartDate = "0";
			EnrolEndDate = "0";
			ExpiryNotify = "0";
			ExpiryThreshold = "86400";
			NotifyAll = "0";
			Password = "$@NULL@$";
			Cost = "$@NULL@$";
			Currency = "$@NULL@$";
			RoleId = "";
			CustomInt1 = "$@NULL@$";
			CustomInt2 = "$@NULL@$";
			CustomInt3 = "$@NULL@$";
			CustomInt4 = "$@NULL@$";
			CustomInt5 = "$@NULL@$";
			CustomInt6 = "$@NULL@$";
			CustomInt7 = "$@NULL@$";
			CustomInt8 = "$@NULL@$";
			CustomChar1 = "$@NULL@$";
			CustomChar2 = "$@NULL@$";
			CustomChar3 = "$@NULL@$";
			CustomDec1 = "$@NULL@$";
			CustomDec2 = "$@NULL@$";
			CustomText1 = "$@NULL@$";
			CustomText2 = "$@NULL@$";
			CustomText3 = "$@NULL@$";
			CustomText4 = "$@NULL@$";
			Timecreated = "";
			Timemodified = "";
			User_enrolments = "";

		}
		
		[XmlElement(ElementName="enrol")]
		public string EnrolMethod { get; set; }
		
		[XmlElement(ElementName="status")]
		public string Status { get; set; }
		
		[XmlElement(ElementName="name")]
		public string Name { get; set; }

		[XmlElement(ElementName = "enrolperiod")]
		public string EnrolPeriod { get; set; }

		[XmlElement(ElementName = "enrolstartdate")]
		public string EnrolStartDate { get; set; }

		[XmlElement(ElementName = "enrolenddate")]
		public string EnrolEndDate { get; set; }

		[XmlElement(ElementName = "expirynotify")]
		public string ExpiryNotify { get; set; }

		[XmlElement(ElementName = "expirythreshold")]
		public string ExpiryThreshold { get; set; }

		[XmlElement(ElementName = "notifyall")]
		public string NotifyAll { get; set; }
		
		[XmlElement(ElementName="password")]
		public string Password { get; set; }
		
		[XmlElement(ElementName="cost")]
		public string Cost { get; set; }
		
		[XmlElement(ElementName="currency")]
		public string Currency { get; set; }
		
		[XmlElement(ElementName="roleid")]
		public string RoleId { get; set; }
		
		[XmlElement(ElementName="customint1")]
		public string CustomInt1 { get; set; }
		
		[XmlElement(ElementName="customint2")]
		public string CustomInt2 { get; set; }
		
		[XmlElement(ElementName="customint3")]
		public string CustomInt3 { get; set; }
		
		[XmlElement(ElementName="customint4")]
		public string CustomInt4 { get; set; }
		
		[XmlElement(ElementName="customint5")]
		public string CustomInt5 { get; set; }
		
		[XmlElement(ElementName="customint6")]
		public string CustomInt6 { get; set; }
		
		[XmlElement(ElementName="customint7")]
		public string CustomInt7 { get; set; }
		
		[XmlElement(ElementName="customint8")]
		public string CustomInt8 { get; set; }
		
		[XmlElement(ElementName="customchar1")]
		public string CustomChar1 { get; set; }
		
		[XmlElement(ElementName="customchar2")]
		public string CustomChar2 { get; set; }
		
		[XmlElement(ElementName="customchar3")]
		public string CustomChar3 { get; set; }
		
		[XmlElement(ElementName="customdec1")]
		public string CustomDec1 { get; set; }
		
		[XmlElement(ElementName="customdec2")]
		public string CustomDec2 { get; set; }
		
		[XmlElement(ElementName="customtext1")]
		public string CustomText1 { get; set; }
		
		[XmlElement(ElementName="customtext2")]
		public string CustomText2 { get; set; }
		
		[XmlElement(ElementName="customtext3")]
		public string CustomText3 { get; set; }
		
		[XmlElement(ElementName="customtext4")]
		public string CustomText4 { get; set; }

		[XmlElement(ElementName = "timecreated")] 
		public string Timecreated { get; set; }

		[XmlElement(ElementName = "timemodified")] 
		public string Timemodified { get; set; }

		[XmlElement(ElementName = "user_enrolments")] 
		public string User_enrolments { get; set; }
		
		[XmlAttribute(AttributeName="id")]
		public string Id { get; set; }
	}