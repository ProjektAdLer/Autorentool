using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="grade_settings")]
public partial class GradebookXmlGradeSettings : IGradebookXmlGradeSettings
{

    public void SetParameters(GradebookXmlGradeSetting? gradeSetting)
    {
        grade_Setting = gradeSetting;
    }
    
    [XmlElement(ElementName = "grade_setting")]
    public GradebookXmlGradeSetting? grade_Setting;
}
