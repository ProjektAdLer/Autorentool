using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

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
