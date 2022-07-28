namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

public interface ICourseInforefXmlInforef : IXmlSerializable
{
    CourseInforefXmlRoleref Roleref { get; set; }
}