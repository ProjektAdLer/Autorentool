namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

public interface ICourseEnrolmentsXmlEnrolments : IXmlSerializable
{
    CourseEnrolmentsXmlEnrols Enrols { get; set; }
}