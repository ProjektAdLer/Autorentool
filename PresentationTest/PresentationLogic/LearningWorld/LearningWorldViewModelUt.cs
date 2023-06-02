using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;
using TestHelpers;

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
        var topic1 = new TopicViewModel("topic1", false);
        var topic2 = new TopicViewModel("topic2", false);
        var topics = new List<TopicViewModel> {topic1, topic2};
        var space1 = new LearningSpaceViewModel("ff", "ff", "ff", Theme.Campus);
        var learningSpaces = new List<ILearningSpaceViewModel> { space1 };
        var condition = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 1);
        var pathWayConditions = new List<PathWayConditionViewModel> { condition };
        var pathWay = new LearningPathwayViewModel(space1, condition);
        var learningPathways = new List<ILearningPathWayViewModel> { pathWay };

        var systemUnderTest = new LearningWorldViewModel(name, shortname, authors, language, description, goals, 
            unsavedChanges: false, learningSpaces: learningSpaces, pathWayConditions: pathWayConditions,
            learningPathWays: learningPathways, topics: topics);
        
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
            Assert.That(systemUnderTest.Topics, Is.EqualTo(topics));
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
        var space = new LearningSpaceViewModel("a", "d", "e", Theme.Campus, layoutViewModel: new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X30_8L));
        var spaceElement = ViewModelProvider.GetLearningElement(workload:6);
        
        space.LearningSpaceLayout.PutElement(0, spaceElement);
        systemUnderTest.LearningSpaces.Add(space);

        Assert.That(systemUnderTest.Workload, Is.EqualTo(6));
    }
    
    [Test]
    public void Points_ReturnsCorrectSum()
    {
        var systemUnderTest = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var spaceElement = ViewModelProvider.GetLearningElement(points: 7);
        var space2 = ViewModelProvider.GetLearningSpace();
        var spaceElement2 = ViewModelProvider.GetLearningElement("2", points: 5);
        
        space.LearningSpaceLayout.PutElement(0, spaceElement);
        space2.LearningSpaceLayout.PutElement(0, spaceElement2);
        systemUnderTest.LearningSpaces.Add(space);
        systemUnderTest.LearningSpaces.Add(space2);

        Assert.That(systemUnderTest.Points, Is.EqualTo(12));

        systemUnderTest.LearningSpaces.Remove(space);
        
        Assert.That(systemUnderTest.Points, Is.EqualTo(5));
    }
}