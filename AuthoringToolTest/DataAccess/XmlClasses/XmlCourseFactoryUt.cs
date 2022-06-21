using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities.course;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class XmlCourseFactoryUt
{
    [Test]
    public void XmlCourseFactory_Constructor_AllPropertiesSet()
    {
        //Arrange
        
        //Act
        var xmlCourseFactory = CreateStandardXmlCourseFactory();

        //Assert
        Assert.That(xmlCourseFactory.CourseCourseXmlCategory, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseCourseXmlCourse, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseEnrolmentsXmlEnrolManual, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseEnrolmentsXmlEnrolGuest, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseEnrolmentsXmlEnrolSelf, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseEnrolmentsXmlEnrols, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseEnrolmentsXmlEnrolments, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseInforefXmlRole, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseInforefXmlRoleref, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseInforefXmlInforef, Is.Not.Null);
        Assert.That(xmlCourseFactory.CourseRolesXmlRoles, Is.Not.Null);
    }

    [Test]
    public void CreateCourseCourseXml_Default_ParametersSetAndSerialized()
    {
        //Arrange 
        var mockCourseCategory = Substitute.For<ICourseCourseXmlCategory>();
        var mockCourseCourse = Substitute.For<ICourseCourseXmlCourse>();
        var mockreadDsl = Substitute.For<IReadDSL>();
        
        var mockIdentifier = Substitute.For<IdentifierJson>();
        var mockLearningWorld = new LearningWorldJson();
        mockLearningWorld.identifier = mockIdentifier;

        mockreadDsl.GetLearningWorld().Returns(mockLearningWorld);

        var xmlCourseFactory = new XmlCourseFactory(mockreadDsl, mockCourseCategory, mockCourseCourse, null,
            null, null, null, null,
            null, null, null);

        //Act
        xmlCourseFactory.CreateCourseCourseXml();
        
        //Assert
        mockCourseCategory.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockCourseCourse.Received().SetParameters(Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CourseCourseXmlCategory?>(), Arg.Any<string?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockCourseCourse.Received().Serialize();

    }
    
    [Test]
    public void CreateCourseEnrolmentsXml_Default_ParametersSetAndSerialized()
    {
        //Arrange 
        var mockEnrol = Substitute.For<ICourseEnrolmentsXmlEnrol>();
        var mockEnrols = Substitute.For<ICourseEnrolmentsXmlEnrols>();
        var mockEnrolments = Substitute.For<ICourseEnrolmentsXmlEnrolments>();
        
        var xmlCourseFactory = CreateTestableXmlCourseFactory(null, null, mockEnrol,
            mockEnrols, mockEnrolments);

        //Act
        xmlCourseFactory.CreateCourseEnrolmentsXml();
        
        //Assert
        mockEnrol.Received().SetParametersShort(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockEnrol.Received().SetParametersFull(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>());
        mockEnrols.Received().SetParameters(Arg.Any<CourseEnrolmentsXmlEnrol>(), Arg.Any<CourseEnrolmentsXmlEnrol>(),
            Arg.Any<CourseEnrolmentsXmlEnrol>());
        mockEnrolments.Received().SetParameters(Arg.Any<CourseEnrolmentsXmlEnrols>());
        mockEnrolments.Received().Serialize();
    }
    
    [Test]
    public void CreateCourseInforefXml_Default_ParametersSetAndSerialized()
    {
        //Arrange 
        var mockInforefRole = Substitute.For<ICourseInforefXmlRole>();
        var mockInforefRoleref = Substitute.For<ICourseInforefXmlRoleref>();
        var mockInforefInforef = Substitute.For<ICourseInforefXmlInforef>();
        
        var xmlCourseFactory = CreateTestableXmlCourseFactory(null, null, null,
            null, null, mockInforefRole, mockInforefRoleref, mockInforefInforef);

        //Act
        xmlCourseFactory.CreateCourseInforefXml();
        
        //Assert
        mockInforefRole.Received().SetParameters(Arg.Any<string>());
        mockInforefRoleref.Received().SetParameters(Arg.Any<CourseInforefXmlRole>());
        mockInforefInforef.Received().SetParameters(Arg.Any<CourseInforefXmlRoleref>());
        mockInforefInforef.Received().Serialize();
    }
    
    [Test]
    public void CreateCourseRolesXml_Default_ParametersSetAndSerialized()
    {
        //Arrange 
        var mockCourseRoles = Substitute.For<ICourseRolesXmlRoles>();

        var xmlCourseFactory = CreateTestableXmlCourseFactory(null, null, null,
            null, null, null, null, null, mockCourseRoles);

        //Act
        xmlCourseFactory.CreateCourseRolesXml();
        
        //Assert
        mockCourseRoles.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>());
        mockCourseRoles.Received().Serialize();
    }
    
    
    public XmlCourseFactory CreateStandardXmlCourseFactory()
    {
        ReadDSL? readDsl = new ReadDSL();
        return new XmlCourseFactory(readDsl);
    }

    public XmlCourseFactory CreateTestableXmlCourseFactory(ICourseCourseXmlCategory? courseXmlCategory = null, ICourseCourseXmlCourse? courseXmlCourse = null,
        ICourseEnrolmentsXmlEnrol? enrolmentsXmlEnrol = null, ICourseEnrolmentsXmlEnrols? enrolmentsXmlEnrols = null, 
        ICourseEnrolmentsXmlEnrolments? enrolmentsXmlEnrolments = null, ICourseInforefXmlRole? inforefXmlRole = null, 
        ICourseInforefXmlRoleref? inforefXmlRoleref = null, ICourseInforefXmlInforef? inforefXmlInforef = null, ICourseRolesXmlRoles? rolesXmlRoles = null,
        ICourseCompletiondefaultXmlCourseCompletionDefaults? courseCourseXmlCompletiondefault = null, IReadDSL? readDsl = null, 
        LearningWorldJson? learningWorldJson = null)
    {
        courseXmlCategory ??= Substitute.For<ICourseCourseXmlCategory>();
        courseXmlCourse ??= Substitute.For<ICourseCourseXmlCourse>();

        enrolmentsXmlEnrol ??= Substitute.For<ICourseEnrolmentsXmlEnrol>();
        enrolmentsXmlEnrols ??= Substitute.For<ICourseEnrolmentsXmlEnrols>();
        enrolmentsXmlEnrolments ??= Substitute.For<ICourseEnrolmentsXmlEnrolments>();

        inforefXmlRole ??= Substitute.For<ICourseInforefXmlRole>();
        inforefXmlRoleref ??= Substitute.For<ICourseInforefXmlRoleref>();
        inforefXmlInforef ??= Substitute.For<ICourseInforefXmlInforef>();

        rolesXmlRoles ??= Substitute.For<ICourseRolesXmlRoles>();

        courseCourseXmlCompletiondefault ??= Substitute.For<ICourseCompletiondefaultXmlCourseCompletionDefaults>();
        
        readDsl ??= Substitute.For<IReadDSL?>();
        learningWorldJson ??= Substitute.For<LearningWorldJson>();

        return new XmlCourseFactory(readDsl, courseXmlCategory, courseXmlCourse, enrolmentsXmlEnrol, enrolmentsXmlEnrols,
            enrolmentsXmlEnrolments, inforefXmlRole, inforefXmlRoleref, inforefXmlInforef, rolesXmlRoles, 
            courseCourseXmlCompletiondefault);
    }
}