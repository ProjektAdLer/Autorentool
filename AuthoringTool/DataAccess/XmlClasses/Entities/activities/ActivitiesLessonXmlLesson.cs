using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="lesson")]
public class ActivitiesLessonXmlLesson : IActivitiesLessonXmlLesson{
	
	public void SetParameters(string? course, string? name, string? intro, string? introformat, 
		string? practice, string? modattempts, string? usepassword, string? password, string? dependency, 
		string? conditions, string? grade, string? custom, string? ongoing, string? usemaxgrade, string? maxanswers, 
		string? maxattempts, string? review, string? nextpagedefault, string? feedback, string? minquestions, 
		string? maxpages, string? timelimit, string? retake, string? activitylink, string? mediafile, 
		string? mediaheight, string? mediawidth, string? mediaclose, string? slideshow, string? width, 
		string? height, string? bgcolor, string? displayleft, string? displayleftif, string? progressbar, 
		string? available, string? deadline, string? timemodified, string? completionendreached, 
		string? completiontimespent, string? allowofflineattempts, ActivitiesLessonXmlPages? pages, 
		string? grades, string? timers, string? overrides, string? id)
	{
		Course = course;
		Name = name;
		Intro = intro;
		Introformat = introformat;
		Practice = practice;
		Modattempts = modattempts;
		Usepassword = usepassword;
		Password = password;
		Dependency = dependency;
		Conditions = conditions;
		Grade = grade;
		Custom = custom;
		Ongoing = ongoing;
		Usemaxgrade = usemaxgrade;
		Maxanswers = maxanswers;
		Maxattempts = maxattempts;
		Review = review;
		Nextpagedefault = nextpagedefault;
		Feedback = feedback;
		Minquestions = minquestions;
		Maxpages = maxpages;
		Timelimit = timelimit;
		Retake = retake;
		Activitylink = activitylink;
		Mediafile = mediafile;
		Mediaheight = mediaheight;
		Mediawidth = mediawidth;
		Mediaclose = mediaclose;
		Slideshow = slideshow;
		Width = width;
		Height = height;
		Bgcolor = bgcolor;
		Displayleft = displayleft;
		Displayleftif = displayleftif;
		Progressbar = progressbar;
		Available = available;
		Deadline = deadline;
		Timemodified = timemodified;
		Completionendreached = completionendreached;
		Completiontimespent = completiontimespent;
		Allowofflineattempts = allowofflineattempts;
		Pages = pages;
		Grades = grades;
		Timers = timers;
		Overrides = overrides;
		Id = id;
	}

	[XmlElement(ElementName="course")]
	public string? Course { get; set; }
		
	[XmlElement(ElementName="name")]
	public string? Name { get; set; }
		
	[XmlElement(ElementName="intro")]
	public string? Intro { get; set; }
		
	[XmlElement(ElementName="introformat")]
	public string? Introformat { get; set; }
		
	[XmlElement(ElementName="practice")]
	public string? Practice { get; set; }
		
	[XmlElement(ElementName="modattempts")]
	public string? Modattempts { get; set; }
		
	[XmlElement(ElementName="usepassword")]
	public string? Usepassword { get; set; }
		
	[XmlElement(ElementName="password")]
	public string? Password { get; set; }
		
	[XmlElement(ElementName="dependency")]
	public string? Dependency { get; set; }
		
	[XmlElement(ElementName="conditions")]
	public string? Conditions { get; set; }
		
	[XmlElement(ElementName="grade")]
	public string? Grade { get; set; }
		
	[XmlElement(ElementName="custom")]
	public string? Custom { get; set; }
	
	[XmlElement(ElementName="ongoing")]
	public string? Ongoing { get; set; }
		
	[XmlElement(ElementName="usemaxgrade")]
	public string? Usemaxgrade { get; set; }
		
	[XmlElement(ElementName="maxanswers")]
	public string? Maxanswers { get; set; }
		
	[XmlElement(ElementName="maxattempts")]
	public string? Maxattempts { get; set; }
	
	[XmlElement(ElementName="review")]
	public string? Review { get; set; }
		
	[XmlElement(ElementName="nextpagedefault")]
	public string? Nextpagedefault { get; set; }
		
	[XmlElement(ElementName="feedback")]
	public string? Feedback { get; set; }
		
	[XmlElement(ElementName="minquestions")]
	public string? Minquestions { get; set; }
		
	[XmlElement(ElementName="maxpages")]
	public string? Maxpages { get; set; }
	
	[XmlElement(ElementName="timelimit")]
	public string? Timelimit { get; set; }
		
	[XmlElement(ElementName="retake")]
	public string? Retake { get; set; }
		
	[XmlElement(ElementName="activitylink")]
	public string? Activitylink { get; set; }
		
	[XmlElement(ElementName="mediafile")]
	public string? Mediafile { get; set; }
		
	[XmlElement(ElementName="mediaheight")]
	public string? Mediaheight { get; set; }
		
	[XmlElement(ElementName="mediawidth")]
	public string? Mediawidth { get; set; }
	
	[XmlElement(ElementName="mediaclose")]
	public string? Mediaclose { get; set; }
		
	[XmlElement(ElementName="slideshow")]
	public string? Slideshow { get; set; }
		
	[XmlElement(ElementName="width")]
	public string? Width { get; set; }
		
	[XmlElement(ElementName="height")]
	public string? Height { get; set; }
		
	[XmlElement(ElementName="bgcolor")]
	public string? Bgcolor { get; set; }
		
	[XmlElement(ElementName="displayleft")]
	public string? Displayleft { get; set; }
		
	[XmlElement(ElementName="displayleftif")]
	public string? Displayleftif { get; set; }
		
	[XmlElement(ElementName="progressbar")]
	public string? Progressbar { get; set; }
		
	[XmlElement(ElementName="available")]
	public string? Available { get; set; }
		
	[XmlElement(ElementName="deadline")]
	public string? Deadline { get; set; }
		
	[XmlElement(ElementName="timemodified")]
	public string? Timemodified { get; set; }
		
	[XmlElement(ElementName="completionendreached")]
	public string? Completionendreached { get; set; }
		
	[XmlElement(ElementName="completiontimespent")]
	public string? Completiontimespent { get; set; }
		
	[XmlElement(ElementName="allowofflineattempts")]
	public string? Allowofflineattempts { get; set; }
		
	[XmlElement(ElementName="pages")]
	public ActivitiesLessonXmlPages? Pages { get; set; }
		
	[XmlElement(ElementName="grades")]
	public string? Grades { get; set; }
		
	[XmlElement(ElementName="timers")]
	public string? Timers { get; set; }
		
	[XmlElement(ElementName="overrides")]
	public string? Overrides { get; set; }
		
	[XmlAttribute(AttributeName="id")]
	public string? Id { get; set; }
	
}
