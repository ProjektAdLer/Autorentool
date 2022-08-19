using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Scales.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Scales.xml;

[TestFixture]
public class ScalesXmlUt
{
    
    [Test]
    public void ScalesXmlScalesDefinition_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var systemUnderTest = new ScalesXmlScalesDefinition();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "scales.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}