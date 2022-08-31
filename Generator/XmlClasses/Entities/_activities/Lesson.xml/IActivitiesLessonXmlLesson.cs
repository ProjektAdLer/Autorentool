namespace Generator.XmlClasses.Entities._activities.Lesson.xml;

public interface IActivitiesLessonXmlLesson
{
	string Course { get; set; }
	
	string Name { get; set; }
		
	string Intro { get; set; }
		
	string IntroFormat { get; set; }
		
	string Practice { get; set; }
		
	string Modattempts { get; set; }
		
	string Usepassword { get; set; }
		
	string Password { get; set; }
		
	string Dependency { get; set; }
		
	string Conditions { get; set; }
		
	string Grade { get; set; }
		
	string Custom { get; set; }
	
	string Ongoing { get; set; }
		
	string Usemaxgrade { get; set; }
		
	string Maxanswers { get; set; }
		
	string MaxAttempts { get; set; }
	
	string Review { get; set; }
		
	string NextPagedefault { get; set; }

	string Feedback { get; set; }
		
	string MinQuestions { get; set; }
		
	string MaxPages { get; set; }
	
	string Timelimit { get; set; }
		
	string Retake { get; set; }
		
	string Activitylink { get; set; }
		
	string Mediafile { get; set; }
		
	string Mediaheight { get; set; }
		
	string Mediawidth { get; set; }
	
	string Mediaclose { get; set; }
		
	string Slideshow { get; set; }
		
	string Width { get; set; }
		
	string Height { get; set; }

	string Bgcolor { get; set; }
		
	string Displayleft { get; set; }
		
	string Displayleftif { get; set; }
		
	string Progressbar { get; set; }

	string Available { get; set; }
		
	string Deadline { get; set; }
		
	string Timemodified { get; set; }
		
	string CompletionEndReached { get; set; }
		
	public string CompletionTimeSpent { get; set; }
		
	string AllowOfflineAttempts { get; set; }
		
	ActivitiesLessonXmlPages Pages { get; set; }
		
	string Grades { get; set; }
		
	string Timers { get; set; }
		
	string Overrides { get; set; }
		
	string Id { get; set; }
}