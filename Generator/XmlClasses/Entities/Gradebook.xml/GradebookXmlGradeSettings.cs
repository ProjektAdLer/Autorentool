using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Gradebook.xml;

[XmlRoot(ElementName="grade_settings")]
public class GradebookXmlGradeSettings : IGradebookXmlGradeSettings
{

    public GradebookXmlGradeSettings()
    {
        GradeSetting = new GradebookXmlGradeSetting();
    }

    [XmlElement(ElementName = "grade_setting")]
    public GradebookXmlGradeSetting GradeSetting { get; set; }
}
