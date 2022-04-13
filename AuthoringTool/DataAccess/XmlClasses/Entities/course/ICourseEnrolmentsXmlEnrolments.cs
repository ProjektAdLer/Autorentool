namespace AuthoringTool.DataAccess.XmlClasses.course;

public interface ICourseEnrolmentsXmlEnrolments : IXmlSerializable
{
    void SetParameters(CourseEnrolmentsXmlEnrols? enrolsList);
}