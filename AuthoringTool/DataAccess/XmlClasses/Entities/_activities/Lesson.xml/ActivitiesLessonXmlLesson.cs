using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="lesson")]
public class ActivitiesLessonXmlLesson : IActivitiesLessonXmlLesson{

	public ActivitiesLessonXmlLesson()
	{
		Course = "1";
		Name = "";
		Intro = "";
		Introformat = "1";
		Practice = "0";
		Modattempts = "0";
		Usepassword = "0";
		Password = "";
		Dependency = "0";
		Conditions = "";
		Grade = "100";
		Custom = "1";
		Ongoing = "0";
		Usemaxgrade = "0";
		Maxanswers = "5";
		MaxAttempts = "1";
		Review = "0";
		NextPagedefault = "0";
		Feedback = "0";
		MinQuestions = "0";
		MaxPages = "1";
		Timelimit = "0";
		Retake = "0";
		Activitylink = "0";
		Mediafile = "";
		Mediaheight = "480";
		Mediawidth = "640";
		Mediaclose = "0";
		Slideshow = "0";
		Width = "480";
		Height = "640";
		Bgcolor = "#FFFFF";
		Displayleft = "0";
		Displayleftif = "0";
		Progressbar = "0";
		Available = "0";
		Deadline = "0";
		Timemodified = "";
		Completionendreached = "0";
		Completiontimespent = "0";
		AllowOfflineAttempts = "0";
		Pages = new ActivitiesLessonXmlPages();
		Grades = "";
		Timers = "";
		Overrides = "";
		Id = "";
	}
	
	[XmlElement(ElementName="course")]
	public string Course { get; set; }
		
	[XmlElement(ElementName="name")]
	public string Name { get; set; }
		
	[XmlElement(ElementName="intro")]
	public string Intro { get; set; }
		
	[XmlElement(ElementName="introformat")]
	public string Introformat { get; set; }
		
	[XmlElement(ElementName="practice")]
	public string Practice { get; set; }
		
	[XmlElement(ElementName="modattempts")]
	public string Modattempts { get; set; }
		
	[XmlElement(ElementName="usepassword")]
	public string Usepassword { get; set; }
		
	[XmlElement(ElementName="password")]
	public string Password { get; set; }
		
	[XmlElement(ElementName="dependency")]
	public string Dependency { get; set; }
		
	[XmlElement(ElementName="conditions")]
	public string Conditions { get; set; }
		
	[XmlElement(ElementName="grade")]
	public string Grade { get; set; }
		
	[XmlElement(ElementName="custom")]
	public string Custom { get; set; }
	
	[XmlElement(ElementName="ongoing")]
	public string Ongoing { get; set; }
		
	[XmlElement(ElementName="usemaxgrade")]
	public string Usemaxgrade { get; set; }
		
	[XmlElement(ElementName="maxanswers")]
	public string Maxanswers { get; set; }
		
	[XmlElement(ElementName="maxattempts")]
	public string MaxAttempts { get; set; }
	
	[XmlElement(ElementName="review")]
	public string Review { get; set; }
		
	[XmlElement(ElementName="nextpagedefault")]
	public string NextPagedefault { get; set; }
		
	[XmlElement(ElementName="feedback")]
	public string Feedback { get; set; }
		
	[XmlElement(ElementName="minquestions")]
	public string MinQuestions { get; set; }
		
	[XmlElement(ElementName="maxpages")]
	public string MaxPages { get; set; }
	
	[XmlElement(ElementName="timelimit")]
	public string Timelimit { get; set; }
		
	[XmlElement(ElementName="retake")]
	public string Retake { get; set; }
		
	[XmlElement(ElementName="activitylink")]
	public string Activitylink { get; set; }
		
	[XmlElement(ElementName="mediafile")]
	public string Mediafile { get; set; }
		
	[XmlElement(ElementName="mediaheight")]
	public string Mediaheight { get; set; }
		
	[XmlElement(ElementName="mediawidth")]
	public string Mediawidth { get; set; }
	
	[XmlElement(ElementName="mediaclose")]
	public string Mediaclose { get; set; }
		
	[XmlElement(ElementName="slideshow")]
	public string Slideshow { get; set; }
		
	[XmlElement(ElementName="width")]
	public string Width { get; set; }
		
	[XmlElement(ElementName="height")]
	public string Height { get; set; }
		
	[XmlElement(ElementName="bgcolor")]
	public string Bgcolor { get; set; }
		
	[XmlElement(ElementName="displayleft")]
	public string Displayleft { get; set; }
		
	[XmlElement(ElementName="displayleftif")]
	public string Displayleftif { get; set; }
		
	[XmlElement(ElementName="progressbar")]
	public string Progressbar { get; set; }
		
	[XmlElement(ElementName="available")]
	public string Available { get; set; }
		
	[XmlElement(ElementName="deadline")]
	public string Deadline { get; set; }
		
	[XmlElement(ElementName="timemodified")]
	public string Timemodified { get; set; }
		
	[XmlElement(ElementName="completionendreached")]
	public string Completionendreached { get; set; }
		
	[XmlElement(ElementName="completiontimespent")]
	public string Completiontimespent { get; set; }
		
	[XmlElement(ElementName="allowofflineattempts")]
	public string AllowOfflineAttempts { get; set; }
		
	[XmlElement(ElementName="pages")]
	public ActivitiesLessonXmlPages Pages { get; set; }
		
	[XmlElement(ElementName="grades")]
	public string Grades { get; set; }
		
	[XmlElement(ElementName="timers")]
	public string Timers { get; set; }
		
	[XmlElement(ElementName="overrides")]
	public string Overrides { get; set; }
		
	[XmlAttribute(AttributeName="id")]
	public string Id { get; set; }
	
}
