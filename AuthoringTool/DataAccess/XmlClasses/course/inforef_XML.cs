using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course

{
    [XmlRoot(ElementName="role")]
    public class CourseInforefXmlRole {
        [XmlElement(ElementName="id")]
        public string Id = "";
    }

    [XmlRoot(ElementName="roleref")]
    public class CourseInforefXmlRoleref {
        [XmlElement(ElementName="role")]
        public CourseInforefXmlRole Role;
    }

    [XmlRoot(ElementName="inforef")]
    public class CourseInforefXmlInforef {
        [XmlElement(ElementName="roleref")]
        public CourseInforefXmlRoleref Roleref;
    }

    public class CourseInforefXmlInit
    {
        public CourseInforefXmlInforef Init()
        {
            var courseInforef = new CourseInforefXmlInforef();
            var courseRoleref = new CourseInforefXmlRoleref();
            var courseInforefRole = new CourseInforefXmlRole();

            courseInforefRole.Id = "5";
            courseRoleref.Role = courseInforefRole;
            courseInforef.Roleref = courseRoleref;
            
            var xml = new XmlSer();
            xml.serialize(courseInforef, "course/inforef.xml"); 
            
            return courseInforef;
        }
    }

}