namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Enrolments.xml;

public interface ICourseEnrolmentsXmlEnrolments : IXmlSerializable
{
    CourseEnrolmentsXmlEnrols Enrols { get; set; }
}