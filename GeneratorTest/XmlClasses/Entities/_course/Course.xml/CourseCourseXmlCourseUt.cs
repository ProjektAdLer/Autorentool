using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._course.Course.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Course.xml;

[TestFixture]
public class CourseCourseXmlCourseUt
{

    [Test]
    public void CourseCourseXmlCourse_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var courseCategory = new CourseCourseXmlCategory();

        //Act
        var systemUnderTest = new CourseCourseXmlCourse();
        systemUnderTest.Category = courseCategory;
        
        //Assert
        Assert.Multiple(() =>
            {
                Assert.That(systemUnderTest.Shortname, Is.EqualTo(""));
                Assert.That(systemUnderTest.Fullname, Is.EqualTo(""));
                Assert.That(systemUnderTest.IdNumber, Is.EqualTo(""));
                Assert.That(systemUnderTest.Summary, Is.EqualTo(""));
                Assert.That(systemUnderTest.SummaryFormat, Is.EqualTo("1"));
                Assert.That(systemUnderTest.Format, Is.EqualTo("tiles"));
                Assert.That(systemUnderTest.DefaultTileIcon, Is.EqualTo("pie-chart"));
                Assert.That(systemUnderTest.BaseColour, Is.EqualTo("#009681"));
                Assert.That(systemUnderTest.CourseUseSubtiles, Is.EqualTo("1"));
                Assert.That(systemUnderTest.UseSubtilesSecZero, Is.EqualTo("0"));
                Assert.That(systemUnderTest.CourseShowTileProgress, Is.EqualTo("2"));
                Assert.That(systemUnderTest.DisplayFilterbar, Is.EqualTo("0"));
                Assert.That(systemUnderTest.CourseUseBarForHeadings, Is.EqualTo("0"));
                Assert.That(systemUnderTest.ShowGrades, Is.EqualTo("1"));
                Assert.That(systemUnderTest.NewsItems, Is.EqualTo("5"));
                Assert.That(systemUnderTest.Startdate, Is.EqualTo("1645484400"));
                Assert.That(systemUnderTest.Enddate, Is.EqualTo("2221567452"));
                Assert.That(systemUnderTest.Marker, Is.EqualTo("0"));
                Assert.That(systemUnderTest.Maxbytes, Is.EqualTo("0"));
                Assert.That(systemUnderTest.Legacyfiles, Is.EqualTo("0"));
                Assert.That(systemUnderTest.ShowReports, Is.EqualTo("0"));
                Assert.That(systemUnderTest.Visible, Is.EqualTo("1"));
                Assert.That(systemUnderTest.GroupMode, Is.EqualTo("0"));
                Assert.That(systemUnderTest.GroupModeForce, Is.EqualTo("0"));
                Assert.That(systemUnderTest.DefaultGroupingId, Is.EqualTo("0"));
                Assert.That(systemUnderTest.Lang, Is.EqualTo(""));
                Assert.That(systemUnderTest.Theme, Is.EqualTo("boost"));
                Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
                Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
                Assert.That(systemUnderTest.Requested, Is.EqualTo("0"));
                Assert.That(systemUnderTest.ShowActivityDates, Is.EqualTo("1"));
                Assert.That(systemUnderTest.ShowCompletionConditions, Is.EqualTo("1"));
                Assert.That(systemUnderTest.EnableCompletion, Is.EqualTo("1"));
                Assert.That(systemUnderTest.CompletionNotify, Is.EqualTo("0"));
                Assert.That(systemUnderTest.HiddenSections, Is.EqualTo("0"));
                Assert.That(systemUnderTest.CourseDisplay, Is.EqualTo("0"));
                Assert.That(systemUnderTest.Category, Is.EqualTo(courseCategory));
                Assert.That(systemUnderTest.Tags, Is.EqualTo(""));
                Assert.That(systemUnderTest.CustomFields, Is.EqualTo(""));
                Assert.That(systemUnderTest.Id, Is.EqualTo("1"));
                Assert.That(systemUnderTest.ContextId, Is.EqualTo("1"));
            });
    }
    
    [Test]

    public void CourseCourseXmlCourse_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Combine(curWorkDir, "XMLFilesForExport","course"));

        var courseCategory = new CourseCourseXmlCategory();
        var systemUnderTest = new CourseCourseXmlCourse();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "course.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}