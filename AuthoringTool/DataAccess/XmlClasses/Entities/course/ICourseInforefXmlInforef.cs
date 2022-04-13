namespace AuthoringTool.DataAccess.XmlClasses.course;

public interface ICourseInforefXmlInforef : IXmlSerializable
{
    void SetParameters(CourseInforefXmlRoleref? inforefRoleref);
}