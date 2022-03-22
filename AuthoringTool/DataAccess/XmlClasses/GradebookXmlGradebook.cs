using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="gradebook")]
public partial class GradebookXmlGradebook
{
    [XmlElement(ElementName = "attributes")]
    public string Attributes = "";

    [XmlElement(ElementName = "grade_categories")]
    public string Grade_categories = "";

    [XmlElement(ElementName = "grade_items")]
    public string Grade_items = ""; 
        
    [XmlElement(ElementName="grade_letters")]
    public string Grade_letters = "";

    [XmlElement(ElementName = "grade_settings")]
    public GradebookXmlGradeSettings Grade_settings;
}