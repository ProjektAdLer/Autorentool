namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesLessonXmlPage
{
    string PrevPageId { get; set; }
        
    string NextPageId { get; set; }
        
    string Qtype { get; set; }
        
    string Qoption { get; set; }
        
    string Layout { get; set; }
        
    string Display { get; set; }
        
    string Timecreated { get; set; }
        
    string Timemodified { get; set; }
        
    string Title { get; set; }
        
    string Contents { get; set; }
        
    string ContentsFormat { get; set; }

    ActivitiesLessonXmlAnswers Answers { get; set; }
        
    string Branches { get; set; }
        
    string Id { get; set; }
}