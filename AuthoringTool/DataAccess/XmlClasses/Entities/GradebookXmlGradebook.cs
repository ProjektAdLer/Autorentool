using System.IO.Abstractions;
using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;
using Microsoft.VisualBasic;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="gradebook")]
public partial class GradebookXmlGradebook : IGradebookXmlGradebook
{
    
    public void SetParameters(GradebookXmlGradeSettings? gradeSettings)
    {
        Grade_settings = gradeSettings;
    }
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "gradebook.xml");
    }
    
    [XmlElement(ElementName = "attributes")]
    public string Attributes = "";

    [XmlElement(ElementName = "grade_categories")]
    public string Grade_categories = "";

    [XmlElement(ElementName = "grade_items")]
    public string Grade_items = ""; 
        
    [XmlElement(ElementName="grade_letters")]
    public string Grade_letters = "";

    [XmlElement(ElementName = "grade_settings")]
    public GradebookXmlGradeSettings? Grade_settings;
}