using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses
{
    [XmlRoot(ElementName="grade_setting")]
    public class GradebookXmlGradeSetting {
        [XmlElement(ElementName="name")]
        public string Name = "";
        
        [XmlElement(ElementName="value")]
        public string Value = "";
        
        [XmlAttribute(AttributeName="id")]
        public string Id = "";
    }

    [XmlRoot(ElementName="grade_settings")]
    public class GradebookXmlGradeSettings
    {
        [XmlElement(ElementName = "grade_setting")]
        public GradebookXmlGradeSetting Grade_setting;
    }

    [XmlRoot(ElementName="gradebook")]
    public class GradebookXmlGradebook
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

    public class GradebookXmlInit
    {
        public GradebookXmlGradebook Init()
        {
            var gradebook = new GradebookXmlGradebook();
            var gradesettings = new GradebookXmlGradeSettings();
            var gradesetting = new GradebookXmlGradeSetting();
            gradesetting.Name = "minmaxtouse";
            gradesetting.Value = "1";

            gradesettings.Grade_setting = gradesetting;
            gradebook.Grade_settings = gradesettings;
            
            return gradebook;
        }
    }

}