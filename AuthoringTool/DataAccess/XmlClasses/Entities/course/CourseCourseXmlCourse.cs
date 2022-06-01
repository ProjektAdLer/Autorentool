using System;
using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="course")]
	public partial class CourseCourseXmlCourse : ICourseCourseXmlCourse
	{
		public void SetParameters(string? shortname, string? fullname, string idnumber, string summary, 
			string summaryformat, string format, string showgrades, string newsitems, string startdate, string enddate, 
			string marker, string maxbytes, string legacyfiles, string showreports, string visible, string groupmode, 
			string groupmodeforce, string defaultgroupingid, string lang, string theme, string timecreated, 
			string timemodified, string requested, string showactivitydates, string showcompletionconditions, 
			string enablecompletion, string completionnotify, string hiddensections, string coursedisplay, 
			CourseCourseXmlCategory? category, string? tags, string customfields, string id, string contextid)
		{
			Shortname = shortname;
			Fullname = fullname;
			Idnumber = idnumber;
			Summary = summary;
			Summaryformat = summaryformat;
			Format = format;
			Showgrades = showgrades;
			Newsitems = newsitems;
			Startdate = startdate;
			Enddate = enddate;
			Marker = marker;
			Maxbytes = maxbytes;
			Legacyfiles = legacyfiles;
			Showreports = showreports;
			Visible = visible;
			Groupmode = groupmode;
			Groupmodeforce = groupmodeforce;
			Defaultgroupingid = defaultgroupingid;
			Lang = lang;
			Theme = theme;
			Timecreated = timecreated;
			Timemodified = timemodified;
			Requested = requested;
			Showactivitydates = showactivitydates;
			Showcompletionconditions = showcompletionconditions;
			Enablecompletion = enablecompletion;
			Completionnotify = completionnotify;
			Hiddensections = hiddensections;
			Coursedisplay = coursedisplay;
			Category = category;
			Tags = tags;
			Customfields = customfields;
			Id = id;
			Contextid = contextid;
		}

		public void Serialize()
		{
			var xml = new XmlSerialize();
			xml.Serialize(this, "course/course.xml");
		}

		[XmlElement(ElementName = "shortname")]
		public string? Shortname = "";
		
		[XmlElement(ElementName="fullname")]
		public string? Fullname = "";
		
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
		public CourseCourseXmlCategory? Category;
		
		[XmlElement(ElementName="tags")]
		public string? Tags;
		
		[XmlElement(ElementName="customfields")]
		public string Customfields = "";
		
		[XmlAttribute(AttributeName="id")]
		public string Id = "";
		
		[XmlAttribute(AttributeName="contextid")]
		public string Contextid = "";
		
	}
	