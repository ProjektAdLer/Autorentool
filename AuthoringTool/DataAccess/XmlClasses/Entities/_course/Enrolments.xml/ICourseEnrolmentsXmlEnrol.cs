namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Enrolments.xml;

public interface ICourseEnrolmentsXmlEnrol
{
		string EnrolMethod { get; set; }
		
		string Status { get; set; }
		
		string Name { get; set; }

		string EnrolPeriod { get; set; }

		string EnrolStartDate { get; set; }

		string EnrolEndDate { get; set; }

		string ExpiryNotify { get; set; }

		string ExpiryThreshold { get; set; }

		string NotifyAll { get; set; }
		
		string Password { get; set; }
		
		string Cost { get; set; }
		
		string Currency { get; set; }
		
		string RoleId { get; set; }
		
		string CustomInt1 { get; set; }
		
		string CustomInt2 { get; set; }
		
		string CustomInt3 { get; set; }

		string CustomInt4 { get; set; }
		
		string CustomInt5 { get; set; }
		
		string CustomInt6 { get; set; }
		
		string CustomInt7 { get; set; }
		
		string CustomInt8 { get; set; }
		
		string CustomChar1 { get; set; }

		string CustomChar2 { get; set; }
		
		string CustomChar3 { get; set; }
		
		string CustomDec1 { get; set; }
		
		string CustomDec2 { get; set; }

		string CustomText1 { get; set; }
		
		string CustomText2 { get; set; }
		
		string CustomText3 { get; set; }

		string CustomText4 { get; set; }

		string Timecreated { get; set; }

		string Timemodified { get; set; }

		string User_enrolments { get; set; }
		
		string Id { get; set; }
}