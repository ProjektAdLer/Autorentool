using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class ReadDSLUt
{

    [Test]
    public void ReadDSL_ReadLearningWorld_DSLDocumentRead()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        var Name = "asdf";
        var Shortname = "jkl;";
        var Authors = "ben and jerry";
        var Language = "german";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var content1 = new LearningContent("a", ".h5p", new byte[] {0x01, 0x02});
        var content2 = new LearningContent("w", "e", new byte[] {0x02, 0x01});
        var ele1 = new LearningElement("a", "b", "e", content1, "pupup", "g", "h", LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2, "baba", "z", "zz", LearningElementDifficultyEnum.Easy, 444, double.MaxValue);
        var ele3 = new LearningElement("a", "b", "e", content1, "pupup", "g", "h", LearningElementDifficultyEnum.Easy, 17, 23);
        var learningElements = new List<LearningElement> {ele1, ele2};
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        space1.LearningElements.Add(ele3);
        var space2 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<LearningSpace> {space1, space2};

        var learningWorld = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            learningElements, learningSpaces);

        var createDsl = new CreateDSL();

        string dslPath = createDsl.WriteLearningWorld(learningWorld, mockFileSystem);

        var readDSL = new ReadDSL();

        //Act
        readDSL.ReadLearningWorld(dslPath, mockFileSystem);
        var learningElementIdent = new IdentifierJson();
        learningElementIdent.type = "FileName";
        learningElementIdent.value = ele1.Name;

        var learningElementIdent2 = new IdentifierJson();
        learningElementIdent2.type = "FileName";
        learningElementIdent2.value = ele3.Name;

        var learningElementJson = new LearningElementJson();
        learningElementJson.id = 1;
        learningElementJson.identifier = learningElementIdent;
        learningElementJson.elementType = "H5P";

        var learningElementJson2 = new LearningElementJson();
        learningElementJson2.id = 2;
        learningElementJson2.identifier = learningElementIdent2;
        learningElementJson2.elementType = "H5P";

        List<LearningElementJson> list = new List<LearningElementJson>();
        list.Add(learningElementJson);
        list.Add(learningElementJson2);

        //Assert
        Assert.That(readDSL.ListH5PElements.Count, Is.EqualTo(list.Count));
        Assert.That(readDSL.GetLearningWorld().learningElements.Count, Is.EqualTo(createDsl.learningWorldJson.learningElements.Count));
        Assert.That(readDSL.GetLearningWorld().learningSpaces.Count, Is.EqualTo(createDsl.learningWorldJson.learningSpaces.Count));
        Assert.That(readDSL.GetH5PElementsList().Count, Is.EqualTo(list.Count));
    }
}