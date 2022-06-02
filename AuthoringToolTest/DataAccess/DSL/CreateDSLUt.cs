using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class CreateDSLUt
{

    [Test]
    public void CreateDSL_WriteLearningWorld_ObjectsAreEqual()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        var Name = "asdf";
        var Shortname = "jkl;";
        var Authors = "ben and jerry";
        var Language = "german";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var content1 = new LearningContent("a", ".h5p", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h", 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz", 444, double.MaxValue);
        var ele3 = new LearningElement("a", "b", "e",content1, "pupup", "g","h", 17, 23);
        var LearningElements = new List<LearningElement> { ele1, ele2 };
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        space1.LearningElements.Add(ele3);
        var space2 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var LearningSpaces = new List<LearningSpace> { space1, space2 };

        var learningWorld = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            LearningElements, LearningSpaces);

        var createDsl = new CreateDSL();
        
        var ALLLearningElements = new List<LearningElement> { ele3, ele1, ele2 };
       
        //Act
        createDsl.WriteLearningWorld(learningWorld, mockFileSystem);
        
        //Assert
        Assert.That(createDsl.learningWorldJson.identifier.value, Is.EqualTo(learningWorld.Name));
        Assert.That(createDsl.learningWorldJson.identifier.type, Is.EqualTo("name"));
        Assert.That(createDsl.listLearningElements, Is.EqualTo(ALLLearningElements));
        Assert.That(createDsl.listLearningSpaces, Is.EqualTo(LearningSpaces));
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "DSL_Document.json");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}