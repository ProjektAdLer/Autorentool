using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class OutcomesXmlUt
{
    [Test]
    public void OutcomesXmlOutcomesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var outcomesOutcomesDefinition = new OutcomesXmlOutcomesDefinition();
        
        //Act
        outcomesOutcomesDefinition.SetParameters();
        
        //Assert
        Assert.That(outcomesOutcomesDefinition, Is.EqualTo(outcomesOutcomesDefinition));
    }
    
    [Test]
    public void OutcomesXmlOutcomesDefinition_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var outcomesOutcomesDefinition = new OutcomesXmlOutcomesDefinition();
        outcomesOutcomesDefinition.SetParameters();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        outcomesOutcomesDefinition.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "outcomes.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}