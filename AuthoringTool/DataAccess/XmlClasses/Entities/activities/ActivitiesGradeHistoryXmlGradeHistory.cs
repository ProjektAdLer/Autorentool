using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;



[XmlRoot(ElementName="grade_history")]
public class ActivitiesGradeHistoryXmlGradeHistory : IActivitiesGradeHistoryXmlGradeHistory{

    public void SetParameterts(string? gradeGrades)
    {
        Grade_grades = gradeGrades;
    }
    
    
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "grade_history.xml"));
    }
        
    [XmlElement(ElementName="grade_grades")]
    public string? Grade_grades { get; set; }
        
}
