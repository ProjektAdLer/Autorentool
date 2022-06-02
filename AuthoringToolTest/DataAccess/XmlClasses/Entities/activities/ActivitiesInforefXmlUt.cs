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
        List<ActivitiesInforefXmlFile> list = new List<ActivitiesInforefXmlFile>();
        
        //Act
        if (list != null)
        {
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock1().ToString());
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock2().ToString());
        }
        
        //Assert
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list[0].Id, Is.EqualTo("1"));
        Assert.That(list[1].Id, Is.EqualTo("2"));
    }

    [Test]
    public void ActivitiesInforefXmlFileref_SetParameters_ObjectAreEqual()
    {
        //Arrange
        List<ActivitiesInforefXmlFile> list = new List<ActivitiesInforefXmlFile>();
        var fileref = new ActivitiesInforefXmlFileref();
        if (list != null)
        {
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock1().ToString());
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock2().ToString());
            
            //Act
            fileref.SetParameters(list);
        }
        
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
        List<ActivitiesInforefXmlFile> list = new List<ActivitiesInforefXmlFile>();
        var fileref = new ActivitiesInforefXmlFileref();
        if (list != null)
        {
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock1().ToString());
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock2().ToString());
            fileref.SetParameters(list);
        }
        
        var gradeitem = new ActivitiesInforefXmlGradeItem();
        gradeitem.SetParameters("1");
        var gradeItemref = new ActivitiesInforefXmlGradeItemref();
        gradeItemref.SetParameters(gradeitem);

        var inforef = new ActivitiesInforefXmlInforef();
        //Act
        inforef.SetParameters(fileref, gradeItemref);

        //Assert
        Assert.That(inforef.Fileref, Is.EqualTo(fileref));
        Assert.That(inforef.Grade_itemref, Is.EqualTo(gradeItemref));
    }
    
    [Test]
    public void ActivitiesInforefXmlInforef_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var readDsl = new ReadDSL();
        var h5pfactory = new XmlH5PFactory(readDsl, mockFileSystem, null, null, null, null,
            null, null, null, null, null, null, null, null, null,
            null, null, null, null);
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        List<ActivitiesInforefXmlFile> list = new List<ActivitiesInforefXmlFile>();
        var fileref = new ActivitiesInforefXmlFileref();
        if (list != null)
        {
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock1().ToString());
            list.Add(new ActivitiesInforefXmlFile());
            list[list.Count - 1]
                .SetParameterts(XmlEntityManager.GetFileIdBlock2().ToString());
            fileref.SetParameters(list);
        }
        
        var gradeitem = new ActivitiesInforefXmlGradeItem();
        gradeitem.SetParameters("1");
        var gradeItemref = new ActivitiesInforefXmlGradeItemref();
        gradeItemref.SetParameters(gradeitem);

        var inforef = new ActivitiesInforefXmlInforef();
        inforef.SetParameters(fileref, gradeItemref);
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        h5pfactory.CreateActivityFolder("1000");
        inforef.Serialize("1000");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000", "inforef.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}