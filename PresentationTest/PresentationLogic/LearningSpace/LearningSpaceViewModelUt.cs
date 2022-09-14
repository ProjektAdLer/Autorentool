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
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var positionX = 5f;
        var positionY = 21f;
        var ele1 = new LearningElementViewModel("a", "b",  null!, "g", "h","i", LearningElementDifficultyEnum.Easy, null, 17,11, 23);
        var ele2 = new LearningElementViewModel("z", "zz",  null!, "z","zz","zzz", LearningElementDifficultyEnum.Hard, null, 444,12, double.MaxValue);
        var learningElements = new List<ILearningElementViewModel> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpaceViewModel(name, shortname, authors, description, goals, learningElements,
            positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElements));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
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
        var element1 = new LearningElementViewModel("a", "b", null!, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, systemUnderTest, 6);
        var element2 = new LearningElementViewModel("abc", "b", null!, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, systemUnderTest, 14);
        
        Assert.That(systemUnderTest.Workload, Is.EqualTo(0));
        
        systemUnderTest.LearningElements.Add(element1);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(6));

        systemUnderTest.LearningElements.Add(element2);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(20));

        systemUnderTest.LearningElements.Remove(element1);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(14));
    }
    
    [Test]
    public void Points_ReturnsCorrectSum()
    {

        var systemUnderTest = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var element1 = new LearningElementViewModel("a", "b", null!, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, systemUnderTest, 6,7);
        var element2 = new LearningElementViewModel("abc", "b", null!, "c", "d", "e",
            LearningElementDifficultyEnum.Easy, systemUnderTest, 14,15);
        
        Assert.That(systemUnderTest.Points, Is.EqualTo(0));
        
        systemUnderTest.LearningElements.Add(element1);
        Assert.That(systemUnderTest.Points, Is.EqualTo(7));

        systemUnderTest.LearningElements.Add(element2);
        Assert.That(systemUnderTest.Points, Is.EqualTo(22));

        systemUnderTest.LearningElements.Remove(element1);
        Assert.That(systemUnderTest.Points, Is.EqualTo(15));
    }
}