namespace Generator.XmlClasses;

public interface IXmlCourseFactory
{
    /// <summary>
    /// Use all the methods of the current class
    /// </summary>
    void CreateXmlCourseFactory();

    void CreateCourseCourseXml();
    void CreateCourseEnrolmentsXml();
    void CreateCourseInforefXml();
    void CreateCourseRolesXml();
    void CreateCourseCompletiondefault();
}