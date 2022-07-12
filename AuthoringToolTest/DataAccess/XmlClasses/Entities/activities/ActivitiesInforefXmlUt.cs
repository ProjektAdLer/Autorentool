using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.activities;

[TestFixture]
public class ActivitiesInforefXmlUt
{

    [Test]
    public void ActivitiesInforefXmlFile_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var list = new List<ActivitiesInforefXmlFile>
        {
            new(),
            new()
        };

        //Act
        list[0].SetParameters(XmlEntityManager.GetFileIdBlock1().ToString());
        list[1].SetParameters(XmlEntityManager.GetFileIdBlock2().ToString());

        //Assert
        Assert.That(list, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(list[0].Id, Is.EqualTo("1"));
            Assert.That(list[1].Id, Is.EqualTo("2"));
        });
    }

    [Test]
    public void ActivitiesInforefXmlFileref_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var list = new List<ActivitiesInforefXmlFile>
        {
            new(),
            new()
        };
        list[0]
            .SetParameters(XmlEntityManager.GetFileIdBlock1().ToString());
        list[1]
            .SetParameters(XmlEntityManager.GetFileIdBlock2().ToString());
        var fileref = new ActivitiesInforefXmlFileref();
            
        //Act
        fileref.SetParameters(list);

        //Assert
        Assert.That(fileref.File, Is.EqualTo(list));
    }

    [Test]
    public void ActivitiesInforefXmlGradeItem_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var gradeitem = new ActivitiesInforefXmlGradeItem();
        
        //Act
        gradeitem.SetParameters("1");
        
        //Assert
        Assert.That(gradeitem.Id, Is.EqualTo("1"));
    }

    [Test]
    public void ActivitiesInforefXmlGradeItemref_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var gradeitem = new ActivitiesInforefXmlGradeItem();
        gradeitem.SetParameters("1");
        var gradeItemref = new ActivitiesInforefXmlGradeItemref();

        //Act
        gradeItemref.SetParameters(gradeitem);
        
        //Assert
        Assert.That(gradeItemref.Grade_item, Is.EqualTo(gradeitem));

    }

    [Test]
    public void ActivitiesInforefXmlInforef_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var list = new List<ActivitiesInforefXmlFile>
        {
            new(),
            new()
        };
        list[0]
            .SetParameters(XmlEntityManager.GetFileIdBlock1().ToString());
        list[1]
            .SetParameters(XmlEntityManager.GetFileIdBlock2().ToString());
        
        var fileref = new ActivitiesInforefXmlFileref();
        fileref.SetParameters(list);

        var gradeitem = new ActivitiesInforefXmlGradeItem();
        gradeitem.SetParameters("1");
        var gradeItemref = new ActivitiesInforefXmlGradeItemref();
        gradeItemref.SetParameters(gradeitem);

        var inforef = new ActivitiesInforefXmlInforef();
        //Act
        inforef.SetParameters(fileref, gradeItemref);
        Assert.Multiple(() =>
        {

            //Assert
            Assert.That(inforef.Fileref, Is.EqualTo(fileref));
            Assert.That(inforef.Grade_itemref, Is.EqualTo(gradeItemref));
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
        list[0]
            .SetParameters(XmlEntityManager.GetFileIdBlock1().ToString());
        list[1]
            .SetParameters(XmlEntityManager.GetFileIdBlock2().ToString());
        var fileref = new ActivitiesInforefXmlFileref();
        fileref.SetParameters(list);

        var gradeitem = new ActivitiesInforefXmlGradeItem();
        gradeitem.SetParameters("1");
        var gradeItemref = new ActivitiesInforefXmlGradeItemref();
        gradeItemref.SetParameters(gradeitem);

        var inforef = new ActivitiesInforefXmlInforef();
        inforef.SetParameters(fileref, gradeItemref);
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        inforef.Serialize("h5pactivity", "1000");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000", "inforef.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}