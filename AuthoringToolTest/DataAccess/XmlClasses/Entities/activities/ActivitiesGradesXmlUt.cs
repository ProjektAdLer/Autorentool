using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.activities;

[TestFixture]
public class ActivitiesGradesXmlUt
{
    [Test]
    public void ActivitiesGradesXmlGradeItem_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var gradeitem = Substitute.For<ActivitiesGradesXmlGradeItem>();
        
        //Act
        gradeitem.SetParameters("h5pElementId", "h5pElementName",
            "mod", "h5pactivity", "1", "0", "$@NULL@$", "$@NULL@$",
            "$@NULL@$", "1", "100.00000", "0.00000", "$@NULL@$", "$@NULL@$",
            "0.00000", "1.00000", "0.00000", "0.00000", "1.00000",
            "0", "2", "0", "$@NULL@$", "0", "0", "0", "0",
            "currentTime", "currentTime", "", "h5pElementId");
        
        //Assert
        Assert.That(gradeitem.Categoryid, Is.EqualTo("h5pElementId"));
        Assert.That(gradeitem.Itemname, Is.EqualTo("h5pElementName"));
        Assert.That(gradeitem.Itemtype, Is.EqualTo("mod"));
        Assert.That(gradeitem.Itemmodule, Is.EqualTo("h5pactivity"));
        Assert.That(gradeitem.Iteminstance, Is.EqualTo("1"));
        Assert.That(gradeitem.Itemnumber, Is.EqualTo("0"));
        Assert.That(gradeitem.Iteminfo, Is.EqualTo("$@NULL@$"));
        Assert.That(gradeitem.Idnumber, Is.EqualTo("$@NULL@$"));
        Assert.That(gradeitem.Calculation, Is.EqualTo("$@NULL@$"));
        Assert.That(gradeitem.Gradetype, Is.EqualTo("1"));
        Assert.That(gradeitem.Grademax, Is.EqualTo("100.00000"));
        Assert.That(gradeitem.Grademin, Is.EqualTo("0.00000"));
        Assert.That(gradeitem.Scaleid, Is.EqualTo("$@NULL@$"));
        Assert.That(gradeitem.Outcomeid, Is.EqualTo("$@NULL@$"));
        Assert.That(gradeitem.Gradepass, Is.EqualTo("0.00000"));
        Assert.That(gradeitem.Multfactor, Is.EqualTo("1.00000"));
        Assert.That(gradeitem.Plusfactor, Is.EqualTo("0.00000"));
        Assert.That(gradeitem.Aggregationcoef, Is.EqualTo("0.00000"));
        Assert.That(gradeitem.Aggregationcoef2, Is.EqualTo("1.00000"));
        Assert.That(gradeitem.Weightoverride, Is.EqualTo("0"));
        Assert.That(gradeitem.Sortorder, Is.EqualTo("2"));
        Assert.That(gradeitem.Display, Is.EqualTo("0"));
        Assert.That(gradeitem.Decimals, Is.EqualTo("$@NULL@$"));
        Assert.That(gradeitem.Hidden, Is.EqualTo("0"));
        Assert.That(gradeitem.Locked, Is.EqualTo("0"));
        Assert.That(gradeitem.Locktime, Is.EqualTo("0"));
        Assert.That(gradeitem.Needsupdate, Is.EqualTo("0"));
        Assert.That(gradeitem.Timecreated, Is.EqualTo("currentTime"));
        Assert.That(gradeitem.Timemodified, Is.EqualTo("currentTime"));
        Assert.That(gradeitem.Grade_grades, Is.EqualTo(""));
        Assert.That(gradeitem.Id, Is.EqualTo("h5pElementId"));
    }

    [Test]
    public void ActivitiesGradesXmlGradeItems_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var gradeitem = Substitute.For<ActivitiesGradesXmlGradeItem>();
        gradeitem.SetParameters("h5pElementId", "h5pElementName",
            "mod", "h5pactivity", "1", "0", "$@NULL@$", "$@NULL@$",
            "$@NULL@$", "1", "100.00000", "0.00000", "$@NULL@$", "$@NULL@$",
            "0.00000", "1.00000", "0.00000", "0.00000", "1.00000",
            "0", "2", "0", "$@NULL@$", "0", "0", "0", "0",
            "currentTime", "currentTime", "", "h5pElementId");
        var gradeitems = Substitute.For<ActivitiesGradesXmlGradeItems>();
        
        //Act
        gradeitems.SetParameters(gradeitem);

        //Assert
        Assert.That(gradeitems.Grade_item, Is.EqualTo(gradeitem));

    }

    [Test]
    public void ActivitiesGradesXmlActivityGradebook_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var gradeitem = Substitute.For<ActivitiesGradesXmlGradeItem>();
        gradeitem.SetParameters("h5pElementId", "h5pElementName",
            "mod", "h5pactivity", "1", "0", "$@NULL@$", "$@NULL@$",
            "$@NULL@$", "1", "100.00000", "0.00000", "$@NULL@$", "$@NULL@$",
            "0.00000", "1.00000", "0.00000", "0.00000", "1.00000",
            "0", "2", "0", "$@NULL@$", "0", "0", "0", "0",
            "currentTime", "currentTime", "", "h5pElementId");
        var gradeitems = Substitute.For<ActivitiesGradesXmlGradeItems>();
        gradeitems.SetParameters(gradeitem);
        var gradeActivityGradebook = Substitute.For<ActivitiesGradesXmlActivityGradebook>();

        //Act
        gradeActivityGradebook.SetParameterts(gradeitems, "");

        //Assert
        Assert.That(gradeActivityGradebook.Grade_items, Is.EqualTo(gradeitems));
        Assert.That(gradeActivityGradebook.Grade_letters, Is.EqualTo(""));
    }

    [Test]
    public void ActivitiesGradesXmlActivityGradebook_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var readDsl = new ReadDSL();
        var h5pfactory = new XmlH5PFactory(readDsl, null, mockFileSystem, null, null, null, null,
            null, null, null, null, null, null, null, null, null,
            null, null, null, null);
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var gradeitem = new ActivitiesGradesXmlGradeItem();
        gradeitem.SetParameters("h5pElementId", "h5pElementName",
            "mod", "h5pactivity", "1", "0", "$@NULL@$", "$@NULL@$",
            "$@NULL@$", "1", "100.00000", "0.00000", "$@NULL@$", "$@NULL@$",
            "0.00000", "1.00000", "0.00000", "0.00000", "1.00000",
            "0", "2", "0", "$@NULL@$", "0", "0", "0", "0",
            "currentTime", "currentTime", "", "h5pElementId");
        var gradeitems = new ActivitiesGradesXmlGradeItems();
        gradeitems.SetParameters(gradeitem);
        var gradeActivityGradebook = new ActivitiesGradesXmlActivityGradebook();
        gradeActivityGradebook.SetParameterts(gradeitems, "");
        
        //Act 
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        h5pfactory.CreateActivityFolder("2");
        gradeActivityGradebook.Serialize("h5pactivity", "2");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2", "grades.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}