using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class GradebookXmlUt
{
    [Test]
    public void GradebookXmlGradeItem_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new GradebookXmlGradeItem();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CategoryId, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.ItemName, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.ItemType, Is.EqualTo("course"));
            Assert.That(systemUnderTest.ItemModule, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.ItemInstance, Is.EqualTo("1"));
            Assert.That(systemUnderTest.ItemNumber, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.ItemInfo, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.IdNumber, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Calculation, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.GradeType, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Grademax, Is.EqualTo("100.00000"));
            Assert.That(systemUnderTest.Grademin, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.ScaleId, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.OutcomeId, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Gradepass, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Multfactor, Is.EqualTo("1.00000"));
            Assert.That(systemUnderTest.Plusfactor, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Aggregationcoef, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Aggregationcoef2, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Weightoverride, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Sortorder, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Display, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Decimals, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Hidden, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Locked, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Locktime, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Needsupdate, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.GradeGrades, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo("1"));
        });
    }

    [Test]
    public void GradebookXmlGradeItems_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var gradeItem = new GradebookXmlGradeItem();
        
        // Act
        var systemUnderTest = new GradebookXmlGradeItems();
        systemUnderTest.GradeItem = gradeItem;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GradeItem, Is.EqualTo(gradeItem));
        });
    }
    
    [Test]
    public void GradebookXmlGradeCategory_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new GradebookXmlGradeCategory();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Parent, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Depth, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Path, Is.EqualTo("/1/"));
            Assert.That(systemUnderTest.Fullname, Is.EqualTo("?"));
            Assert.That(systemUnderTest.Aggregation, Is.EqualTo("13"));
            Assert.That(systemUnderTest.Keephigh, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Droplow, Is.EqualTo("0"));
            Assert.That(systemUnderTest.AggregateOnlyGraded, Is.EqualTo("1"));
            Assert.That(systemUnderTest.AggregateOutcomes, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Hidden, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Id, Is.EqualTo("1"));
        });
    }
    
    [Test]
    public void GradebookXmlGradeCategories_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var gradeCategory = new GradebookXmlGradeCategory();
        
        // Act
        var systemUnderTest = new GradebookXmlGradeCategories();
        systemUnderTest.GradeCategory = gradeCategory;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GradeCategory, Is.EqualTo(gradeCategory));
        });
    }
    
    [Test]
    public void GradebookXmlGradeSetting_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new GradebookXmlGradeSetting();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo("minmaxtouse"));
            Assert.That(systemUnderTest.Value, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
    }
    
    [Test]
    public void GradebookXmlGradeSettings_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var gradeSetting = new GradebookXmlGradeSetting();
        
        // Act
        var systemUnderTest = new GradebookXmlGradeSettings();
        systemUnderTest.GradeSetting = gradeSetting;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GradeSetting, Is.EqualTo(gradeSetting));

        });
    }
    
    [Test]
    public void GradebookXmlGradebook_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var gradecategory = new GradebookXmlGradeCategory();
        var gradeitem = new GradebookXmlGradeItem();
        var gradesetting = new GradebookXmlGradeSetting();
        var gradesettings = new GradebookXmlGradeSettings();
        var gradecategorys = new GradebookXmlGradeCategories();
        var gradeitems = new GradebookXmlGradeItems();
        
        gradeitems.GradeItem = gradeitem;
        gradesettings.GradeSetting = gradesetting;
        gradecategorys.GradeCategory = gradecategory;

        // Act
        var systemUnderTest = new GradebookXmlGradebook();
        systemUnderTest.GradeItems = gradeitems;
        systemUnderTest.GradeSettings = gradesettings;
        systemUnderTest.GradeCategories = gradecategorys;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Attributes, Is.EqualTo(""));
            Assert.That(systemUnderTest.GradeCategories, Is.EqualTo(gradecategorys));
            Assert.That(systemUnderTest.GradeItems, Is.EqualTo(gradeitems));
            Assert.That(systemUnderTest.GradeLetters, Is.EqualTo(""));
            Assert.That(systemUnderTest.GradeSettings, Is.EqualTo(gradesettings));
        });
    }
    
    [Test]
    public void GradebookXmlGradebook_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupGenerator = new BackupFileGenerator(mockFileSystem);
        backupGenerator.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var gradecategory = new GradebookXmlGradeCategory();
        var gradeitem = new GradebookXmlGradeItem();
        var gradesetting = new GradebookXmlGradeSetting();
        var gradesettings = new GradebookXmlGradeSettings();
        var gradecategorys = new GradebookXmlGradeCategories();
        var gradeitems = new GradebookXmlGradeItems();
        
        gradeitems.GradeItem = gradeitem;
        gradesettings.GradeSetting = gradesetting;
        gradecategorys.GradeCategory = gradecategory;

        var systemUnderTest = new GradebookXmlGradebook();
        systemUnderTest.GradeItems = gradeitems;
        systemUnderTest.GradeSettings = gradesettings;
        systemUnderTest.GradeCategories = gradecategorys;
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "gradebook.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}