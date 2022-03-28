using AuthoringTool.DataAccess.XmlClasses.course;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlCourseFactory
{
    public XmlCourseFactory()
    {
        //Write course/course.xml file
        var courseCategory = new CourseCourseXmlCategory();
        courseCategory.SetParameters();
        var courseCourse = new CourseCourseXmlCourse();
        courseCourse.SetParameters(courseCategory);
        
        courseCourse.Serialize();

        //Write course/enrolments.xml file
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol1.SetParametersShort("5", "153", "manual", "0");
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol2.SetParametersShort("0", "154", "guest", "1");
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol3.SetParametersFull("5", "155", "self", "1", "0", "0", "0", "1", "0", "1");
        var enrolmentsEnrols = new CourseEnrolmentsXmlEnrols();
        enrolmentsEnrols.SetParameters(enrolmentsEnrol1, enrolmentsEnrol2, enrolmentsEnrol3);
        var enrolmentsEnrolments = new CourseEnrolmentsXmlEnrolments();
        enrolmentsEnrolments.SetParameters(enrolmentsEnrols);
        
        enrolmentsEnrolments.Serialize();
        
        //Write course/inforef.xml file
        var inforefRole = new CourseInforefXmlRole();
        inforefRole.SetParameters("5");
        var inforefRoleref = new CourseInforefXmlRoleref();
        inforefRoleref.SetParameters(inforefRole);
        var inforefInforef = new CourseInforefXmlInforef();
        inforefInforef.SetParameters(inforefRoleref);
        
        inforefInforef.Serialize();
        
        //Write course/roles.xml file
        var rolesRoles = new CourseRolesXmlRoles();
        rolesRoles.SetParameters();
        
        rolesRoles.Serialize();


    }
}