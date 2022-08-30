using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Gradebook.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Gradebook.xml;

[TestFixture]
public class GradebookXmlUt
{
    
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