using Generator.DSL;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities._course.Completiondefault.xml;
using Generator.XmlClasses.Entities._course.Course.xml;
using Generator.XmlClasses.Entities._course.Enrolments.xml;
using Generator.XmlClasses.Entities._course.Inforef.xml;
using Generator.XmlClasses.Entities._course.Roles.xml;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses;

[TestFixture]
public class XmlCourseFactoryUt
{
    [Test]
    public void XmlCourseFactory_Constructor_AllPropertiesSet()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();

        //Act
        var systemUnderTest = new XmlCourseFactory(mockReadDsl);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CourseCourseXmlCategory, Is.Not.Null);
            Assert.That(systemUnderTest.CourseCourseXmlCourse, Is.Not.Null);
            Assert.That(systemUnderTest.CourseEnrolmentsXmlEnrolManual, Is.Not.Null);
            Assert.That(systemUnderTest.CourseEnrolmentsXmlEnrolGuest, Is.Not.Null);
            Assert.That(systemUnderTest.CourseEnrolmentsXmlEnrolSelf, Is.Not.Null);
            Assert.That(systemUnderTest.CourseEnrolmentsXmlEnrols, Is.Not.Null);
            Assert.That(systemUnderTest.CourseEnrolmentsXmlEnrolments, Is.Not.Null);
            Assert.That(systemUnderTest.CourseInforefXmlRole, Is.Not.Null);
            Assert.That(systemUnderTest.CourseInforefXmlRoleref, Is.Not.Null);
            Assert.That(systemUnderTest.CourseInforefXmlInforef, Is.Not.Null);
            Assert.That(systemUnderTest.CourseRolesXmlRoles, Is.Not.Null);
        });
    }

    [Test]
    public void XmlCourseFactory_CreateXmlCourseFactory_AllMethodsCalled()
    {   
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        
        var mockCourseCategory = new CourseCourseXmlCategory();
        var mockCourseCourse = Substitute.For<ICourseCourseXmlCourse>();
        var mockPluginLocalAdlerCourse = Substitute.For<CourseCourseXmlPluginLocalAdlerCourse>();
        mockCourseCourse.PluginLocalAdlerCourse = mockPluginLocalAdlerCourse;
        var mockLearningWorld = new LearningWorldJson( "world", "12345",
            new List<TopicJson>(), new List<LearningSpaceJson>(), new List<LearningElementJson>());

        mockReadDsl.GetLearningWorld().Returns(mockLearningWorld);
        
        var mockEnrols = new CourseEnrolmentsXmlEnrols();
        var mockEnrolManual = Substitute.For<ICourseEnrolmentsXmlEnrol>();
        var mockEnrolGuest = Substitute.For<ICourseEnrolmentsXmlEnrol>();
        var mockEnrolSelf = Substitute.For<ICourseEnrolmentsXmlEnrol>();
        var mockEnrolments = Substitute.For<ICourseEnrolmentsXmlEnrolments>();
        
        var mockInforefRole = new CourseInforefXmlRole();
        var mockInforefRoleref = new CourseInforefXmlRoleref();
        var mockInforefInforef = Substitute.For<ICourseInforefXmlInforef>();
        
        var mockCourseRoles = Substitute.For<ICourseRolesXmlRoles>();
        
        var mockCourseCompletiondefault= Substitute.For<ICourseCompletiondefaultXmlCourseCompletionDefaults>();

        
        //Act
        var systemUnderTest = new XmlCourseFactory(mockReadDsl, mockCourseCategory, mockCourseCourse,
            mockEnrolManual, mockEnrolGuest, mockEnrolSelf, mockEnrols, mockEnrolments,
            mockInforefRole, mockInforefRoleref, mockInforefInforef, mockCourseRoles, mockCourseCompletiondefault);
        
        systemUnderTest.CreateXmlCourseFactory();
        
        //Assert
        Assert.Multiple(() =>
        {
            mockCourseCourse.Received().Serialize();
            mockEnrolments.Received().Serialize();
            mockInforefInforef.Received().Serialize();
            mockCourseRoles.Received().Serialize();
            mockCourseCompletiondefault.Received().Serialize();
        });
    }

    [Test]
    public void CreateCourseCourseXml_CourseCategoryCourseCourse_AndSerializes()
    {
        //Arrange 

        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockCourseCategory = new CourseCourseXmlCategory();
        var mockCourseCourse = Substitute.For<ICourseCourseXmlCourse>();
        var mockPluginLocalAdlerCourse = Substitute.For<CourseCourseXmlPluginLocalAdlerCourse>();
        mockCourseCourse.PluginLocalAdlerCourse = mockPluginLocalAdlerCourse;
        var mockLearningWorld = new LearningWorldJson( "world", "12345",
            new List<TopicJson>(), new List<LearningSpaceJson>(), new List<LearningElementJson>());

        mockReadDsl.GetLearningWorld().Returns(mockLearningWorld);

        var systemUnderTest = new XmlCourseFactory(mockReadDsl, mockCourseCategory, mockCourseCourse);

        //Act
        systemUnderTest.CreateCourseCourseXml();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(mockCourseCategory.Description, Is.EqualTo("$@NULL@$"));
            Assert.That(mockCourseCategory.Id, Is.EqualTo("1"));
            Assert.That(mockCourseCategory.Name, Is.EqualTo("Miscellaneous"));
            Assert.That(mockCourseCourse.Format, Is.EqualTo("tiles"));
            Assert.That(mockCourseCourse.BaseColour, Is.EqualTo("#009681"));
            Assert.That(mockCourseCourse.CourseUseSubtiles, Is.EqualTo("0"));
            Assert.That(mockCourseCourse.CourseShowTileProgress, Is.EqualTo("2"));
            Assert.That(mockCourseCourse.ShowGrades, Is.EqualTo("1"));
            Assert.That(mockCourseCourse.Visible, Is.EqualTo("1"));
            Assert.That(mockCourseCourse.Theme, Is.EqualTo("boost"));
            Assert.That(mockCourseCourse.ShowCompletionConditions, Is.EqualTo("1"));
            Assert.That(mockCourseCourse.EnableCompletion, Is.EqualTo("1"));
            Assert.That(mockCourseCourse.Category, Is.EqualTo(mockCourseCategory));
            Assert.That(mockCourseCourse.PluginLocalAdlerCourse, Is.EqualTo(mockPluginLocalAdlerCourse));
            Assert.That(mockCourseCourse.PluginLocalAdlerCourse.AdlerCourse.Uuid, Is.EqualTo("12345"));
            mockCourseCourse.Received().Serialize();
        });
    }
    
    [Test]
    public void CreateCourseEnrolmentsXml_SetsEnrolManualGuestSelfEnrolsEnrolments_AndSerializes()
    {
        //Arrange 
        var mockReadDsl = Substitute.For<IReadDsl>();
        
        var mockEnrolManual = new CourseEnrolmentsXmlEnrol();
        var mockEnrolGuest = new CourseEnrolmentsXmlEnrol();
        var mockEnrolSelf = new CourseEnrolmentsXmlEnrol();
        var mockEnrols = new CourseEnrolmentsXmlEnrols();
        var mockEnrolments = Substitute.For<ICourseEnrolmentsXmlEnrolments>();
        
        var enrolsList = new List<CourseEnrolmentsXmlEnrol>();
        enrolsList.Add(mockEnrolManual);
        enrolsList.Add(mockEnrolGuest);
        enrolsList.Add(mockEnrolSelf);
        
        var systemUnderTest = new XmlCourseFactory(mockReadDsl, courseEnrolmentsXmlEnrolManual: mockEnrolManual,
            courseEnrolmentsXmlEnrolGuest: mockEnrolGuest, courseEnrolmentsXmlEnrolSelf: mockEnrolSelf,
            courseEnrolmentsXmlEnrols: mockEnrols, courseEnrolmentsXmlEnrolments: mockEnrolments); 
        
        //Act
        systemUnderTest.CreateCourseEnrolmentsXml();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(mockEnrolSelf.Id, Is.EqualTo("3"));
            Assert.That(mockEnrolSelf.RoleId, Is.EqualTo("5"));
            Assert.That(mockEnrolSelf.EnrolMethod, Is.EqualTo("self"));
            Assert.That(mockEnrolSelf.Status, Is.EqualTo("0"));
            Assert.That(mockEnrolSelf.CustomInt1, Is.EqualTo("0"));
            Assert.That(mockEnrolSelf.CustomInt2, Is.EqualTo("0"));
            Assert.That(mockEnrolSelf.CustomInt3, Is.EqualTo("0"));
            Assert.That(mockEnrolSelf.CustomInt4, Is.EqualTo("1"));
            Assert.That(mockEnrolSelf.CustomInt5, Is.EqualTo("0"));
            Assert.That(mockEnrolSelf.CustomInt6, Is.EqualTo("1"));
            
            Assert.That(mockEnrols.Enrol, Is.EqualTo(enrolsList));
            
            Assert.That(mockEnrolments.Enrols, Is.EqualTo(mockEnrols));
            mockEnrolments.Received().Serialize();
        });
    }
    
    [Test]
    public void CreateCourseInforefXml_SetsInforefRoleRolerefInforef_AndSerializes()
    {
        //Arrange 
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockInforefRole = new CourseInforefXmlRole();
        var mockInforefRoleref = new CourseInforefXmlRoleref();
        var mockInforefInforef = Substitute.For<ICourseInforefXmlInforef>();

        var systemUnderTest = new XmlCourseFactory(mockReadDsl, courseInforefXmlInforef: mockInforefInforef,
            courseInforefXmlRoleref: mockInforefRoleref, courseInforefXmlRole: mockInforefRole);
        
        //Act
        systemUnderTest.CreateCourseInforefXml();
        
        //Assert
        Assert.Multiple(()=>
        {
            Assert.That(mockInforefRoleref.Role, Is.EqualTo(mockInforefRole));
            Assert.That(mockInforefInforef.Roleref, Is.EqualTo(mockInforefRoleref));
            mockInforefInforef.Received().Serialize();
        });
    }
    
    [Test]
    public void CreateCourseRolesXml_SetsCourseRoles_AndSerializes()
    {
        //Arrange 
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockCourseRoles = Substitute.For<ICourseRolesXmlRoles>();

        
        var systemUnderTest = new XmlCourseFactory(mockReadDsl, courseRolesXmlRoles:mockCourseRoles);

        //Act
        systemUnderTest.CreateCourseRolesXml();
        
        //Assert
        Assert.Multiple(()=>
        {
            mockCourseRoles.Received().Serialize();
        });
    }
    
    [Test]
    public void CreateCourseCompletiondefault_SetsCourseCompletiondefault_AndSerializes()
    {
        //Arrange 
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockCourseCompletiondefault= Substitute.For<ICourseCompletiondefaultXmlCourseCompletionDefaults>();
        
        var systemUnderTest = new XmlCourseFactory(mockReadDsl, courseCourseXmlCompletiondefault:mockCourseCompletiondefault);

        //Act
        systemUnderTest.CreateCourseCompletiondefault();
        
        //Assert
        Assert.Multiple(()=>
        {
            mockCourseCompletiondefault.Received().Serialize();
        });
    }


}