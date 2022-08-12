using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Inforef.xml;

[TestFixture]
public class ActivitiesInforefXmlInforefUt
{
    [Test]
    public void ActivitiesInforefXmlInforef_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var filelist = Substitute.For<List<ActivitiesInforefXmlFile>>();
        var fileref = Substitute.For<ActivitiesInforefXmlFileref>();
        fileref.File = filelist;

        var gradeitem = Substitute.For<ActivitiesInforefXmlGradeItem>();
        var gradeItemref = Substitute.For<ActivitiesInforefXmlGradeItemref>();
        gradeItemref.GradeItem = gradeitem;

        var systemUnderTest = new ActivitiesInforefXmlInforef();
        //Act
        systemUnderTest.Fileref = fileref;
        systemUnderTest.GradeItemref = gradeItemref;
        
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(systemUnderTest.Fileref, Is.EqualTo(fileref));
            Assert.That(systemUnderTest.GradeItemref, Is.EqualTo(gradeItemref));
        });
    }

    [Test]
    public void ActivitiesInforefXmlInforef_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000"));
        
        var list = new List<ActivitiesInforefXmlFile>
        {
            new ActivitiesInforefXmlFile(),
            new ActivitiesInforefXmlFile()
        };
        list[0].Id = XmlEntityManager.GetFileIdBlock1().ToString();
        list[1].Id = (XmlEntityManager.GetFileIdBlock2().ToString());
        var fileref = new ActivitiesInforefXmlFileref();
        fileref.File = list;

        var gradeitem = new ActivitiesInforefXmlGradeItem();
        var gradeItemref = new ActivitiesInforefXmlGradeItemref();
        gradeItemref.GradeItem = gradeitem;

        var systemUnderTest = new ActivitiesInforefXmlInforef();
        systemUnderTest.Fileref = fileref;
        systemUnderTest.GradeItemref = gradeItemref;
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize("h5pactivity", "1000");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000", "inforef.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}