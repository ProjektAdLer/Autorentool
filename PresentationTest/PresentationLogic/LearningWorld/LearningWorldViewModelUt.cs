using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldViewModelUt
{
    
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var language = "german";
        var description = "very cool element";
        var goals = "learn very many things";
        var content1 = new LearningContentViewModel("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContentViewModel("z", "e", new byte[]{0x05,0x01});
        var ele1 = new LearningElementViewModel("a", "b", content1, "e", "f", "g",LearningElementDifficultyEnum.Easy,null, 17, 32,23);
        var ele2 = new LearningElementViewModel("z", "zz",  content2, "z","zzz", "z",LearningElementDifficultyEnum.Medium, null, 444, 33, double.MaxValue);
        var learningElements = new List<ILearningElementViewModel> { ele1, ele2 };
        var space1 = new LearningSpaceViewModel("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<ILearningSpaceViewModel> { space1 };

        var systemUnderTest = new LearningWorldViewModel(name, shortname, authors, language, description, goals, 
            unsavedChanges: false, learningElements: learningElements, learningSpaces: learningSpaces);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(language)); 
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.UnsavedChanges, Is.False);
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElements));
            Assert.That(systemUnderTest.LearningSpaces, Is.EqualTo(learningSpaces));
        });
    }

    [Test]
    public void FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "awf";
        var systemUnderTest = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }

    [Test]
    public void Workload_ReturnsCorrectWorkload()
    {
        var systemUnderTest = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        var worldElement = new LearningElementViewModel("a", "b", null!, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, systemUnderTest, 4);
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var spaceElement = new LearningElementViewModel("a", "b", null!, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, space, 6);
        
        space.LearningElements.Add(spaceElement);
        systemUnderTest.LearningSpaces.Add(space);
        systemUnderTest.LearningElements.Add(worldElement);
        
        Assert.That(systemUnderTest.Workload, Is.EqualTo(10));

        systemUnderTest.LearningSpaces.Remove(space);
        
        Assert.That(systemUnderTest.Workload, Is.EqualTo(4));
    }
}