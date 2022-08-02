using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Gradebook.xml;


[XmlRoot(ElementName="grade_item")]
	public class GradebookXmlGradeItem : IGradebookXmlGradeItem{

		public GradebookXmlGradeItem()
		{
			CategoryId = "$@NULL@$";
			ItemName = "$@NULL@$";
			ItemType = "course";
			ItemModule = "$@NULL@$";
			ItemInstance = "1";
			ItemNumber = "$@NULL@$";
			ItemInfo = "$@NULL@$";
			IdNumber = "$@NULL@$";
			Calculation = "$@NULL@$";
			GradeType = "1";
			Grademax = "100.00000";
			Grademin = "0.00000";
			ScaleId = "$@NULL@$";
			OutcomeId = "$@NULL@$";
			Gradepass = "0.00000";
			Multfactor = "1.00000";
			Plusfactor = "0.00000";
			Aggregationcoef = "0.00000";
			Aggregationcoef2 = "0.00000";
			Weightoverride = "0";
			Sortorder = "1";
			Display = "0";
			Decimals = "$@NULL@$";
			Hidden = "0";
			Locked = "0";
			Locktime = "0";
			Needsupdate = "0";
			Timecreated = "";
			Timemodified = "";
			GradeGrades = "";
			Id = "1";
		}


		[XmlElement(ElementName="categoryid")]
		public string CategoryId { get; set; }
		
		[XmlElement(ElementName="itemname")]
		public string ItemName { get; set; }
		
		[XmlElement(ElementName="itemtype")]
		public string ItemType { get; set; }
		
		[XmlElement(ElementName="itemmodule")]
		public string ItemModule { get; set; }
		
		[XmlElement(ElementName="iteminstance")]
		public string ItemInstance { get; set; }
		
		[XmlElement(ElementName="itemnumber")]
		public string ItemNumber { get; set; }
		
		[XmlElement(ElementName="iteminfo")]
		public string ItemInfo { get; set; }
		
		[XmlElement(ElementName="idnumber")]
		public string IdNumber { get; set; }
		
		[XmlElement(ElementName="calculation")]
		public string Calculation { get; set; }
		
		[XmlElement(ElementName="gradetype")]
		public string GradeType { get; set; }
		
		[XmlElement(ElementName="grademax")]
		public string Grademax { get; set; }
		
		[XmlElement(ElementName="grademin")]
		public string Grademin { get; set; }
		
		[XmlElement(ElementName="scaleid")]
		public string ScaleId { get; set; }
		
		[XmlElement(ElementName="outcomeid")]
		public string OutcomeId { get; set; }
		
		[XmlElement(ElementName="gradepass")]
		public string Gradepass { get; set; }
		
		[XmlElement(ElementName="multfactor")]
		public string Multfactor { get; set; }
		
		[XmlElement(ElementName="plusfactor")]
		public string Plusfactor { get; set; }
		
		[XmlElement(ElementName="aggregationcoef")]
		public string Aggregationcoef { get; set; }
		
		[XmlElement(ElementName="aggregationcoef2")]
		public string Aggregationcoef2 { get; set; }
		
		[XmlElement(ElementName="weightoverride")]
		public string Weightoverride { get; set; }
		
		[XmlElement(ElementName="sortorder")]
		public string Sortorder { get; set; }
		
		[XmlElement(ElementName="display")]
		public string Display { get; set; }
		
		[XmlElement(ElementName="decimals")]
		public string Decimals { get; set; }
		
		[XmlElement(ElementName="hidden")]
		public string Hidden { get; set; }
		
		[XmlElement(ElementName="locked")]
		public string Locked { get; set; }
		
		[XmlElement(ElementName="locktime")]
		public string Locktime { get; set; }
		
		[XmlElement(ElementName="needsupdate")]
		public string Needsupdate { get; set; }
		
		[XmlElement(ElementName="timecreated")]
		public string Timecreated { get; set; }
		
		[XmlElement(ElementName="timemodified")]
		public string Timemodified { get; set; }
		
		[XmlElement(ElementName="grade_grades")]
		public string GradeGrades { get; set; }
		
		[XmlAttribute(AttributeName="id")]
		public string Id { get; set; }
		
	}