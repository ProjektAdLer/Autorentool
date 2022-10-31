using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
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
        var requiredPoints = 10;
        var positionX = 20;
        var positionY = 30;
        var ele1 = new LearningElementViewModel("a", "b",  null!, "url","g", "h","i", LearningElementDifficultyEnum.Easy, null, 17,11, 23);
        var ele2 = new LearningElementViewModel("z", "zz",  null!, "url","z","zz","zzz", LearningElementDifficultyEnum.Hard, null, 444,12, double.MaxValue);
        var inBoundCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 3);
        var outBoundSpace = new LearningSpaceViewModel("a", "z", "d", "b", "t", 3);
        var inBoundObjects = new List<IObjectInPathWayViewModel> { inBoundCondition };
        var outBoundObjects = new List<IObjectInPathWayViewModel> { outBoundSpace };
        var learningElements = new List<ILearningElementViewModel> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpaceViewModel(name, shortname, authors, description, goals, requiredPoints, 
            learningElements, positionX, positionY, inBoundObjects, outBoundObjects);
        
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
            Assert.That(systemUnderTest.InBoundObjects, Is.EqualTo(inBoundObjects));
            Assert.That(systemUnderTest.OutBoundObjects, Is.EqualTo(outBoundObjects));
            Assert.That(systemUnderTest.InputConnectionX, Is.EqualTo(positionX - 6));
            Assert.That(systemUnderTest.InputConnectionY, Is.EqualTo(positionY + 25));
            Assert.That(systemUnderTest.OutputConnectionX, Is.EqualTo(positionX + 106));
            Assert.That(systemUnderTest.OutputConnectionY, Is.EqualTo(positionY + 25));
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
        var element1 = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e",
            LearningElementDifficultyEnum.Easy, systemUnderTest, 6);
        var element2 = new LearningElementViewModel("abc", "b", null!, "url","c", "d", "e",
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
        var element1 = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e",
            LearningElementDifficultyEnum.Easy, systemUnderTest, 6,7);
        var element2 = new LearningElementViewModel("abc", "b", null!, "url","c", "d", "e",
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