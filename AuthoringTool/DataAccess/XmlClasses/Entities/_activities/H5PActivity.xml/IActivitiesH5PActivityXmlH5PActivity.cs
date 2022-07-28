namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesH5PActivityXmlH5PActivity
{
    string Name { get; set; }
    
    string Timecreated { get; set; }
    
    string Timemodified { get; set; }
    
    string Intro { get; set; }
    
    string Introformat { get; set; }
    
    string Grade { get; set; }
    
    string Displayoptions { get; set; }
    
    string Enabletracking { get; set; }
    
    string Grademethod { get; set; }
    
    string Reviewmode { get; set; }

    string Attempts { get; set; }
    
    string Id { get; set; }
}