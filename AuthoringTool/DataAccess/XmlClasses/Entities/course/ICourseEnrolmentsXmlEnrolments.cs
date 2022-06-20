namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

public interface ICourseEnrolmentsXmlEnrolments : IXmlSerializable
{
    void SetParameters(CourseEnrolmentsXmlEnrols? enrolsList);
}