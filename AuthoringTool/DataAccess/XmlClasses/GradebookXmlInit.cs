using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses
{
    public class GradebookXmlInit : IXMLInit
    {
        public void XmlInit()
        {
            var gradebook = new GradebookXmlGradebook();
            var gradesettings = new GradebookXmlGradeSettings();
            var gradesetting = new GradebookXmlGradeSetting();
            gradesetting.Name = "minmaxtouse";
            gradesetting.Value = "1";

            gradesettings.Grade_setting = gradesetting;
            gradebook.Grade_settings = gradesettings;
            
            var xml = new XmlSer();
            xml.serialize(gradebook, "gradebook.xml");
            
        }
    }

}