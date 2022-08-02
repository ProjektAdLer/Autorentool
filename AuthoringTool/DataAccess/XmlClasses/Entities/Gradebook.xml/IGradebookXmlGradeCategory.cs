namespace AuthoringTool.DataAccess.XmlClasses.Entities.Gradebook.xml;

public interface IGradebookXmlGradeCategory
{

    string Parent { get; set; }
    
    string Depth { get; set; }
    
    string Path { get; set; }
    
    string Fullname { get; set; }
    
    string Aggregation { get; set; }
    
    public string Keephigh { get; set; }
    
    string Droplow { get; set; }
    
    string AggregateOnlyGraded { get; set; }
    
    string AggregateOutcomes { get; set; }

    string Timecreated { get; set; }
    
    string Timemodified { get; set; }
    
    string Hidden { get; set; }
    
    string Id { get; set; }
}