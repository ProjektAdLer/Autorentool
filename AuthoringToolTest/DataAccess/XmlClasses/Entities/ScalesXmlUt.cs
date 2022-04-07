using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class ScalesXmlUt
{
    [Test]
    public void ScalesXmlScalesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var scalesScalesDefinition = new ScalesXmlScalesDefinition();
        
        //Act
        scalesScalesDefinition.SetParameters();
        
        //Assert
        Assert.That(scalesScalesDefinition, Is.EqualTo(scalesScalesDefinition));
    }
    
    [Test]
    public void ScalesXmlScalesDefinition_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        
        var scalesScalesDefinition = new ScalesXmlScalesDefinition();
        scalesScalesDefinition.SetParameters();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        scalesScalesDefinition.Serialize();
        
        //Assert
        Assert.That(mockFileSystem.FileExists("C:\\XMLFilesForExport\\scales.xml"), Is.True);
    }
}