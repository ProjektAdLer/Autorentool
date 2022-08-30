using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace PresentationTest.PresentationLogic.LearningSpace;

[TestFixture]
public class LearningSpaceViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var Name = "asdf";
        var Shortname = "jkl;";
        var Authors = "ben and jerry";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var PositionX = 5f;
        var PositionY = 21f;
        var ele1 = new LearningElementViewModel("a", "b", null,  null, "g", "h","i", LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElementViewModel("z", "zz", null,  null, "z","zz","zzz", LearningElementDifficultyEnum.Hard, 444, double.MaxValue);
        var learningElements = new List<ILearningElementViewModel> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpaceViewModel(Name, Shortname, Authors, Description, Goals, learningElements,
            PositionX, PositionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElements));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
        
    }

    [Test]
    public void FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "asf";
        var systemUnderTest = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }
    
    [Test]
    public void Workload_ReturnsCorrectWorkload()
    {

        var systemUnderTest = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var element1 = new LearningElementViewModel("a", "b", systemUnderTest, null, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, 6);
        var element2 = new LearningElementViewModel("abc", "b", systemUnderTest, null, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, 14);
        
        Assert.That(systemUnderTest.Workload, Is.EqualTo(0));
        
        systemUnderTest.LearningElements.Add(element1);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(6));

        systemUnderTest.LearningElements.Add(element2);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(20));

        systemUnderTest.LearningElements.Remove(element1);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(14));
    }
}