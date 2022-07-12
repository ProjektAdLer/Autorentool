namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

public interface ICourseInforefXmlInforef : IXmlSerializable
{
    void SetParameters(CourseInforefXmlRoleref? inforefRoleref);
}