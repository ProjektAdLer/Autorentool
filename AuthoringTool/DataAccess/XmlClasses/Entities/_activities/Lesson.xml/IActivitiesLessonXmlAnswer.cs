namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesLessonXmlAnswer
{
    string JumpTo { get; set; }
        
    string Grade { get; set; }

    string Score { get; set; }
        
    string Flags { get; set; }
        
    string Timecreated { get; set; }
    
    string Timemodified { get; set; }
        
    string Answer_text { get; set; }
        
    string Response { get; set; }
        
    string Answerformat { get; set; }
        
    string Responseformat { get; set; }
    
    string Attempts { get; set; }
    
    string Id { get; set; }
}