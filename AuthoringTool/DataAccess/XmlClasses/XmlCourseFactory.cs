using AuthoringTool.DataAccess.XmlClasses.course;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlCourseFactory
{
    internal ICourseCourseXmlCategory CourseCourseXmlCategory { get; }
    internal ICourseCourseXmlCourse CourseCourseXmlCourse { get; }
    internal ICourseEnrolmentsXmlEnrol CourseEnrolmentsXmlEnrolManual { get; }
    internal ICourseEnrolmentsXmlEnrol CourseEnrolmentsXmlEnrolGuest { get; }
    internal ICourseEnrolmentsXmlEnrol CourseEnrolmentsXmlEnrolSelf { get; }
    internal ICourseEnrolmentsXmlEnrolments CourseEnrolmentsXmlEnrolments { get; }
    internal ICourseEnrolmentsXmlEnrols CourseEnrolmentsXmlEnrols { get; }
    internal ICourseInforefXmlInforef CourseInforefXmlInforef { get; }
    internal ICourseInforefXmlRole CourseInforefXmlRole { get; }
    internal ICourseInforefXmlRoleref CourseInforefXmlRoleref { get; }
    internal ICourseRolesXmlRoles CourseRolesXmlRoles { get; }
    
    public XmlCourseFactory()
    {
        CourseCourseXmlCategory = new CourseCourseXmlCategory();
        CourseCourseXmlCourse = new CourseCourseXmlCourse();

        CourseEnrolmentsXmlEnrolManual = new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrolGuest = new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrolSelf = new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrolments = new CourseEnrolmentsXmlEnrolments();
        CourseEnrolmentsXmlEnrols = new CourseEnrolmentsXmlEnrols();

        CourseInforefXmlInforef = new CourseInforefXmlInforef();
        CourseInforefXmlRole = new CourseInforefXmlRole();
        CourseInforefXmlRoleref = new CourseInforefXmlRoleref();

        CourseRolesXmlRoles = new CourseRolesXmlRoles();
    }

    //Just for testing
    public XmlCourseFactory(ICourseCourseXmlCategory courseCourseXmlCategory, ICourseCourseXmlCourse courseCourseXmlCourse,
        ICourseEnrolmentsXmlEnrol courseEnrolmentsXmlEnrol, ICourseEnrolmentsXmlEnrols courseEnrolmentsXmlEnrols, 
        ICourseEnrolmentsXmlEnrolments courseEnrolmentsXmlEnrolments, ICourseInforefXmlRole courseInforefXmlRole, 
        ICourseInforefXmlRoleref courseInforefXmlRoleref, ICourseInforefXmlInforef courseInforefXmlInforef, ICourseRolesXmlRoles courseRolesXmlRoles)
    {
        CourseCourseXmlCategory = courseCourseXmlCategory;
        CourseCourseXmlCourse = courseCourseXmlCourse;

        CourseEnrolmentsXmlEnrolManual = courseEnrolmentsXmlEnrol;
        CourseEnrolmentsXmlEnrolGuest = courseEnrolmentsXmlEnrol;
        CourseEnrolmentsXmlEnrolSelf = courseEnrolmentsXmlEnrol;
        CourseEnrolmentsXmlEnrols = courseEnrolmentsXmlEnrols;
        CourseEnrolmentsXmlEnrolments = courseEnrolmentsXmlEnrolments;

        CourseInforefXmlRole = courseInforefXmlRole;
        CourseInforefXmlRoleref = courseInforefXmlRoleref;
        CourseInforefXmlInforef = courseInforefXmlInforef;

        CourseRolesXmlRoles = courseRolesXmlRoles;
    }
    
    public void CreateXmlCourseFactory()
    {
        CreateCourseCourseXml();
        CreateCourseEnrolmentsXml();
        CreateCourseInforefXml();
        CreateCourseRolesXml();
    }
    
    public void CreateCourseCourseXml()
    {
        //Write course/course.xml file
        CourseCourseXmlCategory.SetParameters("Miscellaneous", "$@NULL@$", "1"); 
        CourseCourseXmlCourse.SetParameters(CourseCourseXmlCategory as CourseCourseXmlCategory);
        
        CourseCourseXmlCourse.Serialize();
    }

    public void CreateCourseEnrolmentsXml()
    {
        //Write course/enrolments.xml file
        CourseEnrolmentsXmlEnrolManual.SetParametersShort("5", "153", "manual", "0");
        CourseEnrolmentsXmlEnrolGuest.SetParametersShort("0", "154", "guest", "1");
        CourseEnrolmentsXmlEnrolSelf.SetParametersFull("5", "155", "self", "1", "0", "0", "0", "1", "0", "1");
        CourseEnrolmentsXmlEnrols.SetParameters(CourseEnrolmentsXmlEnrolManual as CourseEnrolmentsXmlEnrol, 
            CourseEnrolmentsXmlEnrolGuest as CourseEnrolmentsXmlEnrol, CourseEnrolmentsXmlEnrolSelf as CourseEnrolmentsXmlEnrol);
        CourseEnrolmentsXmlEnrolments.SetParameters(CourseEnrolmentsXmlEnrols as CourseEnrolmentsXmlEnrols);
        
        CourseEnrolmentsXmlEnrolments.Serialize();
    }

    public void CreateCourseInforefXml()
    {
        //Write course/inforef.xml file
        CourseInforefXmlRole.SetParameters("5");
        CourseInforefXmlRoleref.SetParameters(CourseInforefXmlRole as CourseInforefXmlRole);
        CourseInforefXmlInforef.SetParameters(CourseInforefXmlInforef as CourseInforefXmlRoleref);
        
        CourseInforefXmlInforef.Serialize();
    }

    public void CreateCourseRolesXml()
    {
        //Write course/roles.xml file
        CourseRolesXmlRoles.SetParameters("", "");
        
        CourseRolesXmlRoles.Serialize();
    }

}