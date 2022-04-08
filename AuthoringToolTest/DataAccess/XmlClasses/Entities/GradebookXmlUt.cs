using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class GradebookXmlUt
{

    [Test]
    public void GradebookXmlGradebook_SetParameters_ObjectsAreEqual()
    {
        //Arrange 
        var gradeSetting = new GradebookXmlGradeSetting();
        gradeSetting.SetParameters("minmaxtouse", "1");
        
        var gradeSettings = new GradebookXmlGradeSettings();
        gradeSettings.SetParameters(gradeSetting);
        
        var gradebook = new GradebookXmlGradebook();
        
        //Act (What to test)
        gradebook.SetParameters(gradeSettings);
        
        //Assert
        Assert.That(gradebook.Grade_settings, Is.EqualTo(gradeSettings));
    }

    [Test]
    public void GradebookXmlSettings_SetParameters_ObjectsAreEqual()
    {
        //Arrange 
        var gradeSetting = new GradebookXmlGradeSetting();
        gradeSetting.SetParameters("minmaxtouse", "1");
        
        var gradeSettings = new GradebookXmlGradeSettings();
        
        //Act (What to test)
        gradeSettings.SetParameters(gradeSetting);
        
        //Assert
        Assert.That(gradeSettings.grade_Setting, Is.EqualTo(gradeSetting));
    }

    [Test]
    public void GradebookXmlSetting_SetParameters_ObjectsAreEqual()
    {
        //Arrange 
        var gradeSetting = new GradebookXmlGradeSetting();

        //Act (What to test)
        gradeSetting.SetParameters("minmaxtouse", "1");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(gradeSetting.Name, Is.EqualTo("minmaxtouse"));
            Assert.That(gradeSetting.Value, Is.EqualTo("1"));
        });
    }
    
    [Test]
    public void GradebookXmlSetting_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var gradebookSetting = new GradebookXmlGradeSetting();
        gradebookSetting.SetParameters("minmaxtouse", "1");
        var gradebookSettings = new GradebookXmlGradeSettings();
        gradebookSettings.SetParameters(gradebookSetting);
        var gradebook = new GradebookXmlGradebook();
        gradebook.SetParameters(gradebookSettings);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        gradebook.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "gradebook.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}