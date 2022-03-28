using AuthoringTool.DataAccess.XmlClasses.course;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlCourseFactory
{
    public void WriteCourseXml()
    {
        //Write course/course.xml file
        var courseCategory = new CourseCourseXmlCategory();
        var courseCourse = new CourseCourseXmlCourse(courseCategory);
        
        courseCourse.Serialize();

        //Write course/enrolments.xml file
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol("5", "153", "manual", "0");
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol("0", "154", "guest", "1");
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol("5", "155", "self", "1", "0", "0", "0", "1", "0", "1");
        var enrolmentsEnrols = new CourseEnrolmentsXmlEnrols(enrolmentsEnrol1, enrolmentsEnrol2, enrolmentsEnrol3);
        var enrolmentsEnrolments = new CourseEnrolmentsXmlEnrolments(enrolmentsEnrols);
        
        enrolmentsEnrolments.Serialize();
        
        //Write course/inforef.xml file
        var inforefRole = new CourseInforefXmlRole("5");
        var inforefRoleref = new CourseInforefXmlRoleref(inforefRole);
        var inforefInforef = new CourseInforefXmlInforef(inforefRoleref);
        
        inforefInforef.Serialize();
        
        //Write course/roles.xml file
        var rolesRoles = new CourseRolesXmlRoles();
        
        rolesRoles.Serialize();


    }
}