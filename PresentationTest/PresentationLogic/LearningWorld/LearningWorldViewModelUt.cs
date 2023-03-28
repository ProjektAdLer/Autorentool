using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
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
        var space1 = new LearningSpaceViewModel("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<ILearningSpaceViewModel> { space1 };
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var pathWayConditions = new List<PathWayConditionViewModel> { condition };
        var pathWay = new LearningPathwayViewModel(space1, condition);
        var learningPathways = new List<ILearningPathWayViewModel> { pathWay };

        var systemUnderTest = new LearningWorldViewModel(name, shortname, authors, language, description, goals, 
            unsavedChanges: false, learningSpaces: learningSpaces, pathWayConditions: pathWayConditions,
            learningPathWays: learningPathways);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(language)); 
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.UnsavedChanges, Is.False);
            Assert.That(systemUnderTest.LearningSpaces, Is.EqualTo(learningSpaces));
            Assert.That(systemUnderTest.PathWayConditions, Is.EqualTo(pathWayConditions));
            Assert.That(systemUnderTest.LearningPathWays, Is.EqualTo(learningPathways));
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
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e", layoutViewModel: new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var spaceElement = new LearningElementViewModel("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, space, 6);
        
        space.LearningSpaceLayout.PutElement(0, spaceElement);
        systemUnderTest.LearningSpaces.Add(space);

        Assert.That(systemUnderTest.Workload, Is.EqualTo(6));
    }
    
    [Test]
    public void Points_ReturnsCorrectSum()
    {
        var systemUnderTest = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e", layoutViewModel: new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var spaceElement = new LearningElementViewModel("a",  null!, "d", "e",
            LearningElementDifficultyEnum.Easy, space, 6,7);
        var space2 = new LearningSpaceViewModel("a", "b", "c", "d", "e", layoutViewModel: new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var spaceElement2 = new LearningElementViewModel("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, space, 4,5);
        
        space.LearningSpaceLayout.PutElement(0, spaceElement);
        space2.LearningSpaceLayout.PutElement(0, spaceElement2);
        systemUnderTest.LearningSpaces.Add(space);
        systemUnderTest.LearningSpaces.Add(space2);

        Assert.That(systemUnderTest.Points, Is.EqualTo(12));

        systemUnderTest.LearningSpaces.Remove(space);
        
        Assert.That(systemUnderTest.Points, Is.EqualTo(5));
    }
}