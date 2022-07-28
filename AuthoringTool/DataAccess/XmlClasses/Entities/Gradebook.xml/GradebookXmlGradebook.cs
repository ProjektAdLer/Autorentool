using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="gradebook")]
public partial class GradebookXmlGradebook : IGradebookXmlGradebook
{

    public GradebookXmlGradebook()
    {
        Attributes = "";
        GradeCategories = new GradebookXmlGradeCategories() ;
        GradeItems = new GradebookXmlGradeItems();
        GradeLetters = "";
        GradeSettings = new GradebookXmlGradeSettings();
    }
    

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "gradebook.xml");
    }
    
    [XmlElement(ElementName="attributes")]
    public string Attributes { get; set; }
    [XmlElement(ElementName="grade_categories")]
    public GradebookXmlGradeCategories GradeCategories { get; set; }
    [XmlElement(ElementName="grade_items")]
    public GradebookXmlGradeItems GradeItems { get; set; }
    [XmlElement(ElementName="grade_letters")]
    public string GradeLetters { get; set; }
    [XmlElement(ElementName="grade_settings")]
    public GradebookXmlGradeSettings GradeSettings { get; set; }
}