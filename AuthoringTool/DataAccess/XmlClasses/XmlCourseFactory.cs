using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities.course;

namespace AuthoringTool.DataAccess.XmlClasses;

/// <summary>
/// Sets the paramters of course/course.xml, course/enrolments.xml, course/inforef.xml, course/roles.xml,
/// course/completiondefaults.xml and creates the files
/// </summary>
public class XmlCourseFactory
{
    private string currentTime;
    private LearningWorldJson? learningWorld;
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
    internal ICourseCompletiondefaultXmlCourseCompletionDefaults CourseCompletiondefaultXmlCourseCompletionDefaults { get; }
    internal IReadDSL? ReadDsl { get; }

   
    public XmlCourseFactory(IReadDSL readDsl, ICourseCourseXmlCategory? courseCourseXmlCategory=null, ICourseCourseXmlCourse? courseCourseXmlCourse=null,
        ICourseEnrolmentsXmlEnrol? courseEnrolmentsXmlEnrol=null, ICourseEnrolmentsXmlEnrols? courseEnrolmentsXmlEnrols=null, 
        ICourseEnrolmentsXmlEnrolments? courseEnrolmentsXmlEnrolments=null, ICourseInforefXmlRole? courseInforefXmlRole=null, 
        ICourseInforefXmlRoleref? courseInforefXmlRoleref=null, ICourseInforefXmlInforef? courseInforefXmlInforef=null, 
        ICourseRolesXmlRoles? courseRolesXmlRoles=null, ICourseCompletiondefaultXmlCourseCompletionDefaults? courseCourseXmlCompletiondefault=null)
    {
        CourseCourseXmlCategory = courseCourseXmlCategory?? new CourseCourseXmlCategory();
        CourseCourseXmlCourse = courseCourseXmlCourse?? new CourseCourseXmlCourse();

        CourseEnrolmentsXmlEnrolManual = courseEnrolmentsXmlEnrol?? new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrolGuest = courseEnrolmentsXmlEnrol?? new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrolSelf = courseEnrolmentsXmlEnrol?? new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrols = courseEnrolmentsXmlEnrols?? new CourseEnrolmentsXmlEnrols();
        CourseEnrolmentsXmlEnrolments = courseEnrolmentsXmlEnrolments?? new CourseEnrolmentsXmlEnrolments();

        CourseInforefXmlRole = courseInforefXmlRole?? new CourseInforefXmlRole();
        CourseInforefXmlRoleref = courseInforefXmlRoleref?? new CourseInforefXmlRoleref();
        CourseInforefXmlInforef = courseInforefXmlInforef?? new CourseInforefXmlInforef();

        CourseRolesXmlRoles = courseRolesXmlRoles?? new CourseRolesXmlRoles();
        
        CourseCompletiondefaultXmlCourseCompletionDefaults = courseCourseXmlCompletiondefault?? new CourseCompletiondefaultXmlCourseCompletionDefaults();
        
        ReadDsl = readDsl;
        learningWorld = readDsl.GetLearningWorld();
        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }
    
    /// <summary>
    /// Use all the methods of the current class
    /// </summary>
    public void CreateXmlCourseFactory()
    {
        //Sets the parameter and creates Course/course.xml
        CreateCourseCourseXml();
        
        //Sets the parameter and creates Course/enrolments.xml
        CreateCourseEnrolmentsXml();
        
        //Sets the parameter and creates Course/inforef.xml
        CreateCourseInforefXml();
        
        //Sets the parameter and creates Course/roles.xml
        CreateCourseRolesXml();
        
        //Sets the parameter and creates Course/completiondefault.xml
        CreateCourseCompletiondefault();
    }
    
    
    public void CreateCourseCourseXml()
    {
        //set parameters of the course/course.xml file
        CourseCourseXmlCategory.SetParameters("Miscellaneous", "$@NULL@$", "1");
        if (learningWorld != null)
            if (learningWorld.identifier != null)
                CourseCourseXmlCourse.SetParameters(learningWorld.identifier.value, learningWorld.identifier.value,
                    "", "", "1", "topics", "1", "5", "1645484400",
                    "2221567452", "0", "0", "0", "0", "1",
                    "0", "0", "0", "", "", currentTime, currentTime,
                    "0", "1", "1", "1", "0",
                    "0", "0", CourseCourseXmlCategory as CourseCourseXmlCategory, "", ""
                    , "1", "1");

        //create course/course.xml file
        CourseCourseXmlCourse.Serialize();
    }

    public void CreateCourseEnrolmentsXml()
    {
        //set parameters of the course/enrolments.xml file
        //the enrolments.xml file is identical in every moodle backup file
        CourseEnrolmentsXmlEnrolManual.SetParametersShort("5", "1", "manual", "0");
        CourseEnrolmentsXmlEnrolGuest.SetParametersShort("0", "2", "guest", "1");
        CourseEnrolmentsXmlEnrolSelf.SetParametersFull("5", "3", "self", "1", "0", "0", "0", "1", "0", "1");
        CourseEnrolmentsXmlEnrols.SetParameters(CourseEnrolmentsXmlEnrolManual as CourseEnrolmentsXmlEnrol, 
            CourseEnrolmentsXmlEnrolGuest as CourseEnrolmentsXmlEnrol, CourseEnrolmentsXmlEnrolSelf as CourseEnrolmentsXmlEnrol);
        CourseEnrolmentsXmlEnrolments.SetParameters(CourseEnrolmentsXmlEnrols as CourseEnrolmentsXmlEnrols);
        
        //create course/enrolments.xml file
        CourseEnrolmentsXmlEnrolments.Serialize();
    }

    public void CreateCourseInforefXml()
    {
        //set parameters of the course/inforef.xml file
        CourseInforefXmlRole.SetParameters("5");
        CourseInforefXmlRoleref.SetParameters(CourseInforefXmlRole as CourseInforefXmlRole);
        CourseInforefXmlInforef.SetParameters(CourseInforefXmlRoleref as CourseInforefXmlRoleref);
        
        //create course/inforef.xml file
        CourseInforefXmlInforef.Serialize();
    }

    public void CreateCourseRolesXml()
    {
        //set parameters of the course/roles.xml file
        CourseRolesXmlRoles.SetParameters("", "");
        
        //create course/roles.xml file
        CourseRolesXmlRoles.Serialize();
    }

    public void CreateCourseCompletiondefault()
    {
        //create course/completiondefaults.xml
        //The file is empty, therefore no parameters need to be set yet.
        CourseCompletiondefaultXmlCourseCompletionDefaults.Serialize();
    }

}