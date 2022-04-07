using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class QuestionsXmlUt
{
    [Test]
    public void QuestionsXmlQuestionsCategories_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var questionsQuestionsCategories = new QuestionsXmlQuestionsCategories();
        
        //Act
        questionsQuestionsCategories.SetParameters();
        
        //Assert
        Assert.That(questionsQuestionsCategories, Is.EqualTo(questionsQuestionsCategories));
    }
    
    [Test]
    public void QuestionsXmlQuestionsCategories_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var questionsQuestionsCategories = new QuestionsXmlQuestionsCategories();
        questionsQuestionsCategories.SetParameters();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        questionsQuestionsCategories.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport\\questions.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}