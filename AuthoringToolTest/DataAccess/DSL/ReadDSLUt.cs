using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class ReadDslUt
{
    //TODO: not a proper unit test, needs to be rewritten with fixed input - n.stich
    [Test]
    public void ReadDSL_ReadLearningWorld_DSLDocumentRead()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        var content1 = new LearningContent("a", "h5p", new byte[] {0x01, 0x02});
        var content2 = new LearningContent("w", ".h5p", new byte[] {0x02, 0x01});
        var ele1 = new LearningElement("a", "b", "e", content1, "pupup", "g", "h", LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2, "baba", "z", "zz", LearningElementDifficultyEnum.Easy, 444, double.MaxValue);
        var ele3 = new LearningElement("a", "b", "e", content1, "pupup", "g", "h", LearningElementDifficultyEnum.Easy, 17, 23);
        var learningElements = new List<LearningElement> {ele1, ele2};
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        space1.LearningElements.Add(ele3);
        var space2 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<LearningSpace> {space1, space2};

        var learningWorld = new LearningWorld(name, shortname, authors, language, description, goals,
            learningElements, learningSpaces);
        
        //TODO: this needs to go and be replaced with fixed input
        var createDsl = new CreateDSL(mockFileSystem);
        var dslPath = createDsl.WriteLearningWorld(learningWorld);
        var systemUnderTest = new ReadDSL(mockFileSystem);


        //Act
        systemUnderTest.ReadLearningWorld(dslPath);
        
        var learningElementIdent = new IdentifierJson
        {
            type = "FileName",
            value = ele1.Name
        };

        var learningElementIdent2 = new IdentifierJson
        {
            type = "FileName",
            value = ele3.Name
        };

        var learningElementJson = new LearningElementJson
        {
            id = 1,
            identifier = learningElementIdent,
            elementType = "H5P"
        };

        var learningElementJson2 = new LearningElementJson
        {
            id = 2,
            identifier = learningElementIdent2,
            elementType = "H5P"
        };

        var list = new List<LearningElementJson>
        {
            learningElementJson,
            learningElementJson2
        };
        
        var listSpace = systemUnderTest.GetLearningSpaceList();
        var listDslDocument = systemUnderTest.GetDslDocumentList();

        //Assert
        var learningWorldJson = systemUnderTest.GetLearningWorld();
        var h5PElementsList = systemUnderTest.GetH5PElementsList();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListH5PElements, Is.Not.Null);
            Assert.That(h5PElementsList, Is.Not.Null);
            Assert.That(learningWorldJson, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListH5PElements!, Has.Count.EqualTo(list.Count));
            Assert.That(learningWorldJson!.learningElements, Is.Not.Null);
            Assert.That(learningWorldJson.learningSpaces, Is.Not.Null);
            Assert.That(learningWorldJson.learningElements!, Has.Count.EqualTo(createDsl.learningWorldJson!.learningElements!.Count));
            Assert.That(learningWorldJson.learningSpaces!, Has.Count.EqualTo(createDsl.learningWorldJson!.learningSpaces!.Count));
            Assert.That(h5PElementsList!, Has.Count.EqualTo(list.Count));
            Assert.That(listSpace.Count, Is.EqualTo(2));
            Assert.That(listDslDocument.Count, Is.EqualTo(1));
        });
    }

    
}