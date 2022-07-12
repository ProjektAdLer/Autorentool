using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="gradebook")]
public partial class GradebookXmlGradebook : IGradebookXmlGradebook
{
    public void SetParameters(string? attributes, GradebookXmlGradeCategories? gradeCategories, 
        GradebookXmlGradeItems? gradeItems, string? gradeLetters, GradebookXmlGradeSettings? gradeSettings)
    {
        Attributes = attributes;
        Grade_categories = gradeCategories;
        Grade_items = gradeItems;
        Grade_letters = gradeLetters;
        Grade_settings = gradeSettings;
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "gradebook.xml");
    }
    
    [XmlElement(ElementName="attributes")]
    public string? Attributes { get; set; }
    [XmlElement(ElementName="grade_categories")]
    public GradebookXmlGradeCategories? Grade_categories { get; set; }
    [XmlElement(ElementName="grade_items")]
    public GradebookXmlGradeItems? Grade_items { get; set; }
    [XmlElement(ElementName="grade_letters")]
    public string? Grade_letters { get; set; }
    [XmlElement(ElementName="grade_settings")]
    public GradebookXmlGradeSettings? Grade_settings { get; set; }
}