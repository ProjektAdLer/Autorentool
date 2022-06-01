using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="activity_gradebook")]
public class ActivitiesGradesXmlActivityGradebook : IActivitiesGradesXmlActivityGradebook {
    
    public void SetParameterts(ActivitiesGradesXmlGradeItems? gradeItems, string? gradeLetters)
    {
        Grade_items = gradeItems;
        Grade_letters = gradeLetters;
    }
    
    public void Serialize(string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this,Path.Join("activities", "h5pactivity_"+moduleId, "grades.xml"));
    }

    [XmlElement(ElementName="grade_items")]
    public ActivitiesGradesXmlGradeItems? Grade_items { get; set; }
    
    [XmlElement(ElementName="grade_letters")]
    public string? Grade_letters { get; set; }
}