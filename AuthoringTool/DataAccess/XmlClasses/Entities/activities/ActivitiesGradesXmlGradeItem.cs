using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName = "grade_item")]
public class ActivitiesGradesXmlGradeItem: IActivitiesGradesXmlGradeItem{
	
	public void SetParameters(string? categoryid, string? itemname, string? itemtype, string? itemmodule, string? iteminstance, 
		string? itemnumber, string? iteminfo, string? idnumber, string? calculation, string? gradetype, string? grademax, 
		string? grademin, string? scaleid, string? outcomeid, string? gradepass, string? multfactor, string? plusfactor, 
		string? aggregationcoef, string? aggregationcoef2, string? weightoverride, string? sortorder, string? display, 
		string? decimals, string? hidden, string? locked, string? locktime, string? needsupdate, string? timecreated, 
		string? timemodified, string? gradeGrades, string? id)
	{
		Categoryid = categoryid;
		Itemname = itemname;
		Itemtype = itemtype;
		Itemmodule = itemmodule;
		Iteminstance = iteminstance;
		Itemnumber = itemnumber;
		Iteminfo = iteminfo;
		Idnumber = idnumber;
		Calculation = calculation;
		Gradetype = gradetype;
		Grademax = grademax;
		Grademin = grademin;
		Scaleid = scaleid;
		Outcomeid = outcomeid;
		Gradepass = gradepass;
		Multfactor = multfactor;
		Plusfactor = plusfactor;
		Aggregationcoef = aggregationcoef;
		Aggregationcoef2 = aggregationcoef2;
		Weightoverride = weightoverride;
		Sortorder = sortorder;
		Display = display;
		Decimals = decimals;
		Hidden = hidden;
		Locked = locked;
		Locktime = locktime;
		Needsupdate = needsupdate;
		Timecreated = timecreated;
		Timemodified = timemodified;
		Grade_grades = gradeGrades;
		Id = id;
	}

		[XmlElement(ElementName="categoryid")]
		public string? Categoryid { get; set; }
		
		[XmlElement(ElementName="itemname")]
		public string? Itemname { get; set; }
		
		[XmlElement(ElementName="itemtype")]
		public string? Itemtype { get; set; }
		
		[XmlElement(ElementName="itemmodule")]
		public string? Itemmodule { get; set; }
		
		[XmlElement(ElementName="iteminstance")]
		public string? Iteminstance { get; set; }
		
		[XmlElement(ElementName="itemnumber")]
		public string? Itemnumber { get; set; }
		
		[XmlElement(ElementName="iteminfo")]
		public string? Iteminfo { get; set; }
		
		[XmlElement(ElementName="idnumber")]
		public string? Idnumber { get; set; }
		
		[XmlElement(ElementName="calculation")]
		public string? Calculation { get; set; }
		
		[XmlElement(ElementName="gradetype")]
		public string? Gradetype { get; set; }
		
		[XmlElement(ElementName="grademax")]
		public string? Grademax { get; set; }
		
		[XmlElement(ElementName="grademin")]
		public string? Grademin { get; set; }
		
		[XmlElement(ElementName="scaleid")]
		public string? Scaleid { get; set; }
		
		[XmlElement(ElementName="outcomeid")]
		public string? Outcomeid { get; set; }
		
		[XmlElement(ElementName="gradepass")]
		public string? Gradepass { get; set; }
		
		[XmlElement(ElementName="multfactor")]
		public string? Multfactor { get; set; }
		
		[XmlElement(ElementName="plusfactor")]
		public string? Plusfactor { get; set; }
		
		[XmlElement(ElementName="aggregationcoef")]
		public string? Aggregationcoef { get; set; }
		
		[XmlElement(ElementName="aggregationcoef2")]
		public string? Aggregationcoef2 { get; set; }
		
		[XmlElement(ElementName="weightoverride")]
		public string? Weightoverride { get; set; }
		
		[XmlElement(ElementName="sortorder")]
		public string? Sortorder { get; set; }
		
		[XmlElement(ElementName="display")]
		public string? Display { get; set; }
		
		[XmlElement(ElementName="decimals")]
		public string? Decimals { get; set; }
		
		[XmlElement(ElementName="hidden")]
		public string? Hidden { get; set; }
		
		[XmlElement(ElementName="locked")]
		public string? Locked { get; set; }
		
		[XmlElement(ElementName="locktime")]
		public string? Locktime { get; set; }
		
		[XmlElement(ElementName="needsupdate")]
		public string? Needsupdate { get; set; }
		
		[XmlElement(ElementName="timecreated")]
		public string? Timecreated { get; set; }
		
		[XmlElement(ElementName="timemodified")]
		public string? Timemodified { get; set; }
		
		[XmlElement(ElementName="grade_grades")]
		public string? Grade_grades { get; set; }
		
		[XmlAttribute(AttributeName="id")]
		public string? Id { get; set; }
		
	}