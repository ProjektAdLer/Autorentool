using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Questions.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Questions.xml;

[TestFixture]
public class QuestionsXmlQuestionsCategoriesUt
{
    
    [Test]
    public void QuestionsXmlQuestionsCategories_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var systemUnderTest = new QuestionsXmlQuestionsCategories();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "questions.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}